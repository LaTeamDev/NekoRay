using System.Reflection;
using NekoLib.Core;
using NLua;
using NLua.Method;

namespace NekoRay.Scripting;

public class ScriptedBehaviour : Behaviour {
    public string ScriptPath = "";
    public LuaTable Table;
    private static LuaTable MetaTable = (LuaTable)LuaHost.Lua.DoString(
        """
        return {
            __index = function(self, key) return self.__scriptable and self.__scriptable[key] or null end 
        }
        """, "SciptedBehaviour Metatable")[0];
    void Awake() {
        if (ScriptPath is null or "") 
            throw new NullReferenceException("Script path is null");
        Table = (LuaTable)LuaHost.Lua.GetFunction("require").Call(ScriptPath)[0];
        //LuaHost.Lua.GetFunction("setmetatable").Call(Table, MetaTable);
        Table["__scriptable"] = this;
        Table["GetEnabled"] = new Func<bool>(() => Enabled);
        Table["SetEnabled"] = new Action<bool>(value => Enabled = value);
        Table["Invoke"] = new Action<string, object?>(Invoke);
        Table["GetIsActiveAndEnabled"] = new Func<bool>(() => IsActiveAndEnabled);
        Table["GetGameObject"] = new Func<GameObject>(() => GameObject);
        Table["GetTags"] = new Func<HashSet<string>>(() => Tags);
        Table["GetAllTags"] = new Func<HashSet<string>>(() => AllTags);
        Table["GetTransform"] = new Func<Transform>(() => Transform);
        Table["Broadcast"] = new Action<string, object?>(Broadcast);
        Table["GetName"] = new Func<string>(() => Name);
        Table["SetName"] = new Action<string>((value) => Name = value);
        Table["AddComponent"] = new Func<string, object>((type) =>
            typeof(GameObject).GetMethod("AddComponent", BindingFlags.Instance | BindingFlags.Public)
                .MakeGenericMethod(Type.GetType(type)).Invoke(GameObject, null));
        Table["__scriptable"] = this;
        ((LuaFunction?)Table["Awake"])?.Call(Table);
        Table["__scriptable"] = null;
    }

    public override void Invoke(string methodName, object? o = null) {
        if (methodName == "Awake") Awake();
        else {
            Table["__scriptable"] = this;
            ((LuaFunction?)Table[methodName])?.Call(Table, o);
            Table["__scriptable"] = null;
        }
    }

    public override void Dispose() {
        base.Dispose();
        Invoke("Dispose");
        Table.Dispose();
    }
}