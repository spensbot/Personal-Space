using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;


/// <summary>
/// Things to do NEXT!!!!!
///
/// 
/// 
/// </summary>

public enum GameState { BOOT, PLAY, SETTINGS }

public class GameManager : Singleton<GameManager>
{
    int score;
    float playTime;
    public GameState currentState { get; private set; }
    public SaveState activeSave;

    //----------------     LIFECYCLE METHODS     ---------------

    protected override void Awake()
    {
        base.Awake();
        activeSave = SaveManager.Load();
        DontDestroyOnLoad(this);
    }

    void Start()
    {   
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

    private void Update()
    {
        if (currentState == GameState.PLAY)
        {
            playTime += Time.deltaTime;
            DifficultyManager.Instance.ModUpdate(playTime);
        }
    }


    //-----------------     EVENTS     -------------------------

    void OnEnemyDied()
    {
        updateScore(score + 1);
    }

    void OnPlayerDied()
    {
        if (DevManager.Instance.AllowPlayerDeath)
        {
            AudioManager.Instance.RestartSong();
            TransitionToState(GameState.BOOT);
            SaveManager.Save(activeSave);
            AudioManager.Instance.PlaySfx(SfxID.PLAYER_DIED);
        }
    }

    public void StartGame()
    {
        TransitionToState(GameState.PLAY);
    }

    public void ToggleSettings()
    {
        if (currentState == GameState.BOOT)
        {
            TransitionToState(GameState.SETTINGS);
        }
        else if (currentState == GameState.SETTINGS)
        {
            TransitionToState(GameState.BOOT);
        }
        //If this is somehow called during the play state, nothing will happen.
    }

    void updateScore(int newScore)
    {
        score = newScore;
        EventManager.NotifyScoreChange(score);
    }

    void updatePlayTime(float newPlayTime)
    {
        playTime = newPlayTime;
        DifficultyManager.Instance.ModUpdate(playTime);
    }

    void updateHighScore()
    {
        if (score > activeSave.highScore)
        {
            activeSave.highScore = score;
        }
        EventManager.NotifyHighScoreChange(activeSave.highScore);
    }

    void updateTotalPlayTime()
    {
        activeSave.totalPlayTime += playTime;
        EventManager.NotifyTotalPlayTimeChanged(activeSave.totalPlayTime);
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
                TimeManager.Instance.SetTimeScale(0.0f);
                updateHighScore();
                updateTotalPlayTime();
                DifficultyManager.Instance.ModUpdate(0f);
         
                break;
            case GameState.PLAY:
                TimeManager.Instance.SetTimeScale(1.0f);
                updateScore(0);
                updatePlayTime(0f);
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
                TimeManager.Instance.ClearEffects();
                break;
        }
    }
}
