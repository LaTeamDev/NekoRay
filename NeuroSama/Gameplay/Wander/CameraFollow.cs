using System.ComponentModel.DataAnnotations;
using System.Numerics;
using NekoLib.Core;
using NekoRay;

namespace NeuroSama.Gameplay.Wander;

public class CameraFollow : Behaviour {
    public Transform FollowTarget;
    [Range(0.01f, 1f)]
    public float Smooth = 0.01f;
    public float LimitMin = 0f;
    public float LimitMax = 128f;

    private float _velocity;
    
    void LateUpdate() {
        Transform.Position = Transform.Position with {X= NekoMath.Damp(Transform.Position.X, Math.Clamp(FollowTarget.Position.X, LimitMin, LimitMax), ref _velocity, Smooth)};
    }
}