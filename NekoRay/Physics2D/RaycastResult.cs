using System.Numerics;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace NekoRay.Physics2D; 

public class RayCastResult { 
    public Fixture Fixture;
    public Vector2 Point;
    public Vector2 Normal;
    public float Fraction;
}