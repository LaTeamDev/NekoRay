using System.Reflection;
using Object = NekoLib.Core.Object;

namespace NekoRay.Tools; 

public class Inspector : Object {
    public object? Target;
    public Type? TargetType => Target?.GetType();

    public virtual void DrawGui() {
        if (Target is null) return;
    }

    public virtual void Initialize() {
        
    }

    public static Inspector? GetInspectorFor(object? target) {
        if (target is null) return null;
        var a = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes()
            ).Where(type => {
                var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
                if (attr is null) return false;
                return target.GetType().IsAssignableTo(attr.InspectType);
            });
        var b = a.FirstOrDefault(type => {
            var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
            if (attr is null) return false;
            return target.GetType() == attr.InspectType;
        })??a.First();
        var instance = Activator.CreateInstance(b);
        if (instance is null) return null;
        ((Inspector) instance).Target = target;
        ((Inspector) instance).Initialize();
        return (Inspector) instance;
    }  
}