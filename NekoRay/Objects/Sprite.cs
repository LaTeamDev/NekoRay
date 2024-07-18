using System.Numerics;
using System.Runtime.CompilerServices;

namespace NekoRay; 

public class Sprite : NekoObject {
    public Texture Texture;
    public Rectangle Bounds;
    public float Width => Bounds.Width;
    public float Height => Bounds.Height;

    public Sprite(Texture texture, Rectangle bounds) {
        Texture = texture;
        Bounds = bounds;
    }
    public void Draw(Vector2 destination, Vector2? scale = null, Vector2? origin = null, float rotation = 0f, Color? color = null) {
        scale ??= Vector2.One;
        origin ??= Vector2.Zero;
        color ??= Raylib.WHITE;
        var source = new Rectangle(
            Bounds.X, Bounds.Y,
            Bounds.Width * Math.Sign(scale.Value.X), Bounds.Height * Math.Sign(scale.Value.Y));
        var dest = new Rectangle(
            destination.X, destination.Y,
            Bounds.Width * Math.Abs(scale.Value.X), Bounds.Height * Math.Abs(scale.Value.Y));

        Raylib.DrawTexturePro(
            Texture._texture, 
            source, 
            dest, 
            origin.Value, 
            rotation, 
            color.Value);
    }
}