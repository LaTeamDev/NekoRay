using MessagePack;

namespace NekoRay.Diagnostics.Model; 

[MessagePackObject]
public class SerializedValue {
    [Key(0)]
    public Type Type;
    [Key(1)]
    public object Value;

    public SerializedValue(object value) {
        Value = value;
        if (value.GetType().IsAssignableTo(typeof(NekoLib.Core.Object))) {
            Value = new SerializedNekoObject((NekoLib.Core.Object)value);
        }

        Type = value.GetType();
    }
}