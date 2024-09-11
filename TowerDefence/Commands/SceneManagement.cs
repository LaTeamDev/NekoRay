using NekoLib.Scenes;
using NekoRay.Tools;
using Serilog;
using TowerDefence.Gameplay;
using Console = NekoRay.Tools.Console;

namespace TowerDefence.Commands;

public static partial class Commands {
    public delegate IScene SceneSpawner();
    
    private static Dictionary<string, SceneSpawner> _scenes = new();
    
    [ConCommand("scene")]
    [ConTags("cheat")]
    public static void SceneLoad(string name, SceneLoadMode mode = SceneLoadMode.Inclusive) {
        if (_scenes.TryGetValue(name, out var value)) {
            SceneManager.LoadScene(value(), mode);
            return;
        }
        Log.Error("Scene was not found");
    }

    public static void SceneAdd(string name, SceneSpawner func) {
        _scenes[name] = func;
    }

    public static void DefualtScenes() {
        SceneAdd("debug", () => new DebugScene());
        SceneAdd("game", () => new GameScene());
    }
}