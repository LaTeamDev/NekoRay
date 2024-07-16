using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Common;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay.Physics2D; 

public class Rigidbody2D : Behaviour {
    private World _world;
    public World World => _world;
    
    void Awake() {
        _world = GameObject.Scene.GetWorld();
    }

    private bool _isReady = false;
    public bool IsReady => _isReady;
    
    private float _startAngularDamping = 0f;
    private float _startAngularVelocity = 0f;
    private bool _startIsBullet;
    private float _startLinearDamping = 0f;
    private Vector2 _startLinearVelocity;
    private bool _startFixedRotation;
    public BodyType BodyType;

    public float AngularDamping {
        get => !IsReady ? _startAngularDamping : _body.GetAngularDamping();
        set {
            if (!IsReady) _startAngularDamping = value;
            else _body.SetAngularDamping(value);
        }
    }
    
    public float AngularVelocity {
        get => !IsReady ? _startAngularVelocity : _body.GetAngularVelocity();
        set {
            if (!IsReady) _startAngularVelocity = value;
            _body.SetAngularVelocity(value);
        }
    }
    
    public bool IsBullet {
        get => _startIsBullet;
        set {
            if (!IsReady) _startIsBullet = IsBullet;
            else throw new Exception("Could not change type after initialization");
        }
    }

    public float LinearDamping {
        get => !IsReady ? _startLinearDamping : _body.GetLinearDamping();
        set {
            if (!IsReady) _startLinearDamping = value;
            else _body.SetLinearDampling(value);
        }
    }
    
    public Vector2 LinearVelocity {
        get => !IsReady ? _startLinearVelocity : _body.GetLinearVelocity();
        set {
            if (!IsReady) _startLinearVelocity = value;
            else _body.SetLinearVelocity(value);
        }
    }
    
    public bool FixedRotation {
        get => !IsReady ? _startFixedRotation : _body.IsFixedRotation();
        set {
            if (!IsReady) _startFixedRotation = value;
            else _body.SetFixedRotation(value);
        }
    }

    public float Inertia => _body.GetInertia();
    public float Mass => _body.GetMass();

    public Vector2 Position {
        get => _body.GetPosition();
        set => _body.SetTransform(value, Rotation);
    }

    public float Rotation {
        get => _body.GetAngle();
        set => _body.SetTransform(Position, value);
    }

    private Body _body;

    void Start() {
        var bodyDef = new BodyDef {
            angle = Transform.Rotation.Z,
            angularDamping = _startAngularDamping,
            angularVelocity = _startAngularVelocity,
            bullet = _startIsBullet,
            linearDamping = _startLinearDamping,
            allowSleep = true,
            linearVelocity = _startLinearVelocity,
            position = Transform.Position.ToVector2(),
            userData = this,
            fixedRotation = _startFixedRotation,
            type = BodyType
        };
        _body = _world.CreateBody(bodyDef);
        var fixtureDefs = GameObject.GetComponentsInChildren().Where(
            component => component.GetType().IsAssignableTo(typeof(Collider))
        ).Cast<Collider>().Select(collider => collider.GetFixtureDef()).ToList();
        foreach (var fixtureDef in fixtureDefs) {
            _body.CreateFixture(fixtureDef);
        }

        _isReady = true;
    }

    void Update() {
        Transform.Position = new Vector3(Position.X * Physics.MeterScale, Position.Y* Physics.MeterScale, Transform.Position.Z);
        var old = Transform.Rotation.YawPitchRollAsVector3(); 
        Transform.Rotation = Quaternion.CreateFromYawPitchRoll(old.X, old.Y, Rotation);
    }

    void Render() {
        var fixture = _body.GetFixtureList();
        if (fixture.Shape.GetType().IsAssignableTo(typeof(CircleShape))) {
            var shape = (CircleShape) fixture.Shape;
            Raylib.DrawCircleLinesV(Physics.MeterScale * Position, shape.Radius, Raylib.WHITE);
        }
    }
}