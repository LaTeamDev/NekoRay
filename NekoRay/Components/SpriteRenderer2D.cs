using System.Numerics;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class SpriteRenderer2D : Behaviour {
    public Sprite? Sprite;
    public Vector2 Origin = new Vector2(0.5f, 0.5f);
    public RayColor? Color;
    public bool FlipX;
    public bool FlipY;

    void Render() {
        Matrix4x4.Decompose(Transform.GlobalMatrix, out var scale, out var rotation, out var position);
        Sprite?.Draw(
            new Vector2(Transform.Position.X, Transform.Position.Y), 
            new Vector2(scale.X* (FlipX ? -1 : 1), scale.Y* (FlipY ? -1 : 1)),
            new Vector2(Sprite.Width*Origin.X, Sprite.Height*Origin.Y),
            float.RadiansToDegrees(Transform.Rotation.YawPitchRollAsVector3().Z),
            Color);
    }
}