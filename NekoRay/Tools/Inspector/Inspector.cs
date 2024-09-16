using System.Reflection;
using Object = NekoLib.Core.Object;

namespace NekoRay.Tools; 

public class Inspector : Object {
    public object? Target;
    public Type? TargetType => Target?.GetType();

    public virtual void DrawGui() {
        
    }

    public virtual void Initialize() {
        
    }

    private static int CalcInspectorScore(Type obj, Type inspector, int prevScore = 0) {
        if (obj == inspector) {
            return prevScore;
        }
        if (obj.BaseType is null) {
            return int.MaxValue;
        }
        return CalcInspectorScore(obj.BaseType, inspector, prevScore+1);
    }

    public static Inspector? GetInspectorFor(object? target) {
        if (target is null) return null;
        var a = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes()
            ).Where(type => {
                var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
                if (attr is null) return false;
                return target.GetType().IsAssignableTo(attr.InspectorType);
            });
        var lastInspectorScore = int.MaxValue;
        Type? bestMatchedInspectorType = null;
        foreach (var type in a) {
            var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
            if (attr is null) continue;
            var currentScore = CalcInspectorScore(target.GetType(), attr.InspectorType);
            if (currentScore < lastInspectorScore) {
                lastInspectorScore = currentScore;
                bestMatchedInspectorType = type;
            }
        }
        var instance = Activator.CreateInstance(bestMatchedInspectorType);
        if (instance is null) return null;
        ((Inspector) instance).Target = target;
        ((Inspector) instance).Initialize();
        return (Inspector) instance;
    }  
}