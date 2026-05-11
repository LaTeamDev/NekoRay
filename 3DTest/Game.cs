using NekoLib.Scenes;
using NekoRay;

namespace _3DTest;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        SceneManager.LoadScene(new MainScene());
    }
}