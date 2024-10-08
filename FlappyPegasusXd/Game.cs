using Box2D;
using FlappyPegasus.GameStuff;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;

namespace FlappyPegasus;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        World.LengthUnitsPerMeter = 64f;
        SaveData.Load();
        SceneManager.LoadScene(new MenuScene());
    }

    public override void Draw() {
        base.Draw();
        Raylib.DrawFPS(0, 0);
        var text = "";
        foreach (var scene in SceneManager.Scenes) {
            text += SceneManager.ActiveScene == scene?"--->":"    ";
            text += scene.Name+"\n";
        }
        Raylib.DrawText($"Current scene: \n{text}",0, 20, 20, Raylib.YELLOW);
    }
}