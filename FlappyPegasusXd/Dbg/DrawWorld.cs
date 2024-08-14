using NekoLib.Core;
using NekoRay.Physics2D;

namespace FlappyPegasus.Dbg; 

public class DrawWorld : Behaviour {
    void Render() {
        GameObject.Scene.GetWorld().Draw(DebugDraw.Instance);
    }
}