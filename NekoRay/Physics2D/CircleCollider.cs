using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public class CircleCollider : Collider {
    public CircleCollider() {
        Shape = new CircleShape();
    }

    public float Radius {
        get => ((CircleShape) Shape).Radius;
        set => ((CircleShape) Shape).Radius = value;
    }
    public override FixtureDef GetFixtureDef() {
        var fixture = base.GetFixtureDef();
        fixture.shape = Shape;
        return fixture;
    }
}