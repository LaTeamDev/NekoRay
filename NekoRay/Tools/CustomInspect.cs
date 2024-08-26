using JetBrains.Annotations;

namespace NekoRay.Tools; 

[MeansImplicitUse]
public class CustomInspectorAttribute : Attribute {
    public Type InspectType;

    public CustomInspectorAttribute(Type inspectType) {
        InspectType = inspectType;
    }
}