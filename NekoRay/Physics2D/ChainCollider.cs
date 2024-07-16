using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public class ChainCollider : Collider {
    public ChainCollider() {
        Shape = new ChainShape();
    }
    public override FixtureDef GetFixtureDef() {
        var fixture = base.GetFixtureDef();
        fixture.shape = Shape;
        return fixture;
    }
}