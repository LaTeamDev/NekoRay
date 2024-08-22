using NekoLib.Scenes;
using NekoRay.Tools;
using Serilog;

namespace FlappyPegasus.GameStuff; 

public class Commands {
    [ConCommand("start")]
    public static void StartGame() {
        SceneManager.LoadScene(new GameScene());
    }
    
    [ConCommand("leave")]
    public static void LeaveGame() {
        if (SceneManager.ActiveScene.GetType().IsInstanceOfType(typeof(MenuScene))) {
            Log.Error("Attempt to leave a game from main menu");
        }
        SceneManager.LoadScene(new MenuScene());
    }
}