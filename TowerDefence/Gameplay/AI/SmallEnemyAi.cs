using System.Numerics;
using NekoRay;

namespace TowerDefence.Gameplay.AI;

public class SmallEnemyAi : EnemyAi {
    public Vector2 Objective;
    public override void OnStateChanged(EnemyState old, EnemyState current) {
        if (current == EnemyState.RunAway || current == EnemyState.RunToObjective) {
            Animator.AnimationName = "run";
            return;
        }
        Animator.AnimationName = "idle";
    }

    public float AttackCoolDown = 1f;
    private float _currentTime = 0f;
    public Vector2 MoveDirection = Vector2.One;
    public override void Attack() {
        Enemy.Attack(MoveDirection);
        CurrentState = PreviousState;
    }

    public bool HasBreakableRoadBlock() {
        return false;
        //check if next to it run direction is a breakable
    }

    public override void Run() {
        if (HasBreakableRoadBlock() && _currentTime > AttackCoolDown) {
            CurrentState = EnemyState.Attack;
            return;
        }
        if (MoveDirection.X == 0f && MoveDirection.Y == 0f) {
            CurrentState = EnemyState.Idle;
            return;
        }
        Enemy.Move(MoveDirection);
    }

    public override void RunAway() {
        Enemy.Move(-MoveDirection);
    }

    public override void Idle() {
        if (MoveDirection.X == 0f && MoveDirection.Y == 0f) return;
        if (!HasBreakableRoadBlock()) {
            CurrentState = EnemyState.RunToObjective;
            return;
        }
        if (_currentTime > AttackCoolDown) {
            _currentTime = 0f;
            CurrentState = EnemyState.Attack;
        }
    }

    protected override void Update() {
        MoveDirection = Vector2.Normalize(Objective - Transform.Position.ToVector2());
        if (float.IsNaN(MoveDirection.X)) {
            MoveDirection.X = 0f;
            MoveDirection.Y = 0f;
        }

        if (StormController.IsInStorm == false && CurrentState != EnemyState.RunAway) {
            CurrentState = EnemyState.RunAway;
        }
        base.Update();
    }
}