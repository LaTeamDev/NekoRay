using System.Numerics;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;

namespace FlappyPegasus.GameStuff; 

public class PlayerMove : Behaviour {

    // Player parameters
    public Rigidbody2D rb2D;
    private static int deaths;
    public AudioPlayer JumpSound;
    private float jumpPower = 3f;
    private float speedMove = 8f;
    private float currentDist;
    private float rotateSpd = 0.45f;

    // Coins
    public Text ScoreCoin;
    public int CoinScore;
    private int currentCoins = 0;

    // Score
    public Text CurrentScore;
    private int score;

    private void Awake()
    {
        score = SaveData.BestScore??0;
        CoinScore = SaveData.CoinCount??0;
    }

    void UpdateHud() {
        //ScoreCoin.TextString = currentCoins.ToString();
    }

    private void Update()
    {
        TouchJump();
        //MovePlayerX();
    }

    void LateUpdate() {
        UpdateHud();
    }

    void MovePlayerX()
    {
        currentDist = Transform.Position.X / 100;
        rb2D.LinearVelocity = rb2D.LinearVelocity with {X = speedMove + currentDist};

        score = (int)rb2D.Position.X;
        CurrentScore.TextString = score.ToString();
    }

    private void ProcessTouch() {
        var jump = Raylib.IsKeyPressed(KeyboardKey.KEY_Z);
        if (!jump)
            return;
        //rb2D.simulated = true;
        //StartPanel.SetActive(!rb2D.simulated);
        rb2D.LinearVelocity = -Vector2.UnitY * jumpPower;
        //JumpSound.Play();
    }
    
    private void TouchJump() {
        ProcessTouch();
        
        //rb2D.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, rb2D.LinearVelocity.Y * rotateSpd);
    }

    public void CoinSaves()
    {
        ScoreCoin.TextString = currentCoins.ToString();
        SaveData.CoinCount = CoinScore + currentCoins;
    }

    public void SaveScore() {
        if (score <= SaveData.BestScore) return;
        SaveData.BestScore = score;
        SaveData.Save();
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
        SaveScore();
    }

    private void OnBeginSensor2D(Contact collision)
    {
        var contactRb = (Rigidbody2D)collision.GetFixtureB().GetBody().UserData;
        if (!contactRb.GameObject.Tags.Contains("Coin")) return;
        currentCoins++;
        CoinSaves();
        Destroy(contactRb.GameObject);
    }
    void DrawGui() {
        Raylib.DrawText($"player: {rb2D.LinearVelocity.ToString()}", 0, 40, 20, Raylib.RED);
    }
}