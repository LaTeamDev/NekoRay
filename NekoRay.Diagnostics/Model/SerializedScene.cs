using MessagePack;
using NekoLib.Scenes;

namespace NekoRay.Diagnostics.Model; 

[MessagePackObject]
public class SerializedScene : SerializedNekoObject {
    
    [Key(16)]
    public List<SerializedGameObject> GameObjects = new(); 

    public SerializedScene(IScene scene) {
        Type = scene.GetType();
        Name = scene.Name;
        foreach (var gameObject in scene.RootGameObjects) {
            GameObjects.Add(new SerializedGameObject(gameObject));
        }
    }
    
    public SerializedScene() {}
}