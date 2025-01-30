using NekoLib.Core;
using NekoLib.Filesystem;
using NLua;

namespace NekoRay.Scripting;

public static class Extensions {
    public static ScriptedBehaviour AddScriptableComponent(this GameObject gameObject, string scriptPath) {
        var a = gameObject.AddComponent<ScriptedBehaviour>();
        a.ScriptPath = scriptPath;
        return a;
    }

    public static void Invoke(this LuaTable table, string name, params object?[] args) =>
        ((LuaFunction?)table[name])?.Call(args);

    public static object[] DoFileVfs(this Lua l, string path) {
        var file = Files.GetFile(path).ReadBinary(); 
        return l.DoString(file, path);
    }
}