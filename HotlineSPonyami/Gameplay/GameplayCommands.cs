using Box2D;
using HotlineSPonyami.Gameplay.DebugStuff;
using NekoLib;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;
using Serilog;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace HotlineSPonyami.Gameplay; 

public sealed class Commands {
    private Commands() { }

    public delegate GameObject EntityBuilder(GameObject gameObject);

    private static Dictionary<string, EntityBuilder> _ents = new();

    static Commands() {
        AddEntity("player", (gameObject) => {
            var pl = gameObject.AddComponent<PlayerController>();
            pl.Inventory = gameObject.AddComponent<Inventory>();
            pl.Inventory.Capacity = 1f;
            pl.RigidBody = gameObject.AddComponent<Rigidbody2D>();
            pl.RigidBody.Type = BodyType.Dynamic;
            pl.RigidBody.FixedRotation = true;
            var colliderPlayer = pl.GameObject.AddComponent<CircleCollider>();
            colliderPlayer.Radius = 14f;
            colliderPlayer.Filter = new Filter<PhysCategory> {
                Category = PhysCategory.Player
            };
            var collector = pl.GameObject.AddChild("CollectSensor").AddComponent<PlayerCollector>();
            collector.Player = pl;
            collector.Inventory = pl.Inventory;
            collector.rb = collector.GameObject.AddComponent<Rigidbody2D>();
            collector.rb.Type = BodyType.Kinematic;
            var collider = collector.GameObject.AddComponent<CircleCollider>();
            collider.Radius = 16f;
            collider.IsSensor = true;
            collider.Filter = new Filter<PhysCategory> {
                Category = PhysCategory.Trigger,
                Mask = PhysCategory.Prop
            };
            return gameObject;
        });
        AddEntity("camera2d", (gameObject) => {
            gameObject.AddComponent<Camera2D>();
            return gameObject;
        });
        AddEntity("carryable", (gameObject) => {
            var carryable = gameObject.AddComponent<Carryable>();
            carryable.RB = gameObject.AddComponent<Rigidbody2D>();
            carryable.RB.Type = BodyType.Dynamic;
            carryable.Weight = 0.125f;
            var collider = gameObject.AddComponent<CircleCollider>();
            collider.Radius = 3f;
            collider.Filter = new Filter<PhysCategory> {
                Category = PhysCategory.Prop,
                Mask = PhysCategory.Trigger | PhysCategory.LevelGeometry | PhysCategory.Prop
            };
            var huh = collider.Filter;
            return gameObject;
        });
    }
    
    public static void AddEntity(string name, EntityBuilder spawnFunc) {
        _ents.Add(name, spawnFunc);
    }

    [ConCommand("ent_list")]
    public static void ListEntities() {
        var list = "Available entities to spawn: ";
        foreach (var ent in _ents) {
            list += "\n" + ent.Key;
        }
        Log.Information(list);
    }

    [ConCommand("ent_spawn")]
    [ConTags("cheat")]
    public static GameObject? EntitySpawn(string name) {
        if (!_ents.TryGetValue(name, out var builder)) {
            Log.Error("No registered entity builder found");
            return null;
        }
        var gameObject = builder.Invoke(new GameObject(name));
        
        InitializeTree(gameObject);
        gameObject.Transform.Position = BaseCamera.Main.ScreenToWorld(Input.MousePosition);
        
        return gameObject;
    }

    private static void InitializeTree(GameObject gameObject) {
        gameObject.Initialize();
        foreach (var transform in gameObject.Transform) {
            InitializeTree(transform.GameObject);
        }
    }
    
    [ConCommand("cam_track_gameObject")]
    [ConTags("cheat")]
    public static void CameraTarget(string name) {
        var target = SceneManager.ActiveScene.GetGameObjectByName(name);
        if (target is null) {
            Log.Error("No gameObject with name {Name} found", name);
        }

        if (BaseCamera.Main is null) {
            EntitySpawn("ent_camera2d").GetComponent<Camera2D>().IsMain = true;
        }

        CameraTargetDebug a;

        if (!BaseCamera.Main.GameObject.HasComponent<CameraTargetDebug>()) {
            a = BaseCamera.Main.GameObject.AddComponent<CameraTargetDebug>();
        }
        else {
            a = BaseCamera.Main.GameObject.GetComponent<CameraTargetDebug>();
        }

        a.Target = target.Transform;
    }

    [ConCommand("cam_track_stop")]
    [ConTags("cheat")]
    public static void CameraTarget() {
        if (BaseCamera.Main is null) {
            Log.Error("There is no camera in scene");
            return;
        }
        if (!BaseCamera.Main.GameObject.HasComponent<CameraTargetDebug>()) {
            Log.Information("Tracking have been already stoped.");
        }

        NekoLib.Core.Object.Destroy(BaseCamera.Main.GameObject.GetComponent<CameraTargetDebug>());
    }

    [ConCommand("load_scene")]
    [ConDescription("Try to load scene under specified name. the scene should be able to be instantiated with parameterless constructor!!")]
    [ConTags("cheat")]
    public static void LoadScene(string typeName) {
        var type = Type.GetType(typeName);
        if (type is null) {
            Log.Error("Failed to load type {Type}", type);
            return;
        }

        if (!type.IsAssignableTo(typeof(IScene))) {
            Log.Error("{Type} is not a scene!", type);
        }

        SceneManager.LoadScene((IScene)Activator.CreateInstance(type));
    }
    
    [ConCommand("load_scene_debug")]
    [ConDescription("load debug scene")]
    [ConTags("cheat")]
    public static void LoadDebugScene() {
        SceneManager.LoadScene(new DebugScene());
    }
}