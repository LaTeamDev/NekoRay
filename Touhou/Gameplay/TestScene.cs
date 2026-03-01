using NekoLib.Core;
using NekoLib.Extra;
using NekoRay;

namespace Touhou.Gameplay;

public class TestScene : Scene {
    public override void Initialize() {
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.IsMain = true;
        var info = PlayerInfo.Load("data/characters/reimuA.plr");
        //new GameObject("reimu").AddComponent<SpriteRenderer2D>().Sprite = Sprite.Load("sprites/characters/reimu/idle.nrs");
        SpawnPlayer(info);
    }

    public static void SpawnPlayer(PlayerInfo playerInfo) {
        var player = new GameObject(playerInfo.DisplayName)
            .AddComponent<PlayerController>();
        var sprite = player.GameObject.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        var animator = player.GameObject.AddComponent<SpriteAnimator>();
        animator.SpriteRenderer = sprite;
        animator.AddAnimations(playerInfo.LoadedAnimations);
        animator.AnimationName = "IDLE";
        player.BaseSpeed = playerInfo.BaseSpeed;
        player.FocusedSpeed = playerInfo.FocusedSpeed;
    }
}