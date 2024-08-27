using JetBrains.Annotations;

namespace NekoRay.Tools; 

[MeansImplicitUse]
public class CustomInspectorAttribute : Attribute {
    public Type InspectorType;

    public CustomInspectorAttribute(Type inspectorType) {
        InspectorType = inspectorType;
    }
}