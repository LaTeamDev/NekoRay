using NekoLib.Core;
using NekoRay.Physics2D;
using Timer = NekoRay.Timer;

namespace FlappyPegasus.GameStuff; 

public class EnemyController : Behaviour {
    public ScoreController Score;
    public Rigidbody2D Rigidbody;
    public float SpeedFactor = 64f;
    public float MinSpeed = 1f;

    void Update() {
        var speed = Math.Max(MinSpeed, Score._speed * SpeedFactor);
        Rigidbody.Position = Rigidbody.Position with {X = Rigidbody.Position.X - speed * Timer.DeltaF};
        if (Transform.Position.X < -400f)
            Destroy(GameObject);
    }
}