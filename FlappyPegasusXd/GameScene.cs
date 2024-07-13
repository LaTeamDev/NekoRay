using FlappyPegasus.GameStuff;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace FlappyPegasus; 

public class GameScene : BaseScene {

    public OverlayScene OverlayScene;
    public override void Initialize() {
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.BackgroundColor = Raylib.BLUE;
        camera.IsMain = true;

        var player = new GameObject("Player").AddComponent<Player>();
        
        base.Initialize();

        OverlayScene = new PauseScene();
        SceneManager.LoadScene(OverlayScene, SceneLoadMode.Inclusive);
        SceneManager.SetSceneActive(this);
    }

    void OnKeyPressed(KeyboardKey key) {
        if (key != KeyboardKey.KEY_ENTER) return;
        OverlayScene.Toggle();
    }

    public override void Draw() {
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) base.Draw();
    }

    public override void Update() {
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) base.Update();
    }
}