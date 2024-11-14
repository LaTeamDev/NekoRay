using Box2D;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;

namespace WebRenderingTest;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        SceneManager.LoadScene(new WebRenderingScene());
    }
}