using System.Numerics;
using NekoLib.Core;
using NekoLib.Extra;
using NekoLib.Scenes;
using NekoRay;
using Serilog;
using ZeroElectric.Vinculum;
using Camera3D = NekoRay.Camera3D;
using Transform = NekoLib.Core.Transform;

namespace _3DTest;

public class MainScene : Scene {
    public override void Initialize() {
        var go1 = new GameObject("holder");
        var gameObject = new GameObject("Camera");
        gameObject.Transform.Parent = go1.Transform;
        var camera = 
            gameObject.AddComponent<Camera3D>();
        camera.Transform.Position = new Vector3(0, 30, -100);
        camera.BackgroundColor = Raylib.SKYBLUE;
        camera.IsMain = true;
        var sphere = new GameObject("Sphere").AddComponent<SphereRenderer>();
        sphere.Color = Raylib.RAYWHITE;
        sphere.Rings = 8;
        sphere.Slices = 16;
        sphere.Radius = 32f;
        var grid = new GameObject("Grid").AddComponent<GridRenderer>();
        var ctrls = camera.GameObject.AddComponent<CameraControls>();
        ctrls.Target = sphere.Transform;
        GenerateSpheres(32, sphere.Transform);
    }

    public static void GenerateSpheres(int count = 16, Transform? parent = null) {
        for (int i = 0; i < count; i++) {
            var sp = new GameObject("Sphere" + i).AddComponent<SphereRenderer>();
            if (parent is not null)
                sp.Transform.Parent = parent;
            sp.Color = Raylib.ColorFromHSV((float)Random.Shared.NextDouble(), 1f, 1f);
            sp.Radius = 2f;
            sp.Rings = 1;
            sp.Slices = 4;
            sp.Enabled = false;
            var moving = sp.GameObject.AddComponent<Moving>();
            moving.Axis = new Vector3((float)Random.Shared.NextDouble(), (float)Random.Shared.NextDouble(),
                (float)Random.Shared.NextDouble());
            moving.Axis = moving.Axis / moving.Axis.LengthSquared();
            moving.Power = 16f * (float)Random.Shared.NextDouble();
            moving.Transform.Position = new Vector3((float)Random.Shared.NextDouble() * 256f-128f,
                (float)Random.Shared.NextDouble() * 256f-128f,
                (float)Random.Shared.NextDouble() * 256f-128f);
            moving.Phase = (float)(Random.Shared.NextDouble() * Math.PI);
            moving.Speed = (float)(Random.Shared.NextDouble() / 4f + 0.75f)*4;
            if (Random.Shared.NextDouble() >= 0.5 && count >= 2) {
                GenerateSpheres(count/2, sp.Transform);
            }
        }
    }

    [ConCommand("gameobjectcount")]
    [ConTags("dev")]
    public static void PrintObject() {
        var count = 0;
        foreach (var scene in SceneManager.Scenes) {
            count += scene.GameObjects.Count;
        }
        Log.Logger.Information("Total Object in all scenes: {Count}, Curreent scene objects: {CurrentCount}", count, SceneManager.ActiveScene.GameObjects.Count);
        
    }
}