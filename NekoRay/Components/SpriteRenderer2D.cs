using System.Numerics;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class SpriteRenderer2D : Behaviour {
    public Sprite? Sprite;
    public Vector2 Origin = new(0.5f, 0.5f);
    public RayColor Color = Raylib.WHITE;
    public bool FlipX;
    public bool FlipY;
    public Shader? Shader;
    public BlendMode BlendMode = BlendMode.BLEND_ALPHA;

    void Render() {
        if (Sprite is null) return;
        Matrix4x4.Decompose(Transform.GlobalMatrix, out var scale, out _, out _);
        var position = new Vector2(Transform.Position.X, Transform.Position.Y);
        var fullscale = new Vector2(scale.X * (FlipX ? -1 : 1), scale.Y * (FlipY ? -1 : 1));
        var origin = new Vector2(Sprite.Width * Origin.X * scale.X, Sprite.Height * Origin.Y * scale.Y);
        var rotation = float.RadiansToDegrees(Transform.Rotation.YawPitchRollAsVector3().Z);
        using (BlendMode.Attach()) {
            using (Shader?.Attach()) {
                Sprite.Draw(position, fullscale, origin, rotation, Color);
            }
        }
    }
    
    public void ProportionallyScaleByWidth(float width) {
        Transform.LocalScale = new Vector3(width / Sprite.Width);
    }
    
    public void ProportionallyScaleByHeight(float height) {
        Transform.LocalScale = new Vector3(height / Sprite.Height);
    }

    public float Width {
        get => (Sprite?.Width ?? 0f)*Transform.LocalScale.X; //FIXME: this does not account for scale, non local scale should be in nekolib
        set => Transform.LocalScale = Transform.LocalScale with {X = value / Sprite.Width};
    }
    
    public float Height {
        get => (Sprite?.Height ?? 0f)*Transform.LocalScale.Y; //FIXME: this does not account for scale, non local scale should be in nekolib
        set => Transform.LocalScale = Transform.LocalScale with {Y = value / Sprite.Height};
    }
}