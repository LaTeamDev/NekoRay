using System.Reflection;
using MessagePack;
using Object = NekoLib.Core.Object;

namespace NekoRay.Diagnostics.Model; 

[MessagePackObject]
public class SerializedNekoObject {
    [Key(0)] 
    public Type Type;
    
    [Key(1)]
    public Guid Id;

    [Key(2)] 
    public string Name;

    [Key(3)]
    public Dictionary<string, SerializedValue> Fields = new();
    
    private Object Object;

    public void PopulateValues() {
        foreach (var field in Type.GetFields()) {
            Fields.Add(field.Name, new SerializedValue(field.GetValue(Object))); ;
        }
    }

    public SerializedNekoObject() { }
    public SerializedNekoObject(Object obj) {
        Object = obj;
        Name = obj.Name;
        Id = obj.Id;
        Type = obj.GetType();
    }
}