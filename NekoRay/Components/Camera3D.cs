using System.Numerics;
using NekoRay.Physics2D;

namespace NekoRay;

public class Camera3D : BaseCamera {
    private RayCamera3D _camera3d;

    public bool Orthographic {
        get => _camera3d.Projection == CameraProjection.CAMERA_ORTHOGRAPHIC;
        set => _camera3d.Projection =
            value ? CameraProjection.CAMERA_ORTHOGRAPHIC : CameraProjection.CAMERA_PERSPECTIVE;
    }

    public Camera3D() {
        _camera3d.up = Vector3.UnitZ;
    }
    
    public override Vector2 WorldToScreen(Vector3 position) =>
        Raylib.GetWorldToScreen(position, _camera3d);

    public override Vector3 ScreenToWorld(Vector2 position) {
        throw new NotImplementedException();
    }

    public void LateUpdate() {
        _camera3d.position = Transform.Position;
        _camera3d.target = Transform.Forward;
    }
    
    void Draw() {
        CurrentCamera = this;
        using (RenderTexture.Attach()) {
            Raylib.BeginMode3D(_camera3d);
            var w = Raylib.GetRenderWidth();
            var h = Raylib.GetRenderHeight();
            RlGl.rlViewport(0, 0, RenderWidth, RenderHeight);
            Raylib.ClearBackground(BackgroundColor);
            foreach (var gameObject in GameObject.Scene.GameObjects) {
                RenderGameObject(gameObject);
            }
            RlGl.rlViewport(0, 0, w, h);
            Raylib.EndMode3D();
        }
        CurrentCamera = null;
    }

    public bool DrawGizmo = false;

    private void RenderGameObject(GameObject gameObject) {
        if (!(gameObject.AllTags.Contains("Skip3D") || gameObject.AllTags.Contains("SkipRender"))) 
            gameObject.SendMessage("Render");
        if (DrawGizmo) gameObject.SendMessage("DrawGizmo");
    }
}