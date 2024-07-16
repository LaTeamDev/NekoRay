using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public abstract class Collider : Component {
    public Shape Shape;
    public float Density = 1f;
    public Filter Filter;
    public float Friction;
    public float Restitution;
    public bool IsSensor;

    public virtual FixtureDef GetFixtureDef() {
        return new FixtureDef {
            density = Density,
            filter = Filter,
            friction = Friction,
            restitution = Restitution,
            isSensor = IsSensor,
            userData = this
        };
    }
}