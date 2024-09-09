using Box2D;
using NekoRay.Physics2D;

namespace TowerDefence.Gameplay;

public class Player : Entity {
    public IController? Controller;
    public Rigidbody2D Rigidbody;
    public CircleCollider Collider;
    public float Speed = 1f;

    public Player() {
        Name = "Player";
    }
    
    public override void Initialize() {
        Controller = new WalkController(this);
        Collider = AddComponent<CircleCollider>();
        Collider.Radius = 16f;
        Rigidbody = AddComponent<Rigidbody2D>();
        Rigidbody.LinearDamping = 1f;
        base.Initialize();
    }

    public override void Update() {
        base.Update();
        Controller?.Update();
    }
    
}