using System.Reflection;
using NekoLib.Core;
using NLua;

namespace NekoRay.Scripting;

public class ScriptedScene(string path) : Scene {
    public LuaTable Table;
    public override void Initialize() {
        Table = (LuaTable)LuaHost.Lua.DoFileVfs(path)[0];
        Table.Invoke("Initialize");
        base.Initialize();
    }

    public override void Draw() {
        Table.Invoke("PreDraw");
        base.Draw();
        Table.Invoke("PostDraw");
    }
    
    public override void Update() {
        Table.Invoke("PreUpdate");
        base.Update();
        Table.Invoke("PostUpdate");
    }

    public override void OnWindowResize() {
        Table.Invoke("OnWindowResize");
        base.OnWindowResize();
    }

    public override void Dispose() {
        base.Dispose();
        Table.Invoke("Dispose");
        Table.Dispose();
    }

    public override void DrawGui() {
        Table.Invoke("PreDrawGui");
        base.DrawGui();
        Table.Invoke("PostDrawGui");
    }

    public override void FixedUpdate() {
        Table.Invoke("PreFixedUpdate");
        base.FixedUpdate();
        Table.Invoke("PostFixedUpdate");
    }
}