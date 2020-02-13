using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
    [Header("Initial Game Parameters")]
    [SerializeField] [Range(1f, 10f)] float startPlayerSpeed;
    [SerializeField] [Range(1f, 10f)] float startEnemySpeed;
    [SerializeField] [Range(1f, 3f)] float startEnemySpawn;
    [SerializeField] [Range(1f, 10f)] float startGateSpawn;

    [Header("Modify param % per minute of play")]
    // % change in player speed per minute of play
    [SerializeField] [Range(0f, 0.5f)] float modPlayerSpeed;
    // % change in enemy speed per minute of play
    [SerializeField] [Range(0f, 0.5f)] float modEnemySpeed;
    // % change in enemy spawn time per minute of play
    [SerializeField] [Range(-0.2f, 0f)] float modEnemySpawn;
    // % change in gate spawn time per minute of play
    [SerializeField] [Range(-0.25f, 0.25f)] float modGateSpawn;

    // Public values for use in game
    public float playerSpeed { get; private set; }
    public float enemySpeed { get; private set; }
    public float enemySpawnTime { get; private set; }
    public float gateSpawnTime { get; private set; }

    // To be called by the game manager to update public values.
    public void ModUpdate(float elapsedSeconds)
    {
        float elapsedMinutes = elapsedSeconds / 60f;
        playerSpeed = startPlayerSpeed * ( 1 + modPlayerSpeed * elapsedMinutes );
        enemySpeed = startEnemySpeed * ( 1 + modEnemySpeed * elapsedMinutes );
        enemySpawnTime = startEnemySpawn * ( 1 + modEnemySpawn * elapsedMinutes );
        gateSpawnTime = startGateSpawn * ( 1 + modGateSpawn * elapsedMinutes );

        //DEBUG VALUES
        DebugManager.Instance.Set(0, $"Elapsed Minutes: {elapsedMinutes}");
        DebugManager.Instance.Set(1, $"Player Speed: {playerSpeed}");
        DebugManager.Instance.Set(2, $"Enemy Speed: {enemySpeed}");
        DebugManager.Instance.Set(3, $"Enemy Spawn Time: {enemySpawnTime}");
        DebugManager.Instance.Set(4, $"Gate Spawn Time: {gateSpawnTime}");
    }
}
