using NekoLib.Filesystem;
using rlImGui_cs;
using Serilog;
using Serilog.Events;
using NekoRay.Tools;
using Console = NekoRay.Tools.Console;

namespace NekoRay; 

public abstract class GameBase {
    public static ILogger Log;
    public unsafe virtual void Initlogging() {
        var loggingCfg = ConfigureLogger(new LoggerConfiguration());
        Serilog.Log.Logger = loggingCfg
            .CreateLogger()
            .ForContext("Name", "NekoRay");
        Log = Serilog.Log.Logger.ForContext("Name", GetType().Name);

        //Raylib.SetTraceLogCallback(&RaylibCallback);
    }

    public virtual LoggerConfiguration ConfigureLogger(LoggerConfiguration configuration) {
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
        return configuration
#if DEBUG
            .MinimumLevel.Verbose()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.FromLogContext()
            .WriteTo.GameConsole()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .WriteTo.File($"logs/nekoray{DateTime.Now:yy.MM.dd-hh.MM.ss}.log", LogEventLevel.Verbose, outputTemplate);
    }
    
    public static bool DevMode = false;
    public Console Console;

    public virtual void Load(string[] args) {
        Console.Register<Input>();
        InitConsole(args.Contains("--console"));
        var assemblyFs = new AssemblyFilesystem(GetType().Assembly, GetType().Namespace);
        assemblyFs.Mount();
        RaylibNekoLibFilesystemCompat.Use();
    }

    public virtual void InitConsole(bool enable) {
        SceneManager.LoadScene(new PersistantScene());
        Console = new GameObject("Console").AddComponent<Console>();
        Console.Enabled = DevMode;
        Console.ExecFile("autoexec");
        if (!(DevMode || enable)) return;
        Input.BindCommand(KeyboardKey.KEY_F5, "toggleconsole");
    }

    public event Action? WindowResize;
    public event Action<KeyboardKey, bool>? KeyPressed;

    public virtual void UpdateEvents() {
        if (Raylib.IsWindowResized())
            WindowResize?.Invoke();
        foreach (var key in Enum.GetValues<KeyboardKey>()) {
            var keyPressed = Raylib.IsKeyPressed(key);
            var keyPressedRepeat = Raylib.IsKeyPressedRepeat(key);
            if (keyPressed || keyPressedRepeat) {
                KeyPressed?.Invoke(key, keyPressedRepeat);
            }
        }
    }
    public virtual void Update() {
        SceneManager.Update();
    }

    public virtual void Draw() {
        SceneManager.Draw();
    }

    public delegate void LoopFunction();

    public virtual LoopFunction Run(string[] args) {
        Raylib.InitAudioDevice();
        rlImGui.Setup();
        Load(args);
        return () => {
            Timer.Step();
            UpdateEvents();
            NekoLib.Core.Timer.Global.Update(Timer.DeltaF);
            Input.Update();
            Update();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            Draw();
            rlImGui.Begin(Timer.DeltaF);
            DrawGui();
            rlImGui.End();
            Raylib.EndDrawing();
        };
    }

    public virtual void Shutdown() {
        rlImGui.Shutdown();
    }

    public virtual void DrawGui() {
        SceneManager.InvokeScene("DrawGui");
    }

    public virtual LoopFunction ErrorHandler(Exception msg) {
        var error = msg.ToString();
        Log.Fatal(msg, "An error occured");

        void Draw() {
            Raylib.ClearBackground(Raylib.RAYWHITE);
            Raylib.DrawText(error, 70, 70, 10, Raylib.GRAY);
        }

        return () => {
            Raylib.BeginDrawing();
            Draw();
            Raylib.EndDrawing();
            Thread.Sleep(100);
        };
    }
}