using System.Numerics;
using Box2D;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay.Physics2D; 

public class BoxCollider : PolygonCollider {

    public float Width;
    public float Height;
    public override void CreateShape(Body body) {
        body.CreatePolygonShape(ShapeDef,
            Polygon.MakeBox(Width, Height, Vector2.Zero, 
                Transform.Rotation.YawPitchRollAsVector3().Z));
    }
}