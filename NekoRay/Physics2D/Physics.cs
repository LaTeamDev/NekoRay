using System.Numerics;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Common;
using Box2D.NetStandard.Dynamics.World;

namespace NekoRay.Physics2D; 

public static class Physics {
    private static Dictionary<IScene, World> _sceneWorlds = new();
    public static Vector2 DefaultGravity = new(0f, 9.31f);
    private static NekoRayContactListener ContactListener = new();
    public static float MeterScale = 64f;

    public static void CreateWorld(this IScene scene, Vector2 gravity) {
        if (_sceneWorlds.ContainsKey(scene)) return;
        var world = new World(gravity);
        world.SetContactListener(ContactListener);
        _sceneWorlds[scene] = world;
    }
    public static void CreateWorld(this IScene scene) {
        scene.CreateWorld(DefaultGravity);
    }

    public static World GetWorld(this IScene scene) {
        if (_sceneWorlds.TryGetValue(scene, out var world)) return world;
        throw new Exception($"There is no world for scene {scene.Name}");
    }

    public static AABB ToAABB(this Rectangle rectangle) {
        return new AABB(new Vector2(rectangle.x, rectangle.y),
            new Vector2(rectangle.x + rectangle.width, rectangle.y + rectangle.Height));
    }
}