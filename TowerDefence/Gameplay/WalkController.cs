using System.Numerics;
using NekoRay;
using NekoRay.Physics2D;

namespace TowerDefence.Gameplay;

public class WalkController : IController {
    public Player Player { get; }
    public Rigidbody2D rb;

    public WalkController(Player player) {
        Player = player;
    }

    private Vector2 _inputDirection = new();
    void UpdateInputDirection() {
        _inputDirection = new(
            (Input.IsDown("right") ? 1f : 0f) + (Input.IsDown("left") ? -1f : 0f),
            (Input.IsDown("backward") ? 1f : 0f) + (Input.IsDown("forward") ? -1f : 0f)
        );
    }
    
    public void Update() {
        UpdateInputDirection();
        //Player.Rigidbody.ApplyForce(Player.Speed*_inputDirection*Time.DeltaF);
    }
}