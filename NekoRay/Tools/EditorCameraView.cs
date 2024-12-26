using System.Numerics;
using ImGuiNET;
using JetBrains.Annotations;

namespace NekoRay.Tools;

public class EditorCameraView  : ToolBehaviour {
    private int _renderWidth;
    private int _renderHeight;
    private bool _autoSize = true;
    private bool _fill = false;
    private static int _displayWidth;
    private static int _displayHeight;
    private Camera3D _camera;
    private DevCameraController _controller;
    private bool _shouldMoveCam;

    private void Awake() {
        _camera = GameObject.AddComponent<Camera3D>();
    }
    
    public float MouseSensetivity = 1f;
    public float Yaw;
    public float Pitch;
    public float Speed = 2f;
    public float MultSpeed = 4f;
    
    void Update() {
        UpdateCamera();
    }

    void UpdateCamera() {
        if (!_shouldMoveCam) return;
        var io = ImGui.GetIO();
        Yaw += float.DegreesToRadians(-io.MouseDelta.X *MouseSensetivity);
        Pitch += float.DegreesToRadians(-io.MouseDelta.Y*MouseSensetivity);
        Pitch = Math.Clamp(Pitch, -MathF.PI/2, MathF.PI/2);
        Transform.LocalRotation =
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, Yaw) *
            Quaternion.CreateFromAxisAngle(Vector3.UnitX, Pitch);
        var sp = Speed * (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT_SHIFT) ? MultSpeed : 1f);
        if ( Raylib.IsKeyPressed(KeyboardKey.KEY_W)) {
            Transform.LocalPosition += Transform.Forward*Time.DeltaF*sp;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_S)) {
            Transform.LocalPosition += Transform.Backward*Time.DeltaF*sp;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_D)) {
            Transform.LocalPosition += Transform.Right*Time.DeltaF*sp;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_A)) {
            Transform.LocalPosition += Transform.Left*Time.DeltaF*sp;
        }
    }
    
    void DrawGui() {
        ImGui.Begin("Editor View");
        ImGui.BeginChild("Render");
            if (_camera is not null) {
                var wsize = ImGui.GetWindowSize();
                _camera.RenderWidth = (int)wsize.X;
                _camera.RenderHeight = (int)wsize.Y;
                ImGui.Image((nint)_camera.RenderTexture.Texture._texture.id,wsize, new(0, 1), new(1, 0));
            }
            //else
            //ImGui.Image((nint)Texture.Missing.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            
            ImGui.EndChild();
        ImGui.End();
    }

    [ConCommand("edit_viewer")]
    public static void OpenGameView() => ToolsShared.ToggleTool<GameView>();
}