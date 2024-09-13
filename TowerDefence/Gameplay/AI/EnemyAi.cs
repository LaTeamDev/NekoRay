using System.Text;
using NekoLib.Core;

namespace TowerDefence.Gameplay.AI;

public abstract class EnemyAi : Behaviour {
    private EnemyState _currentState = EnemyState.Idle;
    public Animator2D Animator;
    public Enemy Enemy;

    public EnemyState CurrentState {
        get => _currentState;
        set {
            var old = _currentState;
            _currentState = value;
            if (old != _currentState) {
                PreviousState = old;
                OnStateChanged(old, _currentState);
            }
        }
    }
    public EnemyState PreviousState { get; private set; }

    public virtual void OnStateChanged(EnemyState old, EnemyState current) { }

    public virtual void Idle() {}
    public virtual void Death() {}
    public virtual void Attack() {}
    public virtual void Shoot() {}
    public virtual void Run() {}
    public virtual void RunAway() {}
    
    
    public virtual void Act() {
        switch (CurrentState) {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Death:
                Death();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Shoot:
                Shoot();
                break;
            case EnemyState.RunToObjective:
                Run();
                break;
            case EnemyState.RunAway:
                RunAway();
                break;
            default:
                throw new ArgumentException("Unknown State", nameof(CurrentState));
        }
    }

    protected virtual void Update() {
        Act();
    }
}