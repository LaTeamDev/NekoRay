using System.Numerics;
using NekoLib.Core;

namespace Lamoon.Engine.Components; 

public class AudioListener : Behaviour {
    public static Vector3 Position;

    void Update() {
        Position = Transform.Position;
    }
}