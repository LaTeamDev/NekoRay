using System.Numerics;
using Box2D;

namespace NekoRay.Physics2D; 

public static class Physics {
    private static Dictionary<IScene, World> _sceneWorlds = new();
    public static Vector2 DefaultGravity = new(0f, 9.31f);

    public static void CreateWorld(this IScene scene, Vector2 gravity) {
        if (_sceneWorlds.ContainsKey(scene)) return;
        var world = new NekoRayWorld(new WorldDef {
            Gravity = gravity
        });
        //world.SetContactListener(ContactListener);
        _sceneWorlds[scene] = world;
    }
    public static void CreateWorld(this IScene scene) {
        scene.CreateWorld(DefaultGravity);
    }

    /*public static RayCastResult RayCast(this World world, Vector2 start, Vector2 end ) {
        var result = new RayCastResult();
        world.RayCast((fixture, point, normal, fraction) => {
            result.Fixture = fixture;
            result.Point = point;
            result.Normal = normal;
            result.Fraction = fraction;
        }, start, end);
        return result;
    }*/

    public static World GetWorld(this IScene scene) {
        if (_sceneWorlds.TryGetValue(scene, out var world)) return world;
        throw new Exception($"There is no world for scene {scene.Name}");
    }

    public static AABB ToAABB(this Rectangle rectangle) =>
        new AABB {
            LowerBound = new Vector2(rectangle.x, rectangle.y),
            UpperBound = new Vector2(rectangle.x + rectangle.width, rectangle.y + rectangle.Height)
        };
}