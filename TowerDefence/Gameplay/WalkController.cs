using System.Numerics;
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

    [ShowInInspector] private Vector2 _inputDirection;
    [ShowInInspector] private Vector2 _normalizedInput;
    void UpdateInputDirection() {
        _inputDirection = new(
            (Input.IsDown("right") ? 1f : 0f) + (Input.IsDown("left") ? -1f : 0f),
            (Input.IsDown("backward") ? 1f : 0f) + (Input.IsDown("forward") ? -1f : 0f)
        );
        _normalizedInput = Vector2.Normalize(_inputDirection);
        if (!float.IsNaN(_normalizedInput.X)) return;
        _normalizedInput.Y = 0f;
        _normalizedInput.X = 0f;
    }
    
    public void Update() {
        UpdateInputDirection();
        Player.Rigidbody.LinearVelocity = Player.Speed*_normalizedInput;
    }
}