using System.Numerics;
using System.Runtime.CompilerServices;
using Box2D.Interop;

namespace NekoRay; 

public static class Extensions {
    public static GameObject AddChild(this GameObject gameObject, string name = "GameObject") {
        var scene = SceneManager.ActiveScene;
        SceneManager.SetSceneActive(gameObject.Scene);
        var go = new GameObject(name);
        go.Transform.Parent = gameObject.Transform;
        SceneManager.SetSceneActive(scene);
        return go;
    }

    public static Vector2 ToVector2(this Vector3 vector) => new(vector.X, vector.Y);
    
    public unsafe static Color ToRaylib(this b2HexColor color) {
        return *(Color*)Unsafe.AsPointer(ref color) with { a = 255}; //hope this will be stable enough
    }
    
    public static void RemoveByValue<T, T2>(this Dictionary<T, T2> dictionary, T2 value) where T : notnull {
        foreach (var kv in dictionary) {
            if (kv.Value == null || !kv.Value.Equals(value)) continue;
            dictionary.Remove(kv.Key);
        }
    }

    public static bool IsNullable(this Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
}