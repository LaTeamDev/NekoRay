using System.Numerics;
using Box2D;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;

namespace TowerDefence.Gameplay.Buildings;

public class TurretControler : IController
{
    public IController? PreviousController { get; private set; }
    public Player Player { get; }
    private ManualTurret _turret;
    
    public TurretControler(Player player, ManualTurret turret) {
        Player = player;
        _turret = turret;
        PreviousController = player.Controller;
    }
    public void Update()
    {
        if (Input.IsPressed("use"))
        {
            _turret.FreePlayer();
            return;
        }
        Vector2 mousePos = BaseCamera.Main.ScreenToWorld(Input.MousePosition).ToVector2();
        Vector2 direction = mousePos - _turret.Transform.Position.ToVector2();
        _turret.Direction = Vector2.Normalize(direction);

        if (Input.IsDown("attack1"))
        {
            _turret.Shoot();
        }
    }
}

public class ManualTurret : BaseBuilding, Usable
{
    private Rigidbody2D _rigidbody;
    private BoxCollider _collider;
    private Player? _currentPlayer = null;
    private double _nextShot = 0;

    public Vector2 Direction = Vector2.UnitX;
    
    public void Use(Player player)
    {
        if(!CanUse(player)) return;
        var controller = new TurretControler(player, this);
        player.Rigidbody.LinearVelocity = Vector2.Zero;
        player.Controller = controller;
        _currentPlayer = player;
    }

    public void Shoot()
    {
        if (Time.CurrentTime >= _nextShot)
        {
            _nextShot = Time.CurrentTime + 0.15f + Math.Sin(Time.CurrentTime * 35) * 0.1;
            //_nextShot = Time.CurrentTime + 0.15f;
            Attack.Fire(Transform.Position.ToVector2() + Direction * 25, Direction, 10, Time.CurrentTime);
        }
    }

    public void FreePlayer()
    {
        if(_currentPlayer == null) return;
        if (_currentPlayer.Controller is not TurretControler turretController)
        {
            _currentPlayer = null;
            return;
        }
        _currentPlayer.Controller = turretController.PreviousController;
        _currentPlayer = null;
    }

    public bool CanUse(Player player)
    {
        return _currentPlayer == null;
    }

    public override void Initialize()
    {
        _collider = AddComponent<BoxCollider>();
        _collider.Width = 15f;
        _collider.Height = 15f;
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
        Raylib.DrawRectangleV(Transform.Position.ToVector2() - Vector2.One * 30f / 2f, Vector2.One * 30, Raylib.DARKBROWN);
        Raylib.DrawCircleV(Transform.Position.ToVector2(), 15, Raylib.GRAY);
        Raylib.DrawLineEx(Transform.Position.ToVector2(), Transform.Position.ToVector2() + Direction * 20, 5, Raylib.BLUE);
        base.Draw();
    }
}