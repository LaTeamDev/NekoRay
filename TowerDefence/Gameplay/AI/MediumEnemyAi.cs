using System.Numerics;
using NekoRay;
using NekoRay.Tools;

namespace TowerDefence.Gameplay.AI;

public class MediumEnemyAi : SmallEnemyAi {
    public bool AlternateAttack = false;
    public override void OnStateChanged(EnemyState old, EnemyState current) {
        switch (current) {
            case EnemyState.RunAway:
            case EnemyState.RunToObjective:
                Animator.AnimationName = "run";
                return;
            case EnemyState.AttackPrepare:
                Animator.AnimationName = "attack_prepare";
                return;
            case EnemyState.Attack:
                Animator.AnimationName = AlternateAttack ? "attack_left" : "attack_right";

                AlternateAttack = !AlternateAttack;
                break;
            default:
                Animator.AnimationName = "idle";
                break;
        }
    }

    [ShowInInspector]
    private bool _attacked = false;

    public float StaggerTime = 0.25f;
    private float _staggerTime = 0f;
    public float PreAttackTime = 0.35f;
    private float _preAttackTime = 0f;
    public override void Attack() {
        if (!_attacked) {
            _attacked = true;
            Enemy.Attack(MoveDirection);
            return;
        }

        _staggerTime += Time.DeltaF;
        if (!(StaggerTime <= _staggerTime)) return;
        _staggerTime = 0f;
        CurrentState = EnemyState.Idle;
    }

    public override void AttackPrepare() {
        _preAttackTime += Time.DeltaF;
        if (!(PreAttackTime <= _preAttackTime)) return;
        _preAttackTime = 0f;
        CurrentState = EnemyState.Attack;
    }

    public override void Run() {
        if (HasBreakableRoadBlock() && _currentTime > AttackCoolDown) {
            CurrentState = EnemyState.AttackPrepare;
            return;
        }
        if (MoveDirection is { X: 0f, Y: 0f }) {
            CurrentState = EnemyState.Idle;
            return;
        }
        Enemy.Move(MoveDirection);
    }

    public override void Idle() {
        if (MoveDirection.X == 0f && MoveDirection.Y == 0f) return;
        if (!HasBreakableRoadBlock()) {
            CurrentState = EnemyState.RunToObjective;
            return;
        }
        if (_currentTime > AttackCoolDown) {
            _currentTime = 0f;
            CurrentState = EnemyState.AttackPrepare;
        }
    }
}