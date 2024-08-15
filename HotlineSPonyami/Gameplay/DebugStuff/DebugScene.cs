using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami.Gameplay.DebugStuff; 

public class DebugScene : BaseScene {
    public override void Initialize() {
        this.CreateWorld();
        this.GetWorld().Sleeping = false;
        //this.GetWorld().Gravity = Vector2.UnitY*9.31f;
        #region Camera
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.BackgroundColor = new Color(203, 219, 252, 255);
        camera.IsMain = true;
        camera.Zoom = 2f;
        #endregion

        #region Reticle
        var reticle = new GameObject("Reticle").AddComponent<ReticleController>();
        var reticleRenderer = reticle.GameObject.AddComponent<SpriteRenderer2D>();
        reticleRenderer.Sprite = new Sprite(Data.GetTexture("data/textures/gameui/reticle.png"), new Rectangle(0, 0, 9, 9));
        #endregion

        new GameObject("Debug draw").AddComponent<DrawWorld>();
        
        base.Initialize();
    }

    public override void Update() {
        base.Update();
        this.GetWorld().Step(Timer.DeltaF, 4);
    }
}