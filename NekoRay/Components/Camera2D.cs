using System.Numerics;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay;

public class Camera2D : BaseCamera {
    internal ZeroElectric.Vinculum.Camera2D _camera;

    public float Zoom {
        get => _camera.zoom;
        set => _camera.zoom = value;
    }

    public Color BackgroundColor = new(0, 0, 0, 0);

    void LateUpdate() {
        _camera.target = new Vector2(Transform.Position.X, Transform.Position.Y);
        _camera.offset = new Vector2(Raylib.GetRenderWidth() / 2f, Raylib.GetRenderHeight() / 2f);
        _camera.rotation = Transform.Rotation.YawPitchRollAsVector3().Z;
        if (Zoom <= 0f) Zoom = 1f;
    }

    void Draw() {
        CurrentCamera = this;
        using (RenderTexture.Attach()) {
            Raylib.BeginMode2D(_camera);
            Raylib.ClearBackground(BackgroundColor);
            foreach (var gameObject in GameObject.Scene.GameObjects) {
                if (gameObject.AllTags.Contains("Skip2D") || 
                    gameObject.AllTags.Contains("SkipRender")) continue;
                gameObject.SendMessage("Render");
            }
            Raylib.EndMode2D();
        }
        CurrentCamera = null;
    }

    public override void Dispose() {
        base.Dispose();
        
        RenderTexture.Dispose();
    }

    public override Vector2 WorldToScreen(Vector3 position) {
        return Raylib.GetWorldToScreen2D(position.ToVector2(), _camera);
    }
    
    public override Vector3 ScreenToWorld(Vector2 position) {
        return new Vector3(Raylib.GetScreenToWorld2D(position, _camera), 0f);
    }
}