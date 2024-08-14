using NekoLib.Core;
using NekoRay;
using Timer = NekoRay.Timer;

namespace FlappyPegasus.GameStuff; 

public class ScoreController : Behaviour {

    public Text CoinText;
    public Text ScoreText;
    
    public int CurrenctCoins;
    public int CurrenctScore => (int)_score;
    public float _score = 0f;

    public float StartSpeed = 1f;
    public float _speed;
    public float Velocity = 0.125f;

    void Awake() {
        _speed = StartSpeed;
    }
    void Update() {
        _speed = _speed + Velocity*Timer.DeltaF;
        _score = _score + _speed*Timer.DeltaF;
    }

    void LateUpdate() {
        CoinText.TextString = "x"+_speed;
        ScoreText.TextString = CurrenctScore+"m";
    }

    public void UpdateSaveData() {
        SaveData.BestScore = Math.Max(SaveData.BestScore, CurrenctScore);
        SaveData.CoinCount += CurrenctCoins;
    }
}