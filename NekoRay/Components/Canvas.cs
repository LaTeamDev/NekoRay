using NekoLib.Core;

namespace NekoRay; 

public class Canvas : Behaviour {
    void Awake() {
        GameObject.Tags.Add("SkipRender");
    }

    void DrawGui() {
        GameObject.Broadcast("Render");
    }
}