using System.Numerics;
using NekoLib.Core;
using NekoRay.Physics2D;

namespace TowerDefence.Gameplay;

public abstract class Enemy : Behaviour {
    public abstract EnemyType Type { get; }
    public float Speed = 1f;
    protected Rigidbody2D Rb;
    
    protected virtual void Awake() {
        Rb = GameObject.GetComponent<Rigidbody2D>();
    }
    public abstract void Attack(Vector2 direction);

    public virtual void Move(Vector2 direction) {
        Rb.LinearVelocity = direction * Speed;
    }

    protected virtual void Update() {
        Rb.LinearVelocity = Vector2.Zero;
    }
}