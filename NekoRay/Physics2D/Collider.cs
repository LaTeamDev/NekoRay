using Box2D;
using Box2D.Interop;

namespace NekoRay.Physics2D; 

public abstract class Collider : Component {
    public ShapeDef ShapeDef = new();
    internal Shape? Shape;

    public abstract ShapeType Type { get; }

    private IShape CurrentStuff {
        get {
            if (Shape is not null) return Shape;
            return CurrentStuff;
        }
    }
    
    public float Friction {
        get => CurrentStuff.Friction;
        set => CurrentStuff.Friction = value;
    }

    public float Restitution {
        get => CurrentStuff.Restitution;
        set => CurrentStuff.Restitution = value;
    }

    public float Density {
        get => CurrentStuff.Density;
        set => CurrentStuff.Density = value;
    }

    public b2Filter Filter {
        get => CurrentStuff.Filter;
        set => CurrentStuff.Filter = value;
    }

    public uint CustomColor {
        get => ShapeDef.CustomColor;
        set => ShapeDef.CustomColor = value;
    }

    public bool IsSensor {
        get => CurrentStuff.IsSensor;
        set => ShapeDef.IsSensor = value;
    }

    public bool EnableSensorEvents {
        get => CurrentStuff.EnableSensorEvents;
        set => CurrentStuff.EnableSensorEvents = value;
    }

    public bool EnableContactEvents {
        get => CurrentStuff.EnableContactEvents;
        set => CurrentStuff.EnableContactEvents = value;
    }

    public bool EnableHitEvents {
        get => CurrentStuff.EnableHitEvents;
        set => CurrentStuff.EnableHitEvents = value;
    }

    public bool EnablePreSolveEvents {
        get => CurrentStuff.EnablePreSolveEvents;
        set => CurrentStuff.EnablePreSolveEvents = value;
    }

    public bool ForceContactCreation {
        get => ShapeDef.ForceContactCreation;
        set => ShapeDef.ForceContactCreation = value;
    }

    public abstract Shape CreateShape(Body body);
}