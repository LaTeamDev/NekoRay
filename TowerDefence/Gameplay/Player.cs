using System.Numerics;
using Box2D;
using Box2D.Interop;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;
using Serilog;
using TowerDefence.Gameplay.Buildings;
using ZeroElectric.Vinculum;
using Object = NekoLib.Core.Object;

namespace TowerDefence.Gameplay;

public class Player : Entity {
    public IController? Controller;
    public Rigidbody2D Rigidbody;
    public CircleCollider Collider;
    public SpriteRenderer2D Sprite;
    public BuildingInventory Inventory { get; private set; }
    public Vector2 TemplatePosition;
    
    public float Speed = 64f;

    public Player() : base("Player") { }

    public override void Initialize()
    {
        Inventory = new BuildingInventory();
        Controller = new WalkController(this);
        Collider = AddComponent<CircleCollider>();
        Collider.Filter = new Filter<PhysicsCategory>()
        {
            Category = PhysicsCategory.Player,
            Mask = PhysicsCategory.Buildings | PhysicsCategory.Enemy
        };
        Collider.Radius = 16f;
        Rigidbody = AddComponent<Rigidbody2D>();
        Rigidbody.Type = BodyType.Dynamic;
        Rigidbody.FixedRotation = true;
        Sprite = this.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        Sprite.Sprite = Data.GetSprite("textures/player/placeholder.png");
        //Rigidbody.LinearDamping = 4f;
        HitboxPosition = Sprite.GameObject.AddChild("Hitbox Position").Transform;
        HitboxPosition.LocalPosition = Vector3.UnitY * -32;
        base.Initialize();
    }

    public NekoLib.Core.Transform HitboxPosition;
    public Vector2 HitboxSize = Vector2.One * 8f;
    
    [ConVariable("r_draw_player_attack")]
    [ConTags("cheat")]
    public static bool DrawPlayerAttackHitbox { get; set; }

    public unsafe void Attack() {
        var pos = HitboxPosition.Position.ToVector2();
        object? context = null;
        Rigidbody.World.OverlapAABB(
            new AABB(pos-HitboxSize, pos+HitboxSize), 
            new QueryFilter<PhysicsCategory> {Mask = PhysicsCategory.Buildings, Category = PhysicsCategory.Trigger},
            static (Shape shape, ref object? ctx) => {
            Log.Verbose("hit {rb}", ((Rigidbody2D)shape.Body.UserData).GameObject.Name);
            return true;
        }, ref context);
    }

    public override void Update() {
        base.Update();
        Controller?.Update();
        try {
            var mousePos = BaseCamera.Main.ScreenToWorld(Input.MousePosition);
            TemplatePosition = mousePos.ToVector2();
            Sprite.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                MathF.Atan2(mousePos.X - Transform.Position.X, Transform.Position.Y - mousePos.Y));
        }
        catch (Exception e) {
            
        }
    }

    public override void Render() {
        base.Render();
        Inventory.DrawTemplate(TemplatePosition);
        if (!DrawPlayerAttackHitbox) return;
        var pos = HitboxPosition.Position.ToVector2();
        var start = pos - HitboxSize;
        var end = pos + HitboxSize;
        //Raylib.DrawCircleV(pos, 8f,Raylib.RED);
        Raylib.DrawRectangleV(start, HitboxSize, Raylib.RED);
    }
}