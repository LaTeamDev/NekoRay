using MessagePack;
using NekoLib.Scenes;
using NekoRay.Diagnostics.Model;

namespace NekoRay.Diagnostics.Packets; 

[MessagePackObject]
public class SendSceneHierarchyPacket {
    [Key(0)]
    public List<SerializedScene> Scenes;
    
    public SendSceneHierarchyPacket(IScene scene) {
        
    }
}