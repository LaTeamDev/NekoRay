using System.Text;
using HotlineSPonyami.Tools;
using MessagePack;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami;

public class Game : GameBase {

    public override void Load(string[] args) {
        Raylib.SetWindowTitle("Hotline S Ponyami");
        if (args.Contains("--tools"))
        {
            SceneManager.LoadScene(new EditorScene(32, 32));
            return;
        }
        SceneManager.LoadScene(new TiledScene());
    }

    public override void Draw() {
        base.Draw();
        Raylib.DrawFPS(0, 0);
    }

    public override void Update() {
        base.Update();
    }
}
