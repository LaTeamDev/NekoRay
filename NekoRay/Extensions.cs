using System.Numerics;

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
}