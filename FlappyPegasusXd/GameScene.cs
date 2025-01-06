using System.Numerics;
using Box2D;
using FlappyPegasus.Dbg;
using FlappyPegasus.GameStuff;
using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace FlappyPegasus; 

public class GameScene : BaseScene {

    public OverlayScene OverlayScene;
    public OverlayScene GameOverScene;

    private Action UpdateBg;

    public override void Initialize() {
        this.CreateWorld(Physics.DefaultGravity * World.LengthUnitsPerMeter);

        #region Camera
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.BackgroundColor = new Color(203, 219, 252, 255);
        camera.IsMain = true;
        camera.Zoom = Raylib.GetRenderHeight() / 288f;
        camera.GameObject.AddComponent<MoveCamera>();
        #endregion

        #region Background
        var background = new GameObject("Background");
        background.Transform.Position = new Vector3(-256f, -144f, 0f);
        
        var cloudsB = background.AddChild("CloudsB").AddComponent<ShaderDrawBg>();
        cloudsB.Texture = Data.GetTexture("texture/clouds2.png");
        cloudsB.Transform.LocalPosition = new Vector3(0f, 32f, 0f);

        var cloudsA = background.AddChild("CloudsA").AddComponent<ShaderDrawBg>();
        cloudsA.Texture = Data.GetTexture("texture/clouds1.png");
        
        var groundA = background.AddChild("GroundA").AddComponent<ShaderDrawBg>();
        groundA.Texture = Data.GetTexture("texture/Mountains_A.png");
        groundA.Transform.LocalPosition = new Vector3(0f, 288f - groundA.Texture.Height - 16f, 0f);
        
        var groundB = background.AddChild("GroundB").AddComponent<ShaderDrawBg>();
        groundB.Texture = Data.GetTexture("texture/Mountains_B.png");
        groundB.Transform.LocalPosition = new Vector3(0f, 288f - groundB.Texture.Height, 0f);
        
        var groundC = background.AddChild("GroundC").AddComponent<ShaderDrawBg>();
        groundC.Texture = Data.GetTexture("texture/Tree_A.png");
        groundC.Transform.LocalPosition = new Vector3(0f, 288f - groundC.Texture.Height, 0f);
        #endregion

        #region Canvas
        var canvas = new GameObject("Canvas").AddComponent<Canvas>();
        var scoreText = canvas.GameObject.AddChild("Score Text").AddComponent<ShadowedText>();
        scoreText.Font = Data.GetFont("texture/scorefont.png", "0123456789xm.");
        scoreText.ShadowFont = Data.GetFont("texture/scorefont_s.png", "0123456789xm.");
        scoreText.Transform.LocalPosition = new Vector3(120f, 120f, 0f);
        scoreText.Transform.LocalScale = Vector3.One * 2;
        var coinText = canvas.GameObject.AddChild("Coin text").AddComponent<ShadowedText>();
        coinText.Font = Data.GetFont("texture/scorefont.png", "0123456789xm.");
        coinText.ShadowFont = Data.GetFont("texture/scorefont_s.png", "0123456789xm.");
        coinText.Transform.LocalPosition = new Vector3(120f, 160f, 0f);
        coinText.Transform.LocalScale = Vector3.One * 2;
        #endregion

        #region Player
        var playerAnimation = new GameObject("Player")
            .AddComponent<SimpleAnimation>();
        playerAnimation._spriteRenderer = playerAnimation.GameObject.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        var spriteTransform = playerAnimation._spriteRenderer.GameObject.Transform;
        //playerAnimation._spriteRenderer.GameObject.Transform.Parent = null;
        playerAnimation._spriteRenderer.FlipX = true;
        var collider = playerAnimation.GameObject.AddComponent<CircleCollider>();
        collider.Radius = 12f;
        
        var rigidbody = playerAnimation.GameObject.AddComponent<Rigidbody2D>();
        rigidbody.Type = BodyType.Dynamic;
        var player = playerAnimation.GameObject.AddComponent<PlayerMove>();
        player.Sprite = playerAnimation._spriteRenderer;
        player.rb2D = rigidbody;
        player.Animation = playerAnimation;
        player.Animation.FreezeFrame = 0;
        player.Transform.Position = player.Transform.Position with {X = -120f};
        
        playerAnimation.AnimationFrames = AsepriteLoader.Load("texture/bladhead.json");

        player.GameObject.AddComponent<AudioListener>();
        #endregion
        
        #region Setup Bounds
        var levelBounds = new GameObject("Level Bounds");
        var tb = levelBounds.AddChild("Top"); 
        var topBound = new {
            rb = tb.AddComponent<Rigidbody2D>(),
            collider = tb.AddComponent<BoxCollider>() //TODO: Make it and segment collider
        };
        topBound.rb.Type = BodyType.Static;
        topBound.collider.Width = 512f;
        topBound.collider.Height = 32f;
        topBound.rb.Transform.Position = new Vector3(0f, -176f, 0f);
        
        var bb = levelBounds.AddChild("Bottom"); 
        var bottomBound = new {
            rb = bb.AddComponent<Rigidbody2D>(),
            collider = bb.AddComponent<BoxCollider>() //TODO: Make it and edge collider
        };
        bottomBound.rb.Type = BodyType.Static;
        bottomBound.collider.Width = 512f;
        bottomBound.collider.Height = 32f;
        bottomBound.rb.Transform.Position = new Vector3(0f, 176f, 0f);
        bb.Tags.Add("Danger");

        #endregion

        #region Score Controller
        player.Score = new GameObject("Score Controller").AddComponent<ScoreController>();
        player.Score.ScoreText = scoreText;
        player.Score.CoinText = coinText;
        
        #endregion

        #region Spawners
        var spawner = new GameObject("Spawners");
        var enemySpawner = spawner.AddChild("Enemy").AddComponent<EnemySpawner>();
        enemySpawner.SpawnLocation = new GameObject("Enemy Spawn Location").Transform;
        enemySpawner.SpawnLocation.Position = Vector3.UnitX * 290f;
        enemySpawner.Score = player.Score;
        #endregion
        
        #region Overlay Scenes
        OverlayScene = new PauseScene();
        GameOverScene = new GameOverScene();
        SceneManager.LoadScene(OverlayScene, SceneLoadMode.Inclusive);
        SceneManager.LoadScene(GameOverScene, SceneLoadMode.Inclusive);
        SceneManager.SetSceneActive(this);
        player.GameOverScene = GameOverScene;
        #endregion

        var groundD = background.AddChild("GroundC").AddComponent<ShaderDrawBg>();
        groundD.Texture = Data.GetTexture("texture/Ground_A.png");
        groundD.Transform.LocalPosition = new Vector3(0f, 288f - groundD.Texture.Height, 0f);

        UpdateBg = () => {
            cloudsA.Speed = 2f * player.Score._speed;
            cloudsB.Speed = 1f * player.Score._speed;
            groundA.Speed = 0.25f * player.Score._speed;
            groundB.Speed = 8f * player.Score._speed;
            groundC.Speed = 32f * player.Score._speed;
            groundD.Speed = 64f * player.Score._speed;
        };

        //new GameObject("DebugWorldDraw").AddComponent<DrawWorld>();

        base.Initialize();
    }

    void HandlePause() {
        if (!Input.IsPressed("pause")) return;
        if (GameOverScene.Active) return;
        OverlayScene.Toggle();
    }

    public override void Draw() {
       if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) base.Draw();
    }

    public override void Update() {
        HandlePause();
        UpdateBg();
        
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) {
            base.Update();
            this.GetWorld().Step(Time.DeltaF, 4);
        }
    }
}