using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.Fixtures;
using ImGuiNET;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using Serilog;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class PlayerCollector : Behaviour {
    public Inventory Inventory;
    public Rigidbody2D rb;
    public PlayerController Player;
    private HashSet<Fixture> _fixtureList = new();
    private HashSet<Collider> _colliderList => _fixtureList.Select(fixture => (Collider) fixture.UserData).ToHashSet();

    void Update() {
        var _mousePos = BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition());
        rb.Position = _mousePos.ToVector2() / Physics.MeterScale;
        if (Input.IsPressed("attack1")) Gameplay.Commands.EntitySpawn("carryable");
        Collect();
    }

    void Collect() {
        if (!Input.IsDown("attack2")) return;
        foreach (var collider in _colliderList) {
            if (!collider.GameObject.HasComponent<Carryable>()) return;
            var carryable = collider.GameObject.GetComponent<Carryable>();
            if (Inventory.Items.Contains(carryable)) return;
            if (Inventory.Add(carryable)) {
                collider.Filter.maskBits = (ushort)(((PhysCategory)collider.Filter.maskBits) & ~PhysCategory.Prop);
            }
        }
    }

    void OnBeginSensor2D(Contact contact) {
        Log.Debug("sensoringRn");

        var contactFixture =
            contact.FixtureA.UserData == rb ? contact.FixtureA : contact.FixtureB;
        _fixtureList.Add(contactFixture);
    }

    void OnEndSensor2D(Contact contact) {
        var contactFixture =
            contact.FixtureA.UserData == rb ? contact.FixtureA : contact.FixtureB;
        _fixtureList.Remove(contactFixture);
    }

    void DrawGui() {
        ImGui.Text(rb.Position.ToString());
        foreach (var contact in _fixtureList) {
            ImGui.Text(contact.ToString());
        }
    }

    void Render() {
        if (!Game.ToolsMode) return;
        foreach (var fixture in _fixtureList) {
            var start = Player.RigidBody.Position * Physics.MeterScale;
            var end = fixture.Body.Position * Physics.MeterScale;
            var result = GameObject.Scene.GetWorld().RayCast(Player.RigidBody.Position, fixture.Body.Position);
            //var length = (end-start) * result.Fraction;
                Raylib.DrawLineV(start, result.Point*Physics.MeterScale, Raylib.RED);
        }
    }
}