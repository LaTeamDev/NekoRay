using Box2D;

namespace NekoRay.Physics2D; 

public class CircleCollider : Collider {
    public override ShapeType Type => ShapeType.Circle;
    private Circle _circle;
    public float Radius {
        get => _circle.Radius;
        set => _circle.Radius = value;
    }

    //TODO: Account for gameobject transform and stuff
    public override void CreateShape(Body body) {
        body.CreateCircleShape(ShapeDef, _circle);
    }
}