using NekoLib.Scenes;
using NekoRay;
using NekoRay.Scripting;

namespace LuaScriptingTest;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        SceneManager.LoadScene(new ScriptedScene("lua/scene.lua"));//new YourScene()); //  
    }
}