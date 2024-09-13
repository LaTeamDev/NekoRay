using System.Numerics;
using NekoRay;
using ZeroElectric.Vinculum;

namespace TowerDefence.Gameplay;

public class Bullet : Entity
{
    public Vector2 Direction;
    private double _startTime;

    public override void Initialize()
    {
        _startTime = Time.CurrentTime;
        base.Initialize();
    }

    public override void Update()
    {
        base.Update();
        var vec = Direction * Time.DeltaF * 500;
        Transform.Position += new Vector3(vec.X, vec.Y, 0);
        if (_startTime + 1 < Time.CurrentTime)
        {
            Destroy(this);
        }
    }

    public override void Render()
    {
        base.Render();
        Raylib.DrawCircleV(Transform.Position.ToVector2(), 5f, Raylib.WHITE);
    }
}