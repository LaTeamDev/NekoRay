using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using Serilog;

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
    protected float _currentTime = 0f;
    public Vector2 MoveDirection = Vector2.One;
    public override void Attack() {
        Enemy.Attack(MoveDirection);
        CurrentState = PreviousState;
    }

    public bool HasBreakableRoadBlock() {
        GameObject? context = null; //FIXME: there could be buildings too
        Enemy.GameObject.GetComponent<Rigidbody2D>() //could be slow, not sure
            .World.CastRay<GameObject>(
            Transform.Position.ToVector2(), 
            Transform.Position.ToVector2()+MoveDirection*16,
            new QueryFilter<PhysicsCategory> {Mask = PhysicsCategory.Buildings | PhysicsCategory.Player, Category = PhysicsCategory.Trigger},
            static (Shape shape, Vector2 point, Vector2 normal, float fraction, ref GameObject? ctx) => {
                var rb = (Rigidbody2D)(shape.Body.UserData);
                ctx = rb.GameObject;
                return 0f;
            }, ref context);
        return context is not null;
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