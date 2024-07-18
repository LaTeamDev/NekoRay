using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using FlappyPegasus.Debug;
using FlappyPegasus.GameStuff;
using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Timer = NekoRay.Timer;

namespace FlappyPegasus; 

public class GameScene : BaseScene {

    public OverlayScene OverlayScene;
    public OverlayScene GameOverScene;
    
    public override void Initialize() {
        this.CreateWorld();

        #region Camera
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.BackgroundColor = new Color(203, 219, 252, 255);
        camera.IsMain = true;
        camera.Zoom = Raylib.GetRenderHeight() / 288f;
        camera.GameObject.AddComponent<MoveCamera>();
        #endregion

        #region Canvas
        var canvas = new GameObject("Canvas").AddComponent<Canvas>();
        var scoreText = canvas.GameObject.AddChild("Score Text").AddComponent<ShadowedText>();
        scoreText.Font = Data.GetFont("Data/texture/scorefont.png", "0123456789xm");
        scoreText.ShadowFont = Data.GetFont("Data/texture/scorefont_s.png", "0123456789xm");
        scoreText.Transform.LocalPosition = new Vector3(120f, 120f, 0f);
        scoreText.Transform.LocalScale = Vector3.One * 2;
        var coinText = canvas.GameObject.AddChild("Coin text").AddComponent<ShadowedText>();
        coinText.Font = Data.GetFont("Data/texture/scorefont.png", "0123456789xm");
        coinText.ShadowFont = Data.GetFont("Data/texture/scorefont_s.png", "0123456789xm");
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
        collider.Radius = 12f/Physics.MeterScale;
        
        var rigidbody = playerAnimation.GameObject.AddComponent<Rigidbody2D>();
        rigidbody.BodyType = BodyType.Dynamic;
        var player = playerAnimation.GameObject.AddComponent<PlayerMove>();
        player.Sprite = playerAnimation._spriteRenderer;
        player.rb2D = rigidbody;
        player.Animation = playerAnimation;
        player.Animation.FreezeFrame = 0;
        player.Transform.Position = player.Transform.Position with {X = -120f};
        
        playerAnimation.AnimationFrames = AsepriteLoader.Load("Data/texture/bladhead.json");
        #endregion
        
        #region Setup Bounds
        var levelBounds = new GameObject("Level Bounds");
        var tb = levelBounds.AddChild("Top"); 
        var topBound = new {
            rb = tb.AddComponent<Rigidbody2D>(),
            collider = tb.AddComponent<PolygonCollider>() //TODO: Make it and edge collider
        };
        topBound.rb.BodyType = BodyType.Static;
        topBound.collider.SetAsBox(512f/Physics.MeterScale, 32f/Physics.MeterScale);
        topBound.rb.Transform.Position = new Vector3(0f, -176f, 0f);
        
        var bb = levelBounds.AddChild("Bottom"); 
        var bottomBound = new {
            rb = bb.AddComponent<Rigidbody2D>(),
            collider = bb.AddComponent<PolygonCollider>() //TODO: Make it and edge collider
        };
        bottomBound.rb.BodyType = BodyType.Static;
        bottomBound.collider.SetAsBox(512f/Physics.MeterScale, 32f/Physics.MeterScale);
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
        
        base.Initialize();
    }

    void OnKeyPressed(KeyboardKey key) {
        if (key != KeyboardKey.KEY_ENTER) return;
        if (GameOverScene.Active) return;
        OverlayScene.Toggle();
    }

    public override void Draw() {
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) base.Draw();
    }

    public override void Update() {
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) {
            base.Update();
            this.GetWorld().Step(Timer.DeltaF, 8, 3);
        }
    }
}