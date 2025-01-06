using System.Numerics;
using NekoLib;
using NekoLib.Core;
using ZeroElectric.Vinculum;

namespace NeuroSama.UI;

public class Rect : Behaviour {
    public float Width = 64f;
    public float Height = 64f;
    public Color Color = Raylib.WHITE;
    public Vector2 Origin;
    
    void Render() {
        Raylib.DrawRectanglePro(
            new Rectangle(Transform.Position.X, Transform.Position.Y, Width, Height), 
            new Vector2(Width*Origin.X, Height*Origin.Y), 
            float.RadiansToDegrees(Transform.Rotation.GetEulerAngles().Y), Color);
    }
}