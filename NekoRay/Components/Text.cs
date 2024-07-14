using System.Numerics;
using NekoLib.Core;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Text : Behaviour {
    public Font Font = Font.Default;
    public string TextString = "";
    public Vector2 Origin;
    public float Spacing = 1f;
    public RayColor Color = Raylib.WHITE;
    public virtual void Render() {
        Font.Draw(
            TextString, 
            new Vector2(Transform.Position.X, Transform.Position.Y), 
            Origin, 
            Transform.Rotation.YawPitchRollAsVector3().Z,
            Font.BaseSize*Transform.LocalScale.X,
            Spacing,
            Color);
    }
}