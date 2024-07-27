using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class PlayerController : Behaviour {
    void Render() {
        if (!Game.ToolsMode) return;
        Raylib.DrawCircleV(Transform.Position.ToVector2(), 24f, Raylib.WHITE);
    }
}