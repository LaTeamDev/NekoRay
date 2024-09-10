using System.Diagnostics;
using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;

namespace TowerDefence.Gameplay;
public class DebugScene : BaseScene {
    [ShowInInspector]
    private World _world;
    public override void Initialize() {
        _world = this.CreateWorld();
        #region Camera
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        //camera.BackgroundColor = new Color(203, 219, 252, 255);
        camera.IsMain = true;
        camera.Zoom = 2f;
        //camera.Zoom = 2f;
        #endregion
        
        base.Initialize();
    }

    public override void FixedUpdate() {
        _world.Step(Time.FixedDeltaF, 4);
    }
}