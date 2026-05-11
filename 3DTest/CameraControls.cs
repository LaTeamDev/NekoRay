using System.Numerics;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Transform = NekoLib.Core.Transform;

namespace _3DTest;

public class CameraControls : Behaviour {
    public float Speed = 90f;
    public Transform Target;
    void Awake() {
        // On component initialization (inside Scene.Initialize())
        
    }

    void Start() {
        // On first frame after this component is created and enabled
    }

    void Update() {
        if (Input.IsDown("forward"))
            Transform.Position += Transform.Forward * Time.DeltaF * Speed;
        if (Input.IsDown("backward"))
            Transform.Position += Transform.Backward * Time.DeltaF * Speed;
        if (Input.IsDown("left"))
            Transform.Position += Transform.Left * Time.DeltaF * Speed;
        if (Input.IsDown("right"))
            Transform.Position += Transform.Right * Time.DeltaF * Speed;
        if (Input.IsDown("up"))
            Transform.Position += Transform.Up * Time.DeltaF * Speed;
        if (Input.IsDown("down"))
            Transform.Position += Transform.Down * Time.DeltaF * Speed;
        //if (Input.IsPressed("lookat"))
            Transform.LookAt(Target.Position);
        //Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, float.DegreesToRadians(30f));
    }

    void FixedUpdate() {
        // Every 1/60 of a second, used e.g. for physics
    }

    void LateUpdate() {
        // Every frame after each component have run their Update()
    }
}