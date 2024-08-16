using System.Numerics;
using NekoLib.Core;
using NekoRay.Physics2D;

namespace HotlineSPonyami.Gameplay;

public class Carryable : Component {
    public float Weight;
    public Vector2 _velocity;
    public Rigidbody2D RB;
    public bool Breakable;
}