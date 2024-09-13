using System.Numerics;
using Box2D;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;

namespace TowerDefence.Gameplay;

public class WalkController : IController {
    public Player Player { get; }
    public Rigidbody2D rb;

    public WalkController(Player player) {
        Player = player;
    }

    [Range(0.01f, 1f)] public float DampingSize = 0.15f;

    [ShowInInspector] private Vector2 _inputDirection;
    [ShowInInspector] private Vector2 _normalizedInput;
    private Vector2 _dampingVelocity;

    void UpdateInputDirection() {
        _inputDirection = new Vector2(
            (Input.IsDown("right") ? 1f : 0f) + (Input.IsDown("left") ? -1f : 0f),
            (Input.IsDown("backward") ? 1f : 0f) + (Input.IsDown("forward") ? -1f : 0f)
        );
        var currentNormalizedInput = Vector2.Normalize(_inputDirection);
        if (float.IsNaN(currentNormalizedInput.X)) {
            currentNormalizedInput.Y = 0f;
            currentNormalizedInput.X = 0f;
        }
        _normalizedInput = NekoMath.Damp(_normalizedInput, currentNormalizedInput, ref _dampingVelocity, DampingSize);
    }
    
    public void Update() {
        UpdateInputDirection();
        Player.Rigidbody.LinearVelocity = Player.Speed*_normalizedInput;
        if (Input.IsPressed("use"))
        {
            object? context = null;
            Circle circle = new Circle(50);
            bool used = false;
            Player.Rigidbody.World.OverlapCircle(
                ref circle, 
                new Transform() { Position = Player.Transform.Position.ToVector2(), Rotation = Rotation.Identity},
                new QueryFilter<PhysicsCategory> {Mask = PhysicsCategory.All, Category = PhysicsCategory.All},
                 (Shape shape, ref object? ctx) =>
                {
                    Rigidbody2D rb = (Rigidbody2D)shape.Body.UserData;
                    if (rb.GameObject is Usable usable && !used && usable.CanUse(Player))
                    {
                        usable.Use(Player);
                        used = true;
                    }
                    return true;
                }, ref context);
        }

        if (Input.IsPressed("attack1"))
        {
            Player.Inventory.Place(Player.TemplatePosition);
        }
    }
}