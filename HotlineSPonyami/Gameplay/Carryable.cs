using System.Numerics;
using Box2D.NetStandard.Dynamics.World;
using NekoLib.Core;
using NekoRay.Physics2D;

namespace HotlineSPonyami.Gameplay;

public class Carryable : Component {
    public float Weight;
    public float _time;
    public Vector3 _velocity;
    public Rigidbody2D RB;
}