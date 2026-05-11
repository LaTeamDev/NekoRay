using System.Numerics;
using NekoLib.Core;
using NekoRay;

namespace _3DTest;

public class Moving : Behaviour {
    private Vector3 _startPosition;
    public Vector3 Axis = Vector3.UnitZ;
    public float Power = 1f;
    public float Phase = 0f;
    public float Speed = 1f;
    public void Start() {
        _startPosition = Transform.LocalPosition;
    }
    public void Update() {
        Transform.LocalPosition = _startPosition + Axis * (float)Math.Sin(Time.CurrentTime*Speed+Phase) * Power;
    }
}