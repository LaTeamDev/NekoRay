using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using HotlineSPonyami.Tools;
using HotlineSPonyami.Interop;
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
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void RaylibCallback(int msgTypeRaw, sbyte* textPtr, sbyte* args) {
        var text = Marshal.PtrToStringUTF8((IntPtr)textPtr);
        var format = Formatting.ConvertPrintfToCSharp(text);
        var msgType = (TraceLogLevel) msgTypeRaw;
        switch (msgType) {
            case TraceLogLevel.LOG_INFO:
                Log.Information(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_TRACE:
                Log.Verbose(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_DEBUG:
                Log.Debug(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_WARNING:
                Log.Warning(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_ERROR:
                Log.Warning(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_FATAL:
                Log.Fatal(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
        }
    }

    public unsafe void Initlogging() {
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


        //Raylib.SetTraceLogCallback(&RaylibCallback);
    }

    public Console Console;
    public bool ToolsMode = false;

    public override void Load(string[] args) {
        Initlogging();
        Raylib.SetWindowTitle("Hotline S Ponyami");
        SceneManager.LoadScene(new PersistantScene());
        Console.Register<EditorScene>();
        ToolsMode = args.Contains("--tools");
        Console = new GameObject("Console").AddComponent<Console>();
        Console.Enabled = ToolsMode;
        if (!ToolsMode)
        {
            SceneManager.LoadScene(new TiledScene());
        }
        if (!ToolsMode && !args.Contains("--console")) return;
        KeyPressed += (key, b) => {
            if (key == KeyboardKey.KEY_F5)
                Console.Enabled = !Console.Enabled;
        };
    }
    public override void Draw() {
        base.Draw();
        Raylib.DrawFPS(0, 0);
    }

    public override void Update() {
        base.Update();
    }
}
