using System.Numerics;
using NekoLib.Core;
using ZeroElectric.Vinculum;
using Timer = NekoRay.Timer;

namespace FlappyPegasus.GameStuff; 

public class Player : Behaviour {
    private float _time;
    void Update() {
        _time += Timer.DeltaF;
        Transform.LocalPosition = 
            new Vector3(MathF.Sin(_time)*128f, MathF.Cos(_time)*128f, 0);
    }
    
    void Render() {
        Raylib.DrawRectangle((int)Transform.Position.X, (int)Transform.Position.Y, 64, 64, Raylib.WHITE);
    }

    void DrawGui() {
        Raylib.DrawText($"player: {MathF.Sin((float)Timer.Time)*128f}", 0, 40, 20, Raylib.RED);
    }
}