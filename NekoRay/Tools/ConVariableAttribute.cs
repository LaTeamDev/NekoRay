using JetBrains.Annotations;

namespace NekoRay.Tools; 

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Property)]
public class ConVariableAttribute : Attribute {
    private string _name;
    public string Name => _name;

    public ConVariableAttribute(string name) {
        _name = name;
    }
}