using System.Reflection;
using System.Text;
using NekoLib.Core;
using NekoLib.Filesystem;
using NLua;
using Serilog;
using Object = NekoLib.Core.Object;

namespace NekoRay.Scripting;

public static class LuaHost {
    public static Lua Lua;

    static LuaHost() {
        Lua = new();
        Lua.LoadCLRPackage();
        Lua.State.Encoding = Encoding.UTF8;
        using var packageStream = typeof(LuaHost).Assembly.GetManifestResourceStream("NekoRay.Scripting.package.lua");
        using var ms = new MemoryStream();
        packageStream.CopyTo(ms);
        Lua.DoString(ms.ToArray(), "NekoRay.Scripting.dll/package.lua");
        Lua.DoString("""
                     import ("Serilog", "Serilog")
                     log = Log.Logger:ForContext("name", "Lua")
                     function print(...)
                        Log.Logger:Debug(table.concat({...}," "))
                     end
                     """);
        Lua["_destroyObject"] = new Action<Object, float>(Object.Destroy);
        Lua.DoString("""
                     function destroy(obj, delay)
                        if obj.__scriptable then
                            _destroyObject(obj.__scriptable, delay and delay or 0)
                            return
                        elseif obj.GetType then
                            _destroyObject(obj, delay and delay or 0)
                            return
                        end
                        log.Error("Attempt to destroy lua object")
                     end
                     """);
        Lua["AddComponentToGameObject"] = new Func<GameObject, string, object>((gameObject, type) =>
            typeof(GameObject).GetMethod("AddComponent", BindingFlags.Instance | BindingFlags.Public)
                .MakeGenericMethod(Type.GetType(type)).Invoke(gameObject, null));
        Lua["AddScriptToGameObject"] = new Func<GameObject, string, object>((gameObject, path) =>
            gameObject.AddScriptableComponent(path)
        );
        Lua["GetComponent"] = new Func<GameObject, string, object>((gameObject, type)=>
            typeof(GameObject).GetMethod("GetComponent", BindingFlags.Instance | BindingFlags.Public)
            .MakeGenericMethod(Type.GetType(type)).Invoke(gameObject, null));
    }
}