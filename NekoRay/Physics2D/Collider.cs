using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public abstract class Collider : Component {
    public Shape Shape;
    public float Density = 1f;
    public Filter? Filter;
    public float? Friction;
    public float? Restitution;
    public bool IsSensor;

    public virtual FixtureDef GetFixtureDef() {
        var fixtureDef = new FixtureDef {
            isSensor = IsSensor,
            userData = this,
            density = Density
        };
        fixtureDef.filter = Filter??fixtureDef.filter;
        fixtureDef.friction = Friction??fixtureDef.friction;
        fixtureDef.restitution = Restitution??fixtureDef.restitution;
        return fixtureDef;
    }
}