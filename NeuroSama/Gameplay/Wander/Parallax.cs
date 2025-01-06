using System.Numerics;
using NekoLib.Core;
using NekoRay.Tools;

namespace NeuroSama.Gameplay.Wander;

public class Parallax : Behaviour {
    public Transform Target;
    public Vector3 InitialPos;
    [Range(1f)] 
    public float Factor;

    void Awake() {
        InitialPos = Transform.Position;
    }
    
    void Update() {
        Transform.Position = InitialPos*(1-Factor) + Target.Position*Factor;
    }
}