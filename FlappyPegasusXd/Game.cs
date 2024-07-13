using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;

namespace FlappyPegasus;

public class Game : GameBase {
    public override void Load(string[] args) {
        Raylib.SetWindowTitle("Flappy Pegasus xd");
        KeyPressed += OnKeyPressed;
        SceneManager.LoadScene(new MenuScene());
    }

    private void OnKeyPressed(KeyboardKey key, bool repeat) {
        if (repeat) return;
        SceneManager.InvokeScene("OnKeyPressed", key);
    }

    public override void Draw() {
        base.Draw();
        Raylib.DrawFPS(0, 0);
        Raylib.DrawText($"Current scene: {SceneManager.ActiveScene.Name}",0, 20, 20, Raylib.YELLOW);
    }
}