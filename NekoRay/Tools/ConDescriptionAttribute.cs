namespace NekoRay.Tools; 

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter)]
public class ConDescriptionAttribute : Attribute {
    private string _description;
    public string Description => _description;

    public ConDescriptionAttribute(string name) {
        _description = name;
    }
}