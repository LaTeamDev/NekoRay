using Box2D;
using ImGuiNET;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami.Gameplay; 

public class PlayerCollector : Behaviour {
    public Inventory Inventory;
    public Rigidbody2D rb;
    public PlayerController Player;
    private HashSet<Shape> _shapeList = new();
    private HashSet<Collider> _colliderList => _shapeList.Select(shape => (Collider) shape.UserData).ToHashSet();

    void Update() {
        var _mousePos = BaseCamera.Main.ScreenToWorld(Input.MousePosition);
        rb.Position = _mousePos.ToVector2();
        if (Input.IsPressed("attack1")) Gameplay.Commands.EntitySpawn("carryable");
        Collect();
    }

    void Collect() {
        if (!Input.IsDown("attack2")) return;
        foreach (var collider in _colliderList) {
            if (!collider.GameObject.HasComponent<Carryable>()) continue;
            var carryable = collider.GameObject.GetComponent<Carryable>();
            if (Inventory.Items.Contains(carryable)) continue;
            if (Inventory.Add(carryable)) {
                collider.GameObject.GetComponent<Rigidbody2D>().Type = BodyType.Kinematic;
                var oldFilter = collider.Filter.For<PhysCategory>();
                oldFilter.Mask &= ~PhysCategory.Prop;
                collider.Filter = oldFilter; 
            }
        }
    }

    void OnSensorEnter2D(SensorEvents.BeginTouchEvent contact) {
        _shapeList.Add(contact.VisitorShape);
    }

    void OnSensorExit2D(SensorEvents.EndTouchEvent contact) {
        _shapeList.Remove(contact.VisitorShape);
    }

    void DrawGui() {
        ImGui.Text(rb.Position.ToString());
        foreach (var contact in _shapeList) {
            ImGui.Text(((Collider)contact.UserData).Id.ToString());
        }
    }

    void Render() {
        if (!Game.DevMode) return;
        foreach (var fixture in _shapeList) {
            var start = Player.RigidBody.Position;
            var end = fixture.Body.Position;
            //var result = GameObject.Scene.GetWorld().RayCast(Player.RigidBody.Position, fixture.Body.Position);
            //var length = (end-start) * result.Fraction;
                //Raylib.DrawLineV(start, result.Point*Physics.MeterScale, Raylib.RED);
        }
    }
}