using Box2D;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;
using NekoRay.Physics2D;
using NeuroSama.Gameplay.MainMenu;
using NeuroSama.Gameplay.MiniGame;

namespace NeuroSama;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        World.LengthUnitsPerMeter = 64f;
        if (DevMode) {
            SceneManager.LoadScene(new MiniGameScene());
            return;
        }
        SceneManager.LoadScene(new Preloader(new SplashScene(new MenuScene(), "disclaimer", "credits")));
    }

    public override void Update() {
        base.Update();
        Ease.UpdateAll(Time.DeltaF);
    }
}