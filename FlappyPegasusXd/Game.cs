using System.Text;
using Box2D;
using MessagePack;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Diagnostics;
using NekoRay.Diagnostics.Model;
using WatsonWebsocket;
using ZeroElectric.Vinculum;
using Timer = NekoRay.Timer;

namespace FlappyPegasus;

public class Game : GameBase {
    public Server Server;
    public override void Load(string[] args) {
        base.Load(args);
        Server = new Server();
        Raylib.SetWindowTitle("Flappy Pegasus xd");
        World.LengthUnitsPerMeter = 64f;
        KeyPressed += OnKeyPressed;
        SaveData.Load();
        SceneManager.LoadScene(new MenuScene());
        Server.Start();
    }

    private void OnKeyPressed(KeyboardKey key, bool repeat) {
        if (repeat) return;
        SceneManager.InvokeScene("OnKeyPressed", key);
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

    private float _time = 0f;

    public override void Update() {
        base.Update();
        _time += Timer.DeltaF;
        if (_time > 1/10f) {
            var bin = MessagePackSerializer.Serialize(new SerializedScene(SceneManager.ActiveScene));
            Server.Broadcast(bin);
            _time = 0f;
        }
    }
}