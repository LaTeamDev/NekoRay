using System.Drawing;
using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoRay;
using NekoRay.Physics2D;
using NeuroSama.Gameplay.Dialogue;
using SoLoud;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama.Gameplay.MiniGame;

public class MiniGameScene : BaseScene {
    public World World;
    private Voice _voice;
    public override void Initialize() {
        World = this.CreateWorld(Vector2.Zero);
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.Zoom = 2f;

        var background = new GameObject("background").AddComponent<SpriteRenderer2D>();
        background.Sprite = Data.GetSprite("textures/minigame/ballpit.png");
        background.Transform.Position = new Vector3(-1f, -24f, 0f);

        var tutel = new GameObject("Tutel").AddComponent<SpriteRenderer2D>();
        tutel.Sprite = Data.GetSprite("textures/minigame/tutel.png");
        var player = tutel.GameObject.AddComponent<PlayerMiniGameController>();
        var playerCollider = player.GameObject.AddComponent<CircleCollider>();
        playerCollider.Radius = 9f;
        player.Collider = playerCollider;
        player.Rigidbody2D = player.GameObject.AddComponent<Rigidbody2D>();
        player.Rigidbody2D.Type = BodyType.Dynamic;

        CreateSprite(new Vector2(-76, -126), "ballpit_text", false);
        CreateSprite(new Vector2(-76, -107), "question_1");
        CreateSprite(new Vector2(-95, -62), "answer_1");
        CreateSprite(new Vector2(99, -47), "answer_2");
        CreateSprite(new Vector2(102, 19), "answer_3");
        

        var overlay = gameObject.AddChild("Overlay").AddComponent<SpriteRenderer2D>();
        overlay.Sprite = Data.GetSprite("textures/minigame/overlay.png");
        overlay.Width = 1280/2;
        overlay.Height = 720/2;
        overlay.BlendMode = BlendMode.BLEND_ADDITIVE;
        
        CreateCollider(new RectangleF(-90f, 50f, 64f, 64f));
        CreateCollider(new RectangleF(-93f, -65f, 137f/2, 32f/2f));
        CreateCollider(new RectangleF(90f, 16f, 137f/2, 32f/2f));
        CreateCollider(new RectangleF(0f, -145f, 320f, 64f));
        CreateCollider(new RectangleF(90f, 80f, 137f/2f, 32f/2f));
        CreateCollider(new RectangleF(90f, -57f, 137f/2f, 48f/2f));
        CreateCollider(new RectangleF(0f, 158f, 320f, 64f));

        var bb = CreateTrigger(new RectangleF(160f, -20f, 16f, 16f)).GameObject.AddComponent<AnswerTrigger>();
        bb.IsCorrect = true;
        var wg = CreateTrigger(new RectangleF(160f, 50f, 16f, 16f)).GameObject.AddComponent<AnswerTrigger>();
        var sa = CreateTrigger(new RectangleF(-160f, -30f, 16f, 16f)).GameObject.AddComponent<AnswerTrigger>();
        
        //var dg = gameObject.AddChild("Dialogue").AddComponent<DialogueOrchestrator>();
        
        using var stream = Files.GetFile("sounds/music/neuro2.mp3").GetStream();
        var musicStream = WavStream.LoadFromStream(stream);
        _voice = Audio.SoLoud.Play(musicStream);
        _voice.Loop = true;
        
        base.Initialize();
    }
    private bool _dialogueShown = false;
    public override void Update() {
        base.Update();
        if (!_dialogueShown) {
            _dialogueShown = true;
            DialogueController.AddStub();
            DialogueController.Add("Neuro", "What was the first song i sung?");
        }
    }

    private void CreateCollider(RectangleF rect) {
        var collider = new GameObject("bound").AddComponent<BoxCollider>();
        collider.Width = rect.Width;
        collider.Height = rect.Height;
        collider.Transform.Position = new Vector3(rect.X, rect.Y, 0f);
        var rb = collider.GameObject.AddComponent<Rigidbody2D>();
    }

    private void CreateSprite(Vector2 pos, string path, bool small = true) {
        var background = new GameObject(path).AddComponent<SpriteRenderer2D>();
        background.Sprite = Data.GetSprite("textures/minigame/"+path+".png");
        background.Transform.Position = new Vector3(pos, 0f);
        if (small) background.Transform.LocalScale = Vector3.One / 2;
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

    public override void FixedUpdate() {
        base.FixedUpdate();
        this.GetWorld().Step(Time.FixedDeltaF, 4);
    }
    
    public override void Dispose() {
        base.Dispose();
        _voice.Stop();
    }
}