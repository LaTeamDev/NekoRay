using System.Numerics;
using ImGuiNET;
using NekoLib;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami.Gameplay; 

public class PlayerController : Behaviour {
    public float Speed = 100f;
    void Render() {
        if (!Game.ToolsMode) return;
        Raylib.DrawRectanglePro(
            new Rectangle(Transform.Position.X, Transform.Position.Y, 24f, 48f),
            new Vector2(12f, 24f),
            float.RadiansToDegrees(Transform.Rotation.GetEulerAngles().Z),
            Raylib.WHITE
            );
    }

    void Update() {
        var input = new Vector2(
            (Input.IsDown("right") ? 1 : 0) - (Input.IsDown("left") ? 1 : 0),
            (Input.IsDown("down") ? 1 : 0) - (Input.IsDown("up") ? 1 : 0)
        );
        var normalizedInput = Vector2.Normalize(input);
        if (float.IsNaN(normalizedInput.X))
            normalizedInput.X = 0f;
        if (float.IsNaN(normalizedInput.Y))
            normalizedInput.Y = 0f;

        Transform.Position = Transform.Position with {
            X = Transform.Position.X + normalizedInput.X * Timer.DeltaF * Speed, 
            Y = Transform.Position.Y + normalizedInput.Y * Timer.DeltaF * Speed
        };
        var mousePos = BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition());
        Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 
            MathF.Atan2(mousePos.X-Transform.Position.X, Transform.Position.Y-mousePos.Y));
    }

    void DrawGui() {
        if (ImGui.Begin("player")) {
            ImGui.Text(Transform.Rotation.GetEulerAngles().ToString());
        }
    }
}