namespace NekoRay.Tools; 

[AttributeUsage(AttributeTargets.Method)]
public class ConTagHandlerAttribute : Attribute {
    public string Tag;

    public ConTagHandlerAttribute(string tag) {
        Tag = tag;
    }
}