using Box2D;

namespace NekoRay.Physics2D; 

public class CapsuleCollider : Collider {
    public override ShapeType Type => ShapeType.Capsule;
    public override void CreateShape(Body body) {
        throw new NotImplementedException();
    }
}