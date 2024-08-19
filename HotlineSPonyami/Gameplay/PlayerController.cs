using System.Numerics;
using Box2D;
using ImGuiNET;
using NekoLib;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class PlayerController : Behaviour {
    public float Speed = 100f;
    private Animation _backLegForward;
    private Animation _frontLegForward;
    private Animation _backLegRight;
    private Animation _frontLegRight;
    private Sprite _body;

    public PlayerController() {
        _backLegForward = AsepriteLoader.Load("textures/entity/player/back_leg_forwad.json").ToAnimation();
        _frontLegForward = AsepriteLoader.Load("textures/entity/player/front_leg_forwad.json").ToAnimation();
        _backLegRight = AsepriteLoader.Load("textures/entity/player/back_leg_forwad.json").ToAnimation();
        _frontLegRight = AsepriteLoader.Load("textures/entity/player/front_leg_forwad.json").ToAnimation();
        _body = Data.GetSprite("textures/entity/player/body.png");
    }

    void Awake() {
        RigidBody.LinearDamping = 12f;
    }

    public Inventory Inventory;

    void Render() {
        if (!Game.DevMode) return;
        
        _body.Draw(
            Transform.Position.ToVector2(),
            Transform.LocalScale.ToVector2(),
            new Vector2(_body.Width/2, _body.Height/2),
            float.RadiansToDegrees(Transform.Rotation.GetEulerAngles().Z)
        );
    }

    private Vector2 _normalizedInput;
    private Vector3 _mousePos;
    public Rigidbody2D RigidBody;

    void Update() {
        var input = new Vector2(
            (Input.IsDown("right") ? 1 : 0) - (Input.IsDown("left") ? 1 : 0),
            (Input.IsDown("down") ? 1 : 0) - (Input.IsDown("up") ? 1 : 0)
        );
        _normalizedInput = Vector2.Normalize(input);
        if (float.IsNaN(_normalizedInput.X))
            _normalizedInput.X = 0f;
        if (float.IsNaN(_normalizedInput.Y))
            _normalizedInput.Y = 0f;
        
        /*Transform.Position = Transform.Position with {
            X = Transform.Position.X + _normalizedInput.X * Timer.DeltaF * Speed, 
            Y = Transform.Position.Y + _normalizedInput.Y * Timer.DeltaF * Speed
        };*/
        RigidBody.LinearVelocity = _normalizedInput * Speed;
        var mousePos = BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition());
        RigidBody.Rotation = new Rotation(MathF.Atan2(mousePos.X-Transform.Position.X, Transform.Position.Y-mousePos.Y));
    }

    void DrawGui() {
        if (!Game.DevMode) return;
        if (ImGui.Begin("player")) {
            ImGui.Text(BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition()).ToString());
            ImGui.Text(Transform.Position.ToString());
            ImGui.Text(Transform.Rotation.GetEulerAngles().ToString());
            ImGui.Text($"Carrying {Inventory.Carrying}/{Inventory.Capacity}");
            foreach (var item in Inventory.Items) {
               ImGui.Text(item.ToString()); 
            }
        }
        ImGui.End();
    }
}