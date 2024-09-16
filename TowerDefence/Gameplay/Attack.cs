using System.Numerics;

namespace TowerDefence.Gameplay;

public static class Attack
{
    public static void FireSingle(Vector2 origin, Vector2 direction)
    {
        Bullet bullet = new Bullet();
        bullet.Direction = direction;
        bullet.Transform.Position = new Vector3(origin.X, origin.Y, 0);
        bullet.Initialize();
    }
    public static void Fire(Vector2 origin, Vector2 direction, float spread, double time)
    {
        time *= 35;
        double rotation = (Math.Cos(time * 2) - Math.Pow(Math.Sin(time * 1.75), 3) + Math.Sin(time * 0.15f) * 2.0) / 2.0;
        direction = Vector2.TransformNormal(direction, Matrix3x2.CreateRotation((float)double.DegreesToRadians(rotation * spread)));
        FireSingle(origin, direction);
    }
}