using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NekoRay.Tools;

namespace TowerDefence.UI;

public abstract class UiBehaviour : Behaviour {
    [ConVariable("ui_zoom")] 
    public static float Zoom { get; set; } = 1f;

    void Update() {
        Transform.LocalScale = Vector3.One * Zoom;
    }
}