﻿using System;

public class EventManager
{
    //SUBSCRIBER EVENTS
    //Subscribe (+=) to be notified when an event is broadcast
    public static event Action PlayerDied;
    public static event Action EnemyDied;
    public static event Action GateDestroyed;
    public static event Action<int> ScoreChanged;
    public static event Action<int> HighScoreChanged;
    public static event Action<float> TotalPlayTimeChanged;
    public static event Action<GameState> ExitState;
    public static event Action<GameState> EnterState;


    //BROADCASTER METHODS
    //Call to broadcast event
    public static void NotifyPlayerDied()
    { PlayerDied?.Invoke(); }

    public static void NotifyEnemyDied()
    { EnemyDied?.Invoke(); }

    public static void NotifyGateDestroyed()
    { GateDestroyed?.Invoke(); }

    public static void NotifyScoreChange(int newScore)
    { ScoreChanged?.Invoke(newScore); }

    public static void NotifyHighScoreChange(int newHighScore)
    { HighScoreChanged?.Invoke(newHighScore); }

    public static void NotifyTotalPlayTimeChanged(float newTotalPlayTime)
    { TotalPlayTimeChanged?.Invoke(newTotalPlayTime); }

    public static void NotifyExitState(GameState state)
    { ExitState?.Invoke(state); }

    public static void NotifyEnterState(GameState state)
    { EnterState?.Invoke(state); }

}