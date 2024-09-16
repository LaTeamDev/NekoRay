using System.Numerics;
using Serilog;

namespace TowerDefence.Gameplay;

public class MediumEnemy : Enemy {
    public override EnemyType Type => EnemyType.Medium;

    public MediumEnemy() {
        Speed = 24f;
    }
    public override void Attack(Vector2 direction) {
        Log.Debug("{Guid} wants to attack", GameObject.Id);
    }
}