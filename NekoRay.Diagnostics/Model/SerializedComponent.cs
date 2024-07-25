using NekoLib.Core;

namespace NekoRay.Diagnostics.Model; 

public class SerializedComponent : SerializedNekoObject {
    public SerializedComponent(Component component) {
        Type = component.GetType();
    }
    public SerializedComponent() {}
}