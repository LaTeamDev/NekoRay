using Box2D;

namespace NekoRay.Physics2D; 

public class SegmentCollider : Collider {
    public override ShapeType Type => ShapeType.Segment;
    public override void CreateShape(Body body) {
        throw new NotImplementedException();
    }
}