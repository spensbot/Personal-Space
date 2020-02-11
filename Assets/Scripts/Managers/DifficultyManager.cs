using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
    [SerializeField] [Range(0f, 50f)] float startPlayerSpeed;
    [SerializeField] [Range(0f, 50f)] float startEnemySpeed;
    [SerializeField] [Range(0f, 50f)] float startEnemySpawn;
    [SerializeField] [Range(0f, 50f)] float startGateSpawn;

    // % change in player speed per minute of play
    [SerializeField] [Range(0f, 50f)] float modPlayerSpeed;
    // % change in enemy speed per minute of play
    [SerializeField] [Range(0f, 50f)] float modEnemySpeed;
        // % change in enemy spawn time per minute of play
    [SerializeField] [Range(0f, 0.01f)] float modEnemySpawn;
    // % change in gate spawn time per minute of play
    [SerializeField] [Range(0f, 0.01f)] float modGateSpawn;

    // Public values for use in game
    public float playerSpeed { get; private set; }
    public float enemySpeed { get; private set; }
    public float enemySpawnTime { get; private set; }
    public float gateSpawnTime { get; private set; }

    // To be called by the game manager to update public values.
    public void ModUpdate(float elapsedSeconds)
    {
        float elapsedMinutes = elapsedSeconds / 60f;
        playerSpeed = startPlayerSpeed + modPlayerSpeed * elapsedMinutes;
        enemySpeed = startEnemySpeed + modEnemySpeed * elapsedMinutes;
        enemySpawnTime = startEnemySpawn + modEnemySpawn * elapsedMinutes;
        gateSpawnTime = startGateSpawn + modGateSpawn * elapsedMinutes;

        //DEBUG VALUES
        DebugManager.Instance.Set(0, $"Elapsed Minutes: {elapsedMinutes}");
        DebugManager.Instance.Set(1, $"Player Speed: {playerSpeed}");
        DebugManager.Instance.Set(2, $"Enemy Speed: {enemySpeed}");
        DebugManager.Instance.Set(3, $"Enemy Spawn Time: {enemySpawnTime}");
        DebugManager.Instance.Set(4, $"Gate Spawn Time: {gateSpawnTime}");
    }
}
