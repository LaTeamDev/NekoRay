using System.Text;
using MessagePack;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;
using Timer = NekoRay.Timer;

namespace FlappyPegasus;

public class Game : GameBase {

    public override void Load(string[] args) {
        Raylib.SetWindowTitle("Hotline S Ponyami");
    }

    public override void Draw() {
        base.Draw();
        Raylib.DrawFPS(0, 0);
    }

    public override void Update() {
        base.Update();
    }
}
