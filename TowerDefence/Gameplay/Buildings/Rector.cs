using System.Numerics;
using Box2D;
using NekoRay;
using NekoRay.Physics2D;

namespace TowerDefence.Gameplay.Buildings;

public class Rector : BaseBuilding
{
    private Rigidbody2D _rigidbody;
    private BoxCollider _collider;
    private Sprite _sprite = Data.GetSprite("textures/reactor.png");
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

    public override void Render()
    {
        base.Render();
        var origin = new Vector2(_sprite.Width / 2f, _sprite.Height / 2f);
        _sprite.Draw(Transform.Position.ToVector2(), null, origin, 0);
    }
}