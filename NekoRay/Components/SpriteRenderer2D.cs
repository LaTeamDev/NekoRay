using System.Numerics;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class SpriteRenderer2D : Behaviour {
    public Sprite? Sprite;
    public Vector2 Origin = new(0.5f, 0.5f);
    public RayColor? Color;
    public bool FlipX;
    public bool FlipY;
    public Shader? Shader;

    void Render() {
        if (Sprite is null) return;
        Matrix4x4.Decompose(Transform.GlobalMatrix, out var scale, out _, out _);
        var position = new Vector2(Transform.Position.X, Transform.Position.Y);
        var fullscale = new Vector2(scale.X * (FlipX ? -1 : 1), scale.Y * (FlipY ? -1 : 1));
        var origin = new Vector2(Sprite.Width * Origin.X, Sprite.Height * Origin.Y);
        var rotation = float.RadiansToDegrees(Transform.Rotation.YawPitchRollAsVector3().Z);
        using (Shader?.Attach()) {
            Sprite.Draw(position, fullscale, origin, rotation, Color);
        }
    }
}