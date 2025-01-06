using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ImGuiNET;
using Serilog;

namespace NekoRay.Tools;

public abstract class Drawer {
    public virtual void DrawGui(MemberInfo info, object? obj) {
        var header = info.GetCustomAttribute<SeparatorAttribute>();
        if (header is not null) ImGui.SeparatorText(header.Text);
    }
    
    private static Dictionary<Type, Drawer> _drawers;

    public static bool TryGet(Type t, [MaybeNullWhen(false)] out Drawer? value) {
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

    public static bool TryGet<T>(out Drawer? value) {
        return TryGet(typeof(T), out value);
    }
    
    static Drawer() {
        UpdateDrawers();
        HotReloadService.OnUpdateApplication += _ => {
            UpdateDrawers();
        };
    }

    public static void UpdateDrawers() {
        _drawers = new();
        var allCustomDrawers = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes()
            ).Where(type => type.IsDefined(typeof(CustomDrawerAttribute)));
        foreach (var customDrawer in allCustomDrawers) {
            var who = customDrawer.GetCustomAttribute<CustomDrawerAttribute>();
            _drawers[who!.DrawerType] = (Drawer)Activator.CreateInstance(customDrawer);
        }
        Log.Verbose("Drawers successfully updated");
    }
}