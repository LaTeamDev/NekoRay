using NekoLib.Core;
using NekoRay;

namespace FlappyPegasus.GameStuff; 

public class ScoreController : Behaviour {

    public Text CoinText;
    public Text ScoreText;
    
    public int CurrenctCoins;
    public int CurrenctScore;

    void LateUpdate() {
        CoinText.TextString = "x"+CurrenctCoins;
        ScoreText.TextString = CurrenctScore+"m";
    }
    
    public void SaveCoin() {
        SaveData.CoinCount = CurrenctCoins;
        SaveData.Save();
    }

    public void SaveScore() {
        if (CurrenctScore <= SaveData.BestScore) return;
        SaveData.BestScore = CurrenctScore;
        SaveData.Save();
    }
}