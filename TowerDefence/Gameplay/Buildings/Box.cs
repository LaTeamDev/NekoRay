using System.Numerics;
using Box2D;
using NekoRay;
using NekoRay.Physics2D;
using Serilog;
using ZeroElectric.Vinculum;

namespace TowerDefence.Gameplay.Buildings;

public class Box : Entity, Usable
{
    private BuildingTemplate _building;
    private Rigidbody2D _rigidbody;
    private BoxCollider _collider;
    private Sprite _sprite = Data.GetSprite("textures/orbit_drop.png");

    public override void Initialize()
    {
        _collider = AddComponent<BoxCollider>();
        _collider.Width = _sprite.Width / 2f;
        _collider.Height = _sprite.Height / 2f;
        _collider.Filter = new Filter<PhysicsCategory>
        {
            Category = PhysicsCategory.Buildings,
            Mask = PhysicsCategory.All
        };
        _rigidbody = AddComponent<Rigidbody2D>();
        _rigidbody.Type = BodyType.Kinematic;
        _rigidbody.FixedRotation = true;
        
        base.Initialize();
    }

    public Box(BuildingTemplate building) : base()
    {
        _building = building;
    }
    
    public void Use(Player player)
    {
        player.Inventory.Add(_building);
        Destroy(this);
    }

    public bool CanUse(Player player)
    {
        return true;
    }

    public override void Render()
    {
        base.Draw();
        var origin = new Vector2(_sprite.Width / 2f, _sprite.Height / 2f);
        _sprite.Draw(Transform.Position.ToVector2(), null, origin, 0);
        //Raylib.DrawRectangleV(Transform.Position.ToVector2() - Vector2.One * size / 2f, Vector2.One * size, new Color(255, 128, 0, 255));
    }
}