using System.Diagnostics;
using Box2D;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;

namespace TowerDefence.Gameplay;
public class DebugScene : BaseScene {
    [ShowInInspector]
    private World _world;
    public override void Initialize() {
        _world = this.CreateWorld();
        base.Initialize();
    }

    public override void FixedUpdate() {
        _world.Step(Time.FixedDeltaF, 4);
    }
}