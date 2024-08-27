using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using Microsoft.VisualBasic.FileIO;
using NekoLib;
using NekoLib.Core;
using Serilog;

namespace NekoRay.Tools; 

[CustomInspector(typeof(NekoLib.Core.Object))]
public class ComponentInspector : Inspector {
    public override void Initialize() {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        base.Initialize();
        //TODO: support attributes on getters and setters
        Members = TargetType.GetMembers(flags).Where(info => {
            if (Attribute.IsDefined(info, typeof(HideInInspectorAttribute))) return false;
            var isPublic = info.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo)info).IsPublic,
                MemberTypes.Property => ((PropertyInfo)info).GetAccessors().Any(methodInfo => methodInfo.IsPublic),
                MemberTypes.Event => ((EventInfo) info).GetAddMethod().IsPublic, //TODO: fixme
                MemberTypes.Method => ((MethodInfo)info).IsPublic,
                _ => false
            };
            return isPublic || Attribute.IsDefined(info, typeof(ShowInInspectorAttribute));
        }).ToArray();
    }

    public MemberInfo[] Members;

    private static Dictionary<Type, Drawer> _drawers;

    private static bool TryGetDrawer(Type t, [MaybeNullWhen(false)] out Drawer? value) {
        var searchType = t;
        while (true) {
            if (_drawers.TryGetValue(searchType, out value)) {
                return true;
            }

            if (searchType.BaseType is not null) {
                searchType = searchType.BaseType;
                continue;
            }

            foreach (var interfaceType in searchType.GetInterfaces()) {
                if (_drawers.TryGetValue(interfaceType, out value)) {
                    return true;
                }
            }

            return _drawers.TryGetValue(typeof(object), out value);
        }
    }

    static ComponentInspector() {
        UpdateFieldPropDrawers();
        HotReloadService.OnUpdateApplication += _ => {
            UpdateFieldPropDrawers();
        };
    }

    public static void UpdateFieldPropDrawers() {
        _drawers = new();
        var allCustomDrawers = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes()
            ).Where(type => type.IsDefined(typeof(CustomDrawerAttribute)));
        foreach (var customDrawer in allCustomDrawers) {
            var who = customDrawer.GetCustomAttribute<CustomDrawerAttribute>();
            _drawers[who!.DrawerType] = (Drawer)Activator.CreateInstance(customDrawer);
        }
    }

    public virtual void RenderMember(MemberInfo memberInfo) {
        switch (memberInfo.MemberType) {
            case MemberTypes.Event: RenderEvent((EventInfo) memberInfo); break;
            case MemberTypes.Property: RenderProperty((PropertyInfo) memberInfo); break;
            case MemberTypes.Field: RenderField((FieldInfo) memberInfo); break;
            case MemberTypes.Method: RenderMethod((MethodInfo) memberInfo); break;
            default: ImGui.Text("Error: Unsupported Member type"); break;
        }
    }

    public virtual void RenderEvent(EventInfo eventInfo) {
        ImGui.Text($"event {eventInfo.EventHandlerType?.FullName} {eventInfo.Name}");
    }
    
    public virtual void RenderProperty(PropertyInfo property) {
        if (TryGetDrawer(property.PropertyType, out var value))
            value.DrawGui(property, Target);
    }

    public virtual void RenderField(FieldInfo field) {
        if (TryGetDrawer(field.FieldType, out var value))
            value.DrawGui(field, Target);
    }

    public virtual void RenderMethod(MethodInfo MethodInfo) {
        //ImGui.Text("hi");
        if (MethodInfo.IsSpecialName) return;
        //ImGui.TextDisabled(MethodInfo.ToString());
    }

    public override void DrawGui() {
        var target = (Component) Target;
        foreach (var member in Members) {
            RenderMember(member);
        }
    }
}