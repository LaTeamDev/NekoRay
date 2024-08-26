using System.Drawing;
using System.Numerics;
using NekoLib.Core;

namespace NekoRay.Tools; 

/*
public class ImguiToolsView : Behaviour {
    public Camera DevCamera;

    void Awake() {
        DevCamera = GameObject.AddComponent<Camera>();
        DevCamera.Invoke("Awake");
    }

    void UpdateSize() {
        if (!_sizeChanged) return;
        DevCamera.ChangeSize(_renderSize);
    }

    void Update() {
        UpdateSize();
        UpdateCamera();
    }

    private bool shouldMoveCam;

    public float mouseSensetivity = 1f;
    public float yaw;
    public float pitch;
    public float speed = 2f;
    public float multSpeed = 4f;
    
    public IKeyboard kb;
    void UpdateCamera() {
        if (!shouldMoveCam) return;
        var io = ImGui.GetIO();
        yaw += float.DegreesToRadians(-io.MouseDelta.X *mouseSensetivity);
        pitch += float.DegreesToRadians(-io.MouseDelta.Y*mouseSensetivity);
        pitch = Math.Clamp(pitch, -MathF.PI/2, MathF.PI/2);
        Transform.LocalRotation =
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, yaw) *
            Quaternion.CreateFromAxisAngle(Vector3.UnitX, pitch);
        var sp = speed * (kb.IsKeyPressed(Key.ShiftLeft) ? multSpeed : 1f);
        if ( kb.IsKeyPressed(Key.W)) {
            Transform.LocalPosition += Transform.Forward*Time.DeltaF*sp;
        }
        if (kb.IsKeyPressed(Key.S)) {
            Transform.LocalPosition += Transform.Backward*Time.DeltaF*sp;
        }
        if (kb.IsKeyPressed(Key.D)) {
            Transform.LocalPosition += Transform.Right*Time.DeltaF*sp;
        }
        if (kb.IsKeyPressed(Key.A)) {
            Transform.LocalPosition += Transform.Left*Time.DeltaF*sp;
        }
    }

    private Size _renderSize = GraphicsReferences.ScreenSize;
    private bool _sizeChanged;
    void DrawGui() {
        if (ImGui.Begin("Tools View")) {
            ImGui.BeginChild("ToolRenderer");
            shouldMoveCam = ImGui.IsWindowHovered() && ImGui.IsMouseDown(ImGuiMouseButton.Right);
            var wsize = ImGui.GetWindowSize();
            var size = new Size((int) wsize.X, (int) wsize.Y);
            if (_renderSize != size) {
                _sizeChanged = true;
                _renderSize = size;
            }
            GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            ImGui.Image((nint)DevCamera.RenderTexture.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            ImGui.EndChild();
        }
        ImGui.End();
    }
}*/