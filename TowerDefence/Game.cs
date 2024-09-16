using System.Numerics;
using Box2D;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using TowerDefence.Gameplay;

namespace TowerDefence;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);

        World.LengthUnitsPerMeter = 8f;
        Physics.DefaultGravity = Vector2.Zero;
        if (!DevMode)
        {
            SceneManager.LoadScene(new GameScene());
        }
    }
}
