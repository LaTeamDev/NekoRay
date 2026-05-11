using System.Numerics;
using NekoLib.Extra;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay;

public class Camera3D : BaseCamera {
    public ZeroElectric.Vinculum.Camera3D _camera = new(Vector3.Zero, Vector3.Zero, Vector3.UnitZ, 90f, CameraProjection.CAMERA_PERSPECTIVE);

    public Color BackgroundColor = new(0, 0, 0, 0);

    public bool Orthographic {
        get => _camera.Projection == CameraProjection.CAMERA_ORTHOGRAPHIC;
        set => _camera.Projection = value ? CameraProjection.CAMERA_ORTHOGRAPHIC : CameraProjection.CAMERA_PERSPECTIVE;
    }

    public float FieldOfView {
        get => _camera.fovy;
        set => _camera.fovy = value;
    }

    void LateUpdate() {
        _camera.position = Transform.Position;
        _camera.up = Transform.Up;
        _camera.target = Transform.Position + Transform.Forward;
    }

    // public MatrixStack MatrixStack = new ();
    
    void Draw() {
        CurrentCamera = this;
        using (RenderTexture.Attach()) {
            //RlGl.rlEnableDepthTest();
            Raylib.BeginMode3D(_camera);
            Raylib.ClearBackground(BackgroundColor);
            foreach (var gameObject in GameObject.Scene.GameObjects) {
                if (gameObject.Transform.Parent is null) //skip all nested gameobjects as they would be drawn recursively
                    DrawGameObject(gameObject);
            }
            //if (DebugDraw.ConvarDraw && GameObject.Scene.TryGetWorld(out var world)) world?.Draw(DebugDraw.Instance);
            Raylib.EndMode3D();
            //RlGl.rlDisableDepthTest();
        }
        CurrentCamera = null;
    }

    void DrawGameObject(GameObject gameObject) {
        MatrixStack.Push(gameObject.Transform.ModelMatrix);
        var tags = gameObject.AllTags;
        if (tags.Contains("Skip3D") || tags.Contains("SkipRender")) return;
        gameObject.SendMessage("Render");
        foreach (var child in gameObject.Transform)
            DrawGameObject(child.GameObject);
        MatrixStack.Pop();
    }

    public override void Dispose() {
        base.Dispose();
        
        RenderTexture.Dispose();
    }

    public override Vector2 WorldToScreen(Vector3 position) {
        return Raylib.GetWorldToScreenEx(position, _camera, RenderWidth, RenderHeight);
    }
    
    public override Vector3 ScreenToWorld(Vector2 position) {
        throw new NotImplementedException();
        // return new Vector3(Raylib.GetScree(position, _camera), 0f);
    }
    [ConVariable("r_wireframes")]
    [ConTags("cheat")]
    public static bool Wireframes { get; set; }
}