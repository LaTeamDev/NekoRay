using NekoLib.Core;

namespace HotlineSPonyami.Gameplay.DebugStuff; 

public class CameraTargetDebug : Behaviour {
    public Transform Target;

    void Update() {
        Transform.Position = Target.Position;
    }
}