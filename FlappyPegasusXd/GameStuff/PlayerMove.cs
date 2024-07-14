using System.Numerics;
using NekoLib.Core;

namespace FlappyPegasus.GameStuff; 
/*
public class PlayerMove : Behaviour {

    // Player parameters
    //public Rigidbody2D rb2D;
    private static int deaths;
    public string CounterDeath;
    public AudioSource JumpSound;
    public AudioClip SoundJump;
    private float jumpPower = 7.36f;
    private float speedMove = 8f;
    private float currentDist;
    private float rotateSpd = 0.45f; 
    public bool isTouchEnable = true;

    // Coins
    public string KeyCoin;
    public Text ScoreCoin;
    public int CoinScore;
    private int currentCoins = 0;

    // Score
    public Text CurrentScore;
    private int score;

    // Game panels
    public GameObject StartPanel;
    public GameObject LoseMenuPanel;

    private void Awake()
    {
        score = PlayerPrefs.GetInt(ProgressPlayer.Score, 0);
        CoinScore = PlayerPrefs.GetInt(ProgressPlayer.Coins, 0);

        ScoreCoin.text = currentCoins.ToString();
    }

    private void Update()
    {
        TouchJump();
        MovePlayerX();
    }

    private void MovePlayerX()
    {
        currentDist = rb2D.position.x / 100;
        rb2D.velocity = new Vector2(speedMove + currentDist, rb2D.velocity.y);

        score = (int)rb2D.position.x;
        CurrentScore.text = score.ToString();
    }

    private void TouchJump()
    {
        if (Input.touchCount > 0 && isTouchEnable)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                rb2D.simulated = true;
                StartPanel.SetActive(!rb2D.simulated);
                rb2D.velocity = Vector2.up * jumpPower;
                JumpSound.clip = SoundJump;
                JumpSound.Play();
            }
        }

        rb2D.transform.rotation = Quaternion.Euler(0, 0, rb2D.velocity.y * rotateSpd);
    }

    public void CoinSaves()
    {
        ScoreCoin.text = currentCoins.ToString();
        PlayerPrefs.SetInt(ProgressPlayer.Coins, CoinScore + currentCoins);
    }

    public void SaveScore()
    {
        if (score > PlayerPrefs.GetInt(ProgressPlayer.Score, 0))
        {
            PlayerPrefs.SetInt(ProgressPlayer.Score, score);
            PlayerPrefs.Save();
            score = PlayerPrefs.GetInt(ProgressPlayer.Score, 0);
        }
    }

    public void CounterDead()
    {
        deaths++;
        if (deaths > PlayerPrefs.GetInt(CounterDeath, 0))
        {
            PlayerPrefs.SetInt(CounterDeath, deaths);
            PlayerPrefs.Save();
            deaths = PlayerPrefs.GetInt(CounterDeath, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameProperties.Danger))
        {
            isTouchEnable = false;
            rb2D.simulated = isTouchEnable;
            LoseMenuPanel.SetActive(!rb2D.simulated);
            CounterAchievement.CheckDead(score, currentCoins, deaths);
            CounterDead();
            AdsStartPanel(deaths, 3, 0);
            SaveScore();
        }
    }

    private void AdsStartPanel(int value, int max, int zero)
    {
        if (value % max == zero)
        {
            InterstitialAD.ShowInterstitial(() =>
            {
                LoseMenuPanel.SetActive(true);
            });

            LoseMenuPanel.SetActive(InterstitialAD.addFailed);
        }
    }

    private void OnTriggerEnter2D(Collider2D coin2D)
    {
        if (coin2D.CompareTag(KeyCoin))
        {
            currentCoins++;
            CoinSaves();
            Destroy(coin2D.gameObject);
        }
    }
}*/