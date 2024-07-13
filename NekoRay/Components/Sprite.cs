using System.Numerics;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Sprite2D : Behaviour {
    public Texture Texture;
    public Vector2 Origin;
    public RayColor Color = Raylib.WHITE;

    void Render() {
        Texture.Draw(
            new Rectangle(0, 0, Texture.Width, Texture.Height),
            new Rectangle(Transform.Position.X, Transform.Position.Y, Texture.Width*Transform.LocalScale.X, Texture.Height*Transform.LocalScale.Y),
            new Vector2(Texture.Width*Origin.X, Texture.Height*Origin.Y),
            Transform.Rotation.YawPitchRollAsVector3().Z,
            Color);
    }
}