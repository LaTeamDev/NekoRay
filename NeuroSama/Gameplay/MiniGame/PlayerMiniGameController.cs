using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;
using Transform = NekoLib.Core.Transform;

namespace NeuroSama.Gameplay.MiniGame;

public class PlayerMiniGameController : Behaviour {
    public Collider Collider;
    public Rigidbody2D Rigidbody2D;
    
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

    [ShowInInspector] public float DirectionAngle => MathF.Atan2(_normalizedInput.X,-_normalizedInput.Y);

    void Update() {
        UpdateInputDirection();
        Rigidbody2D.LinearVelocity = Speed*_normalizedInput;
        Rigidbody2D.Rotation = new Rotation(DirectionAngle);
        // if (Input.IsPressed("use"))
        // {
        //     object? context = null;
        //     Circle circle = new Circle(50);
        //     bool used = false;
        //     Rigidbody2D.World.OverlapCircle(
        //         ref circle, 
        //         new Box2D.Transform() { Position = Transform.Position.ToVector2(), Rotation = Rotation.Identity},
        //         new QueryFilter<PhysicsCategory> {Mask = PhysicsCategory.All, Category = PhysicsCategory.All},
        //         (Shape shape, ref object? ctx) =>
        //         {
        //             Rigidbody2D rb = (Rigidbody2D)shape.Body.UserData;
        //             if (rb.GameObject is Usable usable && !used && usable.CanUse(Player))
        //             {
        //                 usable.Use(Player);
        //                 used = true;
        //             }
        //             return true;
        //         }, ref context);
        // }
    }

    public float Speed = 100f;
}