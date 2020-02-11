using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { BOOT, PLAY }

public class GameManager : Singleton<GameManager>
{
    [SerializeField] SpawnManager spawnManager;

    public Vector2 screenBounds { get; private set; }

    int score;
    GameState currentState;
    SaveState activeSave;

//----------------     LIFECYCLE METHODS     ---------------

    void Start()
    {
        activeSave = SaveManager.Load();
        updateHighScore(activeSave.highScore);

        DontDestroyOnLoad(this);
        
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        EventManager.EnemyDied += OnEnemyDied;
        EventManager.PlayerDied += OnPlayerDied;
        TransitionToState(GameState.BOOT);
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.EnemyDied -= OnEnemyDied;
        EventManager.PlayerDied -= OnPlayerDied;
    }


//-----------------     EVENTS     -------------------------

    void OnEnemyDied()
    {
        updateScore(score + 1);
    }

    void OnPlayerDied()
    {
        if (score > activeSave.highScore)
        {
            updateHighScore(score);
        }
        TransitionToState(GameState.BOOT);
    }

    public void StartGame()
    {
        TransitionToState(GameState.PLAY);
    }

    void updateScore(int newScore)
    {
        score = newScore;
        EventManager.NotifyScoreChange(score);
    }

    void updateHighScore(int newHighScore)
    {
        activeSave.highScore = newHighScore;
        SaveManager.Save(activeSave);
        EventManager.NotifyHighScoreChange(newHighScore);
    }


//--------------     STATE MANAGEMENT     ------------------

    void TransitionToState(GameState newState)
    {
        ExitState(currentState);
        EnterState(newState);
        currentState = newState;
    }

    void EnterState(GameState state)
    {
        EventManager.NotifyEnterState(state);
        switch (state)
        {
            case GameState.BOOT:
                updateScore(0);
                Time.timeScale = 0.0f;
                break;
            case GameState.PLAY:
                Time.timeScale = 1.0f;
                break;
        }
    }

    //Use EnterState when possible
    void ExitState(GameState state)
    {
        EventManager.NotifyExitState(state);
        switch (state)
        {
            case GameState.BOOT:
                break;
            case GameState.PLAY:
                break;
        }
    }
}
