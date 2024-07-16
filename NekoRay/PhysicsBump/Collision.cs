using System.Numerics;

namespace NekoRay.PhysicsBump; 

[Obsolete]
public class Collision {
    internal Collision() {}
    public bool Overlaps;
    public float Ti;
    public Vector2 Move;
    public Vector2 Normal;
    public Vector2 Touch;
    public Rectangle ItemRect;
    public Rectangle OtherRect;
}