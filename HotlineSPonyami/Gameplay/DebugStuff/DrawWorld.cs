using NekoLib.Core;
using NekoRay.Physics2D;

namespace HotlineSPonyami.Gameplay.DebugStuff; 

public class DrawWorld : Behaviour {
    void Render() {
        GameObject.Scene.GetWorld().Draw(DebugDraw.Instance);
    }
}