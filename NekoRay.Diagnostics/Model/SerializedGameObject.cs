using MessagePack;
using NekoLib.Core;

namespace NekoRay.Diagnostics.Model; 

[MessagePackObject]
public class SerializedGameObject : SerializedNekoObject {


    [Key(16)]
    public List<SerializedGameObject> Children = new();
    
    [Key(17)]
    public List<SerializedComponent> Components = new();

    [Key(18)] 
    public SerializedComponent Transform;
    public SerializedGameObject(GameObject gameObject) {
        Id = gameObject.Id;
        Type = gameObject.GetType();
        Name = gameObject.Name;
        Transform = new SerializedComponent(gameObject.Transform);
        foreach (var transform in gameObject.Transform) {
            Children.Add(new SerializedGameObject(transform.GameObject));
        }
        foreach (var component in gameObject.GetComponents()) {
            Components.Add(new SerializedComponent(component));
        }
    }
    public SerializedGameObject() {}
}