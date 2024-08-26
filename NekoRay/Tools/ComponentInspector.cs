using System.Numerics;
using System.Reflection;
using ImGuiNET;
using NekoLib;
using NekoLib.Core;
using Serilog;

namespace NekoRay.Tools; 

[CustomInspector(typeof(Component))]
public class ComponentInspector : Inspector {
    private event Action meow;
    public override void Initialize() {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        base.Initialize();
        //TODO: support attributes on getters and setters
        Members = TargetType.GetMembers(flags).Where(info => {
            if (Attribute.IsDefined(info, typeof(DontShowInInspectorAttribute))) return false;
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
        if (property.SetMethod is null) ImGui.BeginDisabled();
            
            if (property.GetMethod is null) {
                //TODO: add setter anyway
                ImGui.TextDisabled(property.Name+": No get method");
                return;
            }
        
            var value = property.GetValue(Target);
            var changed = false;
            
            if (property.PropertyType == typeof(Vector3)) {
                var vec = (Vector3) value;
                ImGui.InputFloat3(property.Name, ref vec);
                changed = ImGui.IsItemEdited();
                value = vec; 
            } else if (property.PropertyType == typeof(bool)) {
                var vec = (bool) value;
                ImGui.Checkbox(property.Name, ref vec);
                changed = ImGui.IsItemEdited();
                value = vec;
            } else if (property.PropertyType == typeof(Vector2)) {
                var vec = (Vector2)value;
                ImGui.InputFloat2(property.Name, ref vec);
                changed = ImGui.IsItemEdited();
                value = vec;
            } else if (property.PropertyType == typeof(float)) {
                var vec = (float)value;
                ImGui.InputFloat(property.Name, ref vec);
                changed = ImGui.IsItemEdited();
                value = vec;
            } else if (property.PropertyType == typeof(double)) {
                var vec = (double)value;
                ImGui.InputDouble(property.Name, ref vec);
                changed = ImGui.IsItemEdited();
                value = vec;
            } else if (property.PropertyType == typeof(int)) {
                var vec = (int)value;
                ImGui.InputInt(property.Name, ref vec);
                changed = ImGui.IsItemEdited();
                value = vec;
            } else if (property.PropertyType.IsAssignableTo(typeof(Component))) {
                ImGui.TextDisabled(property.Name+": "+(property.GetValue(Target)??"null"));
            } else if (property.PropertyType == typeof(Quaternion)) {
                var vec = (Quaternion)value;
                var euler = vec.GetEulerAngles();
                ImGui.InputFloat3(property.Name, ref euler);
                changed = ImGui.IsItemEdited();
                if (changed) value = Quaternion.CreateFromYawPitchRoll(euler.X, euler.Y, euler.Z);
            }
            
            if (property.SetMethod is null) {ImGui.EndDisabled(); return;}
            if (changed) property.SetValue(Target, value);
    }

    public virtual void RenderField(FieldInfo field) {
        var name = field.Name + "##" + ((NekoObject)Target).Id;
        var changed = false;
        var value = field.GetValue(Target);
        if (field.FieldType == typeof(Vector3)) {
            var vec = (Vector3)value;
            ImGui.InputFloat3(name, ref vec);
            changed = ImGui.IsItemEdited();
            value = vec;
        } else if (field.FieldType == typeof(Vector2)) {
            var vec = (Vector2)value;
            ImGui.InputFloat2(name, ref vec);
            changed = ImGui.IsItemEdited();
            value = vec;
        } else if (field.FieldType == typeof(bool)) {
            var vec = (bool)value;
            ImGui.Checkbox(name, ref vec);
            changed = ImGui.IsItemEdited();
            value = vec;
        } else if (field.FieldType == typeof(float)) {
            var vec = (float)value;
            ImGui.InputFloat(name, ref vec);
            changed = ImGui.IsItemEdited();
            value = vec;
        } else if (field.FieldType == typeof(double)) {
            var vec = (double)value;
            ImGui.InputDouble(name, ref vec);
            changed = ImGui.IsItemEdited();
            value = vec;
        } else if (field.FieldType == typeof(int)) {
            var vec = (int)value;
            ImGui.InputInt(name, ref vec);
            changed = ImGui.IsItemEdited();
            value = vec;
        } else if (field.FieldType.IsAssignableTo(typeof(Component))) {
            ImGui.TextDisabled(field.Name+": "+field.GetValue(Target));
        }
        if (changed) field.SetValue(Target, value);
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