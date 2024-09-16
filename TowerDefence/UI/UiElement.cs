using System.Numerics;
using NekoLib.Core;

namespace TowerDefence.UI;

public abstract class UiElement : Behaviour {
    public Vector2 Size { get; protected set; }

    public abstract void RecomputeSize();
    
    protected virtual void UpdateSize() {
        if (Transform.LocalScale != prevScale) {
            prevScale = Transform.LocalScale;
            RecomputeSize();
        }
    }

    private Vector3 prevScale;
    protected virtual void Update() {
        UpdateSize();
    }
}