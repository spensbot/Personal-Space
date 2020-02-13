using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Text scoreText;
    [SerializeField] Text playButtonText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text totalPlayTimeText;
    [SerializeField] GameObject bootScreen;

//---------------     LIFECYCLE METHODS     -----------------

    void Start()
    {
        EventManager.EnterState += EnterState;
        EventManager.ScoreChanged += ScoreChanged;
        EventManager.HighScoreChanged += HighScoreChanged;
        EventManager.TotalPlayTimeChanged += TotalPlayTimeChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.EnterState -= EnterState;
        EventManager.ScoreChanged -= ScoreChanged;
        EventManager.HighScoreChanged -= HighScoreChanged;
        EventManager.TotalPlayTimeChanged -= TotalPlayTimeChanged;
    }

//-----------------     EVENT RECEIVERS     -----------------

    void EnterState(GameState state)
    {
        switch (state){
            case GameState.PLAY:
                bootScreen.SetActive(false);
                break;
            case GameState.BOOT:
                bootScreen.SetActive(true);
                break;
        }
    }

    void ScoreChanged(int newScore)
    {
        scoreText.text = $"{newScore}";
        scoreText.fontSize = GetScoreFontSize(newScore);
        playButtonText.text = GetPlayButtonText(newScore);

    }

    void HighScoreChanged(int newHighScore)
    {
        highScoreText.text = $"High Score: {newHighScore}";
    }

    void TotalPlayTimeChanged(float newTotalPlayTime)
    {
        totalPlayTimeText.text = GetTotalPlayTimeText(newTotalPlayTime);
    }

//-------------------     HELPER FUNCTIONS     -----------------

    string GetPlayButtonText(int score)
    {
        if (score > 100)
        {
            return "It's Going Down For Real";
        }
        else if (score > 20)
        {
            return "I Can't Let Them Take My Personal Space";
        }
        else if (score > 10)
        {
            return "I CAN DO BETTER!";
        }
        else if (score > 0)
        {
            return "Ok, I'm Getting It Now";
        }
        else
        {
            return "Get Started";
        }
    }

    int GetScoreFontSize(int score)
    {
        return 50 + (score);
    }

    string GetTotalPlayTimeText(float totalPlayTime)
    {
        if (totalPlayTime < 60 * 60)
        {
            float minutes = totalPlayTime / 60;
            return string.Format("{0:f1} minutes", minutes);
        } else
        {
            float hours = totalPlayTime / 60 / 60;
            return string.Format("{0:f1 hours", hours);
        }
    }
}
