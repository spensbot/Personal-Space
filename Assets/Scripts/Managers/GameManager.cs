using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates { BOOT, PLAY }

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Text scoreText;
    [SerializeField] Text playButtonText;
    [SerializeField] Text highScoreText;
    [SerializeField] GameObject bootScreen;
    [SerializeField] GameObject hud;
    public Vector2 screenBounds { get; private set; }
    int highScore;
    int score;
    GameStates currentState;


//----------------     LIFECYCLE METHODS     ---------------

    void Start()
    {
        DontDestroyOnLoad(this);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        EventManager.EnemyDied += OnEnemyDied;
        EventManager.PlayerDied += OnPlayerDied;
        TransitionToState(GameStates.BOOT);
    }

    protected override void OnDestroy()
    {
        EventManager.EnemyDied -= OnEnemyDied;
        EventManager.PlayerDied -= OnPlayerDied;
    }


//-----------------     EVENTS     -------------------------

    void OnEnemyDied()
    {
        score += 1;
        scoreText.fontSize = GetScoreFontSize(score);
        scoreText.text = $"{score}";
    }

    void OnPlayerDied()
    {
        if (score > highScore)
        {
            highScore = score;
        }
        score = 0;
        TransitionToState(GameStates.BOOT);
    }

    public void StartGame()
    {
        TransitionToState(GameStates.PLAY);
    }


    //--------------     STATE MANAGEMENT     ------------------

    public void TransitionToState(GameStates newState)
    {
        ExitState(currentState);
        EnterState(newState);
        currentState = newState;
    }

    void EnterState(GameStates state)
    {
        switch (state)
        {
            case GameStates.BOOT:
                Time.timeScale = 0.0f;
                bootScreen.gameObject.SetActive(true);
                hud.gameObject.SetActive(false);
                highScoreText.text = $"Your High Score: {highScore}";
                playButtonText.text = GetPlayButtonText(highScore);
                break;
            case GameStates.PLAY:
                Time.timeScale = 1.0f;
                bootScreen.gameObject.SetActive(false);
                hud.gameObject.SetActive(true);
                player.resetPosition();
                break;
        }
    }

    //Use EnterState if possible
    void ExitState(GameStates state)
    {
        switch (state)
        {
            case GameStates.BOOT:
                break;
            case GameStates.PLAY:
                break;
        }
    }


    string GetPlayButtonText(int score)
    {
        if (score > 100)
        {
            return "It's Going Down For Real";
        }
        else if (score > 20)
        {
            return "If I Don't Stop These Guys From Taking My Personal Space, Who Will?";
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
}
