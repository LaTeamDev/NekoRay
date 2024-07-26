using System.Diagnostics;
using System.Reflection;
using Tomlyn;

namespace NekoRay;

public static class Program {
    private static bool _shouldQuit = false;
    public static bool ShouldQuit => _shouldQuit;
    public static void Quit() => _shouldQuit = true;
    
    private static NekoRayConf ReadConf(string id = "default") {
        Stream fileStream;
        if (File.Exists($"./{id}.conf.toml"))
            fileStream = new FileStream($"./{id}.conf.toml", FileMode.Open, FileAccess.Read);
        else
            fileStream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream("NekoRay.default.conf.toml");
        var streamReader = new StreamReader(fileStream);
        return Toml.ToModel<NekoRayConf>(streamReader.ReadToEnd());
    }

    private static GameBase GetGame(string Identity) {
        GameBase gameBase;
        if (!File.Exists(Identity + ".dll")) return new NoGame();
        var gameDll = Assembly.LoadFrom(Identity+".dll");
        var gameType = gameDll.GetTypes().FirstOrDefault(type => typeof(GameBase).IsAssignableFrom(type));
        if (gameType is null)
            gameBase = new NoGame();
        else
            gameBase = (GameBase)Activator.CreateInstance(gameType);
        return gameBase;
    }
    
    [DebuggerStepThrough]
    public static int Main(string[] args) {
        Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        var gameId = "default";
        for (int i = 0; i < args.Length; i++) {
            if (args[i] != "-game") continue;
            gameId = args[i + 1];
            break;
        }
        var conf = ReadConf(gameId);
        var game = GetGame(conf.Identity);
        Raylib.SetWindowState(conf.GetFlags());
        Raylib.InitWindow(conf.Width, conf.Height, "NekoRay");
        try {
            var loopFunction = game.Run(args);
            while (!(Raylib.WindowShouldClose() || _shouldQuit)) {
                loopFunction();
            }
            game.Shutdown();
        }
        catch (Exception e) when (!Debugger.IsAttached){
            var loopFunction = game.ErrorHandler(e);
            while (!(Raylib.WindowShouldClose() || _shouldQuit)) {
                loopFunction();
            }
        }
        Raylib.CloseWindow();
        return 0;
    }
}