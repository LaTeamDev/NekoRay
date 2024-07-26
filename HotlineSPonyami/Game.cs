using System.Text;
using HotlineSPonyami.Tools;
using MessagePack;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using Serilog;
using Serilog.Events;
using ZeroElectric.Vinculum;
using Console = HotlineSPonyami.Tools.Console;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami;

public class Game : GameBase {

    public void Initlogging() {
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Verbose()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.FromLogContext()
            .WriteTo.GameConsole()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .WriteTo.File($"logs/nekoray{DateTime.Now:yy.MM.dd-hh.MM.ss}.log", LogEventLevel.Verbose, outputTemplate)
            .CreateLogger()
            .ForContext("Name", "NekoRay");
    }

    public override void Load(string[] args) {
        Initlogging();
        Raylib.SetWindowTitle("Hotline S Ponyami");
        SceneManager.LoadScene(new PersistantScene());
        SceneManager.LoadScene(new TiledScene());
        //new GameObject("Console").AddComponent<Console>();
    }

    public override void Draw() {
        base.Draw();
        Raylib.DrawFPS(0, 0);
    }

    public override void Update() {
        base.Update();
    }
}
