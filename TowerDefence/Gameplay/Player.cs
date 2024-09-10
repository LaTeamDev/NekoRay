using System.Numerics;
using Box2D;
using NekoRay;
using NekoRay.Physics2D;

namespace TowerDefence.Gameplay;

public class Player : Entity {
    public IController? Controller;
    public Rigidbody2D Rigidbody;
    public CircleCollider Collider;
    public SpriteRenderer2D Sprite;
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
        Rigidbody.FixedRotation = true;
        Sprite = this.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        Sprite.Sprite = Data.GetSprite("textures/player/placeholder.png");
        //Rigidbody.LinearDamping = 4f;
        base.Initialize();
    }

    public override void Update() {
        base.Update();
        Controller?.Update();
        try {
            var mousePos = BaseCamera.Main.ScreenToWorld(Input.MousePosition);
            Sprite.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                MathF.Atan2(mousePos.X - Transform.Position.X, Transform.Position.Y - mousePos.Y));
        }
        catch (Exception e) {
            
        }
    }
}