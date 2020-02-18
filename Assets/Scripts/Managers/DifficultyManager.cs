using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
    [Header("Initial Game Parameters")]
    [SerializeField] float startPlayerSpeed;
    [SerializeField] float startEnemySpeed;
    [SerializeField] float startEnemySpawn;
    [SerializeField] float startGateSpawn;

    [Header("Game Parameter Asymptotes")]
    [SerializeField] float approachPlayerSpeed;
    [SerializeField] float approachEnemySpeed;
    [SerializeField] float approachEnemySpawn;

    [Header("Asynmptote Power")]
    [SerializeField] float powPlayerSpeed;
    [SerializeField] float powEnemySpeed;
    [SerializeField] float powEnemySpawn;

    // Public values for use in game
    public float playerSpeed { get; private set; }
    public float enemySpeed { get; private set; }
    public float enemySpawnTime { get; private set; }
    public float gateSpawnTime { get; private set; }

    // To be called by the game manager to update public values.
    public void ModUpdate(float elapsedSeconds)
    {
        float elapsedMinutes = elapsedSeconds / 60f;
        playerSpeed = Asymptotic(elapsedMinutes, startPlayerSpeed, approachPlayerSpeed, powPlayerSpeed);
        enemySpeed = Asymptotic(elapsedMinutes, startEnemySpeed, approachEnemySpeed, powEnemySpeed);
        enemySpawnTime = Asymptotic(elapsedMinutes, startEnemySpawn, approachEnemySpawn, powEnemySpawn);
        gateSpawnTime = startGateSpawn;

        //DEBUG VALUES
        DevManager.Instance.Set(0, $"Elapsed Minutes: {elapsedMinutes}");
        DevManager.Instance.Set(1, $"Player Speed: {playerSpeed}");
        DevManager.Instance.Set(2, $"Enemy Speed: {enemySpeed}");
        DevManager.Instance.Set(3, $"Enemy Spawn Time: {enemySpawnTime}");
        DevManager.Instance.Set(4, $"Gate Spawn Time: {gateSpawnTime}");
    }

    //Returns values that approach "asymptote" as x -> infinity
    //Takes the form 1/x^pow
    //Uses algebra to shape the function based on tangible inputs.
    private float Asymptotic(float x, float initY, float asymptote, float pow)
    {
        return (initY - asymptote) / Mathf.Pow(x + 1f, pow) + asymptote; 
    }
}
