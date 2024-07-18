using System.Data;
using System.Numerics;
using NekoLib.Core;
using NekoRay.Physics2D;

namespace FlappyPegasus.GameStuff; 

public class EnemyController : Behaviour {
    public ScoreController Score;
    public Rigidbody2D Rigidbody;
    public float SpeedFactor = 64f;
    public float MinSpeed = 1f / Physics.MeterScale;

    void Update() {
        var speed = Math.Max(MinSpeed, Score.CurrenctScore / Physics.MeterScale / SpeedFactor);
        Rigidbody.Position = Rigidbody.Position with {X = Rigidbody.Position.X - speed};
    }
}