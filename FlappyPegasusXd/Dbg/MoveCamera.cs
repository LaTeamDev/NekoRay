using NekoLib.Core;
using ZeroElectric.Vinculum;

namespace FlappyPegasus.Dbg; 

public class MoveCamera : Behaviour {
    void Update() {
        if (!Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_MIDDLE)) return;
        var delta = Raylib.GetMouseDelta();
        Transform.Position = Transform.Position with {
            X = Transform.Position.X - delta.X, Y = Transform.Position.Y - delta.Y
        };
    }
}