using ImGuiNET;
using NekoLib.Filesystem;
using NekoRay.Physics2D;
using rlImGui_cs;
using Serilog;
using Serilog.Events;
using NekoRay.Tools;
using Console = NekoRay.Tools.Console;

namespace NekoRay; 

public abstract class GameBase {
    private static GameBase? _instance;
    public static GameBase Instance {
        get {
            ArgumentNullException.ThrowIfNull(_instance, nameof(_instance));
            return _instance;
        }
    }

    public GameBase() {
        _instance = this;
    }

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

    [ConVariable("devmode_enabled")] 
    public static bool DevMode => CliOptions.Instance.DevMode;
    
    public ConsoleWindow ConsoleWindow;

    public virtual void Load(string[] args) {
        new AssemblyFilesystem(GetType().Assembly, GetType().Namespace).Mount();
        Compat.RaylibNekoLibFilesystem.Use();
        Console.ExecFile("autoexec");
        InitConsoleWindow(CliOptions.Instance.ConsoleOnStart);
    }

    public virtual void InitConsoleWindow(bool enable) {
        SceneManager.LoadScene(new PersistantScene());
        ConsoleWindow = ConsoleWindow.ToggleConsole();
        ConsoleWindow.Enabled = false;
        ConsoleWindow.Enabled |= DevMode;
        ConsoleWindow.Enabled |= enable;
        
        if (!(DevMode || enable)) return;
        Input.BindCommand(KeyboardKey.KEY_F5, "toggleconsole");
    }

    public event Action? WindowResize;
    public event Action<KeyboardKey, bool>? KeyPressed;

    public virtual void UpdateEvents() {
        if (Raylib.IsWindowResized()) {
            WindowResize?.Invoke();
            SceneManager.InvokeScene("OnWindowResize");
        }
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
        if (Time.DrawFps) Raylib.DrawFPS(0, 0);
    }

    public virtual void FixedUpdate() {
        SceneManager.InvokeScene("FixedUpdate");
    }

    public delegate void LoopFunction();

    public unsafe virtual void SetupImGuiFonts(ImGuiIOPtr io) {
        var cfg = new ImFontConfigPtr(ImGuiNative.ImFontConfig_ImFontConfig());
        cfg.OversampleH = 1;
        cfg.OversampleV = 1;
        io.ConfigWindowsMoveFromTitleBarOnly = true;
        var firaFont = io.Fonts.AddFontFromFilesystemTTF("fonts/Lpix.ttf", 7, cfg);
    }
    public virtual LoopFunction Run(string[] args) {
        Audio.Init();
        rlImGui.SetupUserFonts = SetupImGuiFonts;
        rlImGui.Setup(true, true);
        Load(args);
        return () => {
            Time.Step();
            UpdateEvents();
            NekoLib.Core.Timer.Global.Update(Time.DeltaF);
            Input.Update();
            Update();
            while (Time._fixedTime > Time.FixedDeltaF) {
                Time.IsInFixed = true;
                FixedUpdate();
                Time.IsInFixed = false;
                Time._fixedTime -= Time.FixedDeltaF;
            }
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            Draw();
            rlImGui.Begin(Time.DeltaF);
            if (DevMode) ImGui.DockSpaceOverViewport(0, ImGui.GetMainViewport());
            DrawGui();
            rlImGui.End();
            Raylib.EndDrawing();
        };
    }

    public virtual void Shutdown() {
        rlImGui.Shutdown();
        Audio.Close();
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