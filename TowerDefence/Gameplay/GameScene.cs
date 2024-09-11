using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using TowerDefence.Gameplay.UI;
using TowerDefence.UI;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace TowerDefence.Gameplay;

public class GameScene : BaseScene {
    private World _world;
    public override void Initialize() {
        _world = this.CreateWorld();
        #region Camera
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.BackgroundColor = new Color(203, 219, 252, 255);
        camera.IsMain = true;
        camera.Zoom = 2f;
        #endregion
        
        var player = new Player();

        var ui = new GameObject("UI").AddComponent<Canvas>().GameObject.AddComponent<InGameUi>();
        var progressBar = ui.GameObject.AddChild("ProgressBar").AddComponent<ProgressBar>();
        progressBar.Transform.Position = new Vector3(228, 18, 0);
        progressBar.Transform.LocalScale = new Vector3(512, 16, 1);
        progressBar.GameObject.AddComponent<StormWatcher>();
        
        base.Initialize();
        
    }

    public override void Update() {
        StormController.Update();
        base.Update();
    }
    
    public override void FixedUpdate() {
        base.FixedUpdate();
        _world.Step(Time.FixedDeltaF, 4);
    }
}