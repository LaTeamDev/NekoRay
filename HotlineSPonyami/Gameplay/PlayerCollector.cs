using Box2D.NetStandard.Dynamics.Contacts;
using ImGuiNET;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using Serilog;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class PlayerCollector : Behaviour {
    public PlayerInventory Inventory;
    public Rigidbody2D rb;
    private HashSet<Collider> _contactList = new();

    void Update() {
        var _mousePos = BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition());
        rb.Position = _mousePos.ToVector2() / Physics.MeterScale;
        if (Input.IsPressed("attack1")) Gameplay.Commands.EntitySpawn("carryable");
        Collect();
    }

    void Collect() {
        if (!Input.IsDown("attack2")) return;
        foreach (var collider in _contactList) {
            if (!collider.GameObject.HasComponent<Carryable>()) return;
            var carryable = collider.GameObject.GetComponent<Carryable>();
            collider.Filter.maskBits = (ushort)(((PhysCategory)collider.Filter.maskBits) & ~PhysCategory.Prop);
            if (Inventory.Items.Contains(carryable)) return;
            Inventory.Add(carryable);
        }
    }

    void OnBeginSensor2D(Contact contact) {
        Log.Debug("sensoringRn");
        
        var contactCollider =
            (Collider) (contact.FixtureA.UserData == rb ? contact.FixtureA.UserData : contact.FixtureB.UserData);
        _contactList.Add(contactCollider);
    }
    void OnEndSensor2D(Contact contact) {
        var contactCollider =
            (Collider) (contact.FixtureA.UserData == rb ? contact.FixtureA.UserData : contact.FixtureB.UserData);
        _contactList.Remove(contactCollider);
    }

    void DrawGui() {
        ImGui.Text(rb.Position.ToString());
        foreach (var contact in _contactList) {
            ImGui.Text(contact.ToString());
        }
    }
}