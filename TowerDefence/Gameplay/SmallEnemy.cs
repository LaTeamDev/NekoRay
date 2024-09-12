using System.Numerics;
using Serilog;

namespace TowerDefence.Gameplay;

public class SmallEnemy : Enemy {
    public override EnemyType Type => EnemyType.Small;

    public SmallEnemy() {
        Speed = 32f;
    }
    public override void Attack(Vector2 direction) {
        Log.Debug("{Guid} wants to attack", GameObject.Id);
    }
}