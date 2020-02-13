using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Text scoreText;
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
