using System.Numerics;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using NekoLib;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using ZeroElectric.Vinculum.Extensions;

namespace FlappyPegasus.GameStuff; 

public class PlayerMove : Behaviour {

    // Player parameters
    public Rigidbody2D rb2D;
    private static int deaths;
    public AudioPlayer JumpSound;
    private float jumpPower = 3f;
    private float speedMove = 8f;
    private float currentDist;
    private float rotateSpd = .15f;
    public SpriteRenderer2D Sprite;
    public PlayerAnimation Animation;

    public ScoreController Score;

    private void Update()
    {
        TouchJump();
    }

    private void ProcessTouch() {
        var jump = Raylib.IsKeyPressed(KeyboardKey.KEY_Z);
        if (!jump)
            return;
        rb2D.LinearVelocity = -Vector2.UnitY * jumpPower;
        Animation.RunAnim(5);
        //JumpSound.Play();
    }

    private Vector4 smoothDamp;
    
    private void TouchJump() {
        ProcessTouch();
        
        
        Sprite.Transform.Rotation = NekoMath.Damp(
            Sprite.Transform.Rotation, 
            Quaternion.CreateFromAxisAngle(Vector3.UnitZ, rb2D.LinearVelocity.Y* rotateSpd),
            ref smoothDamp,
            0.2f);
    }



    public void CounterDead()
    {
        deaths++;
        if (deaths <= SaveData.DeathCount) return;
        SaveData.DeathCount = deaths;
        SaveData.Save();
    }

    private void OnBeginContact2D(Contact collision) {
        var contactRb = (Rigidbody2D)collision.GetFixtureB().GetBody().UserData;
        if (!contactRb.GameObject.Tags.Contains("Danger")) return;
        //isTouchEnable = false;
        //LoseMenuPanel.SetActive(!rb2D.simulated);
        //CounterAchievement.CheckDead(score, currentCoins, deaths);
        CounterDead();
        Score.SaveScore();
        Score.SaveCoin();
    }

    private void OnBeginSensor2D(Contact collision)
    {
        var contactRb = (Rigidbody2D)collision.GetFixtureB().GetBody().UserData;
        if (!contactRb.GameObject.Tags.Contains("Coin")) return;
        Score.CurrenctCoins++;
        Destroy(contactRb.GameObject);
    }
    void DrawGui() {
        Raylib.DrawText($"player: {Sprite.Transform.LocalRotation.YawPitchRollAsVector3()}", 0, 40, 20, Raylib.RED);
    }
}