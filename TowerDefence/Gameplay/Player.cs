using Box2D;
using NekoRay.Physics2D;

namespace TowerDefence.Gameplay;

public class Player : Entity {
    public IController? Controller;
    public Rigidbody2D Rigidbody;
    public CircleCollider Collider;
    public float Speed = 64f;

    public Player() {
        Name = "Player";
    }
    
    public override void Initialize() {
        Controller = new WalkController(this);
        Collider = AddComponent<CircleCollider>();
        Collider.Radius = 16f;
        Rigidbody = AddComponent<Rigidbody2D>();
        Rigidbody.Type = BodyType.Kinematic;
        //Rigidbody.LinearDamping = 4f;
        base.Initialize();
    }

    public override void Update() {
        base.Update();
        Controller?.Update();
    }
    
}