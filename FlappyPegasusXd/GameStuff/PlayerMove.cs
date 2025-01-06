using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoRay;
using NekoRay.Physics2D;
using SoLoud;
using ZeroElectric.Vinculum;

namespace FlappyPegasus.GameStuff; 

public class PlayerMove : Behaviour {

    // Player parameters
    public Rigidbody2D rb2D;
    private static int deaths;
    public Wav JumpSound;
    private float jumpPower = 3f;
    private float speedMove = 8f;
    private float currentDist;
    private float rotateSpd = .15f;
    public SpriteRenderer2D Sprite;
    public SimpleAnimation Animation;

    public ScoreController Score;
    public OverlayScene GameOverScene;

    void Awake() {
        using var stream = Files.GetFile("sounds/JumpSound.wav").GetStream();
        JumpSound = Wav.LoadFromStream(stream);
    }

    private void Update()
    {
        TouchJump();
    }

    private void ProcessTouch() {
        var jump = Input.IsPressed("attack");
        if (!jump)
            return;
        rb2D.LinearVelocity = -Vector2.UnitY * jumpPower* World.LengthUnitsPerMeter;
        Animation.RunAnim(5);
        Audio.SoLoud.Play(JumpSound);
    }

    private Vector4 smoothDamp;
    
    private void TouchJump() {
        ProcessTouch();
        if (Input.IsDown("attack2"))
            rb2D.Position = BaseCamera.Main?.ScreenToWorld(Input.MousePosition).ToVector2()??rb2D.Position;

        Sprite.Transform.Rotation = NekoMath.Damp(
            Sprite.Transform.Rotation, 
            Quaternion.CreateFromAxisAngle(Vector3.UnitZ, rb2D.LinearVelocity.Y * rotateSpd/World.LengthUnitsPerMeter),
            ref smoothDamp,
            0.2f);
    }



    public void OnDeath() {
        Score.UpdateSaveData();
        SaveData.DeathCount++;
        SaveData.Save();
        //CounterAchievement.CheckDead(score, currentCoins, deaths);
        GameOverScene.Open();
    }

    private void OnBeginContact2D(ContactEvents.BeginTouchEvent collision) {
        var contactRb = (Rigidbody2D)collision.ShapeA.Body.UserData;
        if (contactRb.GameObject == GameObject)
            contactRb = (Rigidbody2D)collision.ShapeB.Body.UserData;
        if (!contactRb.GameObject.Tags.Contains("Danger")) return;
        OnDeath();
    }

    private void OnSensorEnter2D(SensorEvents.BeginTouchEvent collision)
    {
        var contactRb = (Rigidbody2D)collision.SensorShape.Body.UserData;
        if (!contactRb.GameObject.Tags.Contains("Coin")) return;
        Score.CurrenctCoins++;
        Destroy(contactRb.GameObject);
    }
}