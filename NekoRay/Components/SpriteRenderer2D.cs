using System.Numerics;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class SpriteRenderer2D : Behaviour {
    public Sprite? Sprite;
    public Vector2 Origin;
    public RayColor? Color;

    void Render() {
        Matrix4x4.Decompose(Transform.GlobalMatrix, out var scale, out  _, out _);
        Font.Default.Draw(scale.ToString(), new Vector2(Transform.Position.X, Transform.Position.Y));
        Sprite?.Draw(
            new Vector2(Transform.Position.X, Transform.Position.Y), 
            new Vector2(scale.X, scale.Y),
            new Vector2(Sprite.Width*Origin.X, Sprite.Height*Origin.Y),
            Transform.Rotation.YawPitchRollAsVector3().Z,
            Color);
    }
}