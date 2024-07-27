using Box2D.NetStandard.Dynamics.Contacts;
using ImGuiNET;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class PlayerCollector : Behaviour {
    public PlayerInventory Inventory;
    public Rigidbody2D rb;

    void Update() {
        var _mousePos = BaseCamera.Main.ScreenToWorld(Raylib.GetMousePosition());
        rb.Position = _mousePos.ToVector2() / Physics.MeterScale;
    }

    void OnBeginSensor2D(Contact contact) {
        if (!Input.IsDown("attack2")) return;
        var contactRb =
            (Rigidbody2D) (contact.FixtureA.UserData == rb ? contact.FixtureB.UserData : contact.FixtureA.UserData);
        if (contactRb.GameObject.HasComponent<Carryable>()) {
            Inventory.Add(contactRb.GameObject.AddComponent<Carryable>());
        }
    }

    void DrawGui() {
        ImGui.Text(rb.Position.ToString());
    }
}