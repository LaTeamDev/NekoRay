using System.Drawing;
using Box2D;
using System.Numerics;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoRay;
using NekoRay.Physics2D;
using NeuroSama.Gameplay;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.MiniGame;
using NeuroSama.UI;
using SoLoud;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Font = NekoRay.Font;

namespace NeuroSama.Gameplay.Wander;

public class WanderScene : BaseScene {
    private World _world;
    private Voice _voice;
    public override void Initialize() {
        _world = this.CreateWorld(new Vector2(0f, 9.31f*64f));
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.BackgroundColor = Raylib.BLACK;
        camera.Zoom = 1f;
        // var player = playerAnimation.GameObject.AddComponent<PlayerMove>();
        var backgroundbg = new GameObject("backgroundbg").AddComponent<SpriteRenderer2D>();
        backgroundbg.Sprite = Data.GetSprite("textures/location1_bg.png");
        backgroundbg.Transform.Position = new Vector3(-640f, -24f, 0f);
        backgroundbg.ProportionallyScaleByHeight(720);
        backgroundbg.Origin = new Vector2(0, 0.5f);
        var bgbgParallax = backgroundbg.GameObject.AddComponent<Parallax>();
        bgbgParallax.Target = camera.Transform;
        bgbgParallax.Factor = 0.09f;
        bgbgParallax.Transform.Position = new Vector3(-940f, -24f, 0f);
        
        var background = new GameObject("background").AddComponent<SpriteRenderer2D>();
        background.Sprite = Data.GetSprite("textures/location1_fg.png");
        background.Transform.Position = new Vector3(-640f, -24f, 0f);
        background.ProportionallyScaleByHeight(720);
        background.Origin = new Vector2(0, 0.5f);
        
        
        var pl = new GameObject("Player").AddComponent<WanderPlayerController>();
        var playerCollider = pl.GameObject.AddComponent<BoxCollider>();
        playerCollider.Width = 64f;
        playerCollider.Height = 256f;
        pl.rb = pl.GameObject.AddComponent<Rigidbody2D>();
        pl.rb.FixedRotation = true;
        pl.rb.Type = BodyType.Dynamic;
        pl.Transform.Position = new Vector3(0, 86f, 0);
        var follower = camera.GameObject.AddComponent<CameraFollow>();
        follower.FollowTarget = pl.Transform;
        follower.LimitMax = 2906;

        CreateCollider(new RectangleF(-640, 0, 64f, 720f));
        CreateCollider(new RectangleF(1500f, 760f, 3000f, 420f));
        CreateCollider(new RectangleF(3350, 0, 64f, 720f));

        var doorTrigger = CreateTrigger(new RectangleF(1475f, 0, 270f, 720f));
        doorTrigger.GameObject.AddComponent<DoorTrigger>();
        
        var dg = gameObject.AddChild("Dialogue").AddComponent<DialogueOrchestrator>();
        
        using var stream = Files.GetFile("sounds/music/neuro3.mp3").GetStream();
        var musicStream = WavStream.LoadFromStream(stream);
        _voice = Audio.SoLoud.Play(musicStream);
        _voice.Loop = true;
        
        base.Initialize();
    }
    
    private void CreateCollider(RectangleF rect) {
        var collider = new GameObject("bound").AddComponent<BoxCollider>();
        collider.Width = rect.Width;
        collider.Height = rect.Height;
        collider.Transform.Position = new Vector3(rect.X, rect.Y, 0f);
        var rb = collider.GameObject.AddComponent<Rigidbody2D>();
    }

    private Rigidbody2D CreateTrigger(RectangleF rect) {
        var collider = new GameObject("bound").AddComponent<BoxCollider>();
        collider.Width = rect.Width;
        collider.Height = rect.Height;
        collider.Transform.Position = new Vector3(rect.X, rect.Y, 0f);
        collider.IsSensor = true;
        var rb = collider.GameObject.AddComponent<Rigidbody2D>();
        return rb;
    }

    public override void Dispose() {
        base.Dispose();
        _voice.Stop();
    }
    
    public override void OnWindowResize() {
        base.OnWindowResize();
        ((Camera2D) (BaseCamera.Main)).Zoom = 1280 / BaseCamera.Main.RenderWidth;
    }
    
    
    public override void FixedUpdate() {
        base.FixedUpdate();
        _world.Step(Time.FixedDeltaF, 4);
    }
}