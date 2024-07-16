using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public class PolygonCollider : Collider {
    public PolygonCollider() {
        Shape = new PolygonShape();
    }

    public PolygonShape PolygonShape => (PolygonShape) Shape;

    public void Set(in Vector2[] vert) {
        PolygonShape.Set(vert);
    }

    public void SetAsBox(float height, float width) {
        PolygonShape.SetAsBox(height, width);
    }
    
    public override FixtureDef GetFixtureDef() {
        var fixture = base.GetFixtureDef();
        fixture.shape = Shape;
        return fixture;
    }
}