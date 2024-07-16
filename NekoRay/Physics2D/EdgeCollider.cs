using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public class EdgeCollider : Collider {
    public EdgeCollider() {
        Shape = new EdgeShape();
    }

    public EdgeShape EdgeShape => (EdgeShape) Shape;

    public Vector2 Vertex1 {
        get => EdgeShape.Vertex1;
    }
    public Vector2 Vertex2 {
        get => EdgeShape.Vertex2;
    }

    public override FixtureDef GetFixtureDef() {
        var fixture = base.GetFixtureDef();
        fixture.shape = Shape;
        return fixture;
    }
}