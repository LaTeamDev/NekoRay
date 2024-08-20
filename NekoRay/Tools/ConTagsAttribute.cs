namespace NekoRay.Tools; 

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class ConTagsAttribute : Attribute {
    public List<string> Tags;

    public ConTagsAttribute(params string[] tags) {
        Tags = tags.ToList();
    }
}