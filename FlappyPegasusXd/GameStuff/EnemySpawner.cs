using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using Timer = NekoRay.Timer;

namespace FlappyPegasus.GameStuff; 

public class EnemySpawner : Behaviour {
    public ScoreController Score;
    public Transform SpawnLocation;
    public float SpawnRate = 1f;
    private float _time;
    private Random random = new((int)Timer.Time);
    private float SpawnRadius = 120f;
    private List<AnimationFrame[]> EnemyAnimations = new();

    void Awake() {
        EnemyAnimations.Add(AsepriteLoader.Load("Data/texture/T228.json"));
        EnemyAnimations.Add(AsepriteLoader.Load("Data/texture/bladhead.json"));
    }
    
    void Spawn() {
        var enemy = new GameObject("Enemy").AddComponent<EnemyController>();
        enemy.GameObject.Tags.Add("Danger");
        enemy.Score = Score;
        var collider = enemy.GameObject.AddComponent<CircleCollider>();
        collider.Radius = 10f / Physics.MeterScale;
        enemy.Rigidbody = enemy.GameObject.AddComponent<Rigidbody2D>();
        enemy.Rigidbody.BodyType = BodyType.Kinematic;
        enemy.Transform.Position = SpawnLocation.Position;
        enemy.Transform.Position += SpawnRadius * (random.NextSingle() - 0.5f) * Vector3.UnitY;
        var anim = enemy.GameObject.AddComponent<SimpleAnimation>();
        anim.AnimationFrames = EnemyAnimations[random.Next(EnemyAnimations.Count - 1)];
        anim._spriteRenderer = enemy.GameObject.AddComponent<SpriteRenderer2D>();
        enemy.GameObject.Initialize(); //TODO: THIS SHOULD NOT BE DONE MANUALLY
    }

    void Update() {
        _time += Timer.DeltaF;
        while (_time >= SpawnRate) {
            _time -= SpawnRate;
            Spawn();
        }
    }
    
}