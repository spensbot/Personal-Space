using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] [Range(1f, 5f)] const float defaultForeshadow = 3f;

    [SerializeField] GameObject gate;
    [SerializeField] GameObject foreshadow;
    [SerializeField] PlayerController player;

    float secondsToNextGate = 0f;
    float secondsToNextEnemy = 0f;

    void Start()
    {
        EventManager.EnterState += EnterState;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.EnterState -= EnterState;
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameState.PLAY)
        {
            float dt = Time.deltaTime;
            secondsToNextEnemy -= dt;
            secondsToNextGate -= dt;

            if (secondsToNextGate < 0)
            {
                SpawnGate();
                secondsToNextGate = DifficultyManager.Instance.gateSpawnTime;
            }

            if (secondsToNextEnemy < 0)
            {
                SpawnForeshadow();
                secondsToNextEnemy = DifficultyManager.Instance.enemySpawnTime;
            }
        }
    }

    void SpawnForeshadow(float secondsToEnemy = defaultForeshadow)
    {
        if (DevManager.Instance.SpawnEnemies)
        {
            Vector3 spawnPoint = GetRandomPointAlongRect(ScreenManager.Instance.PlayRectUnits);
            Debug.Log(spawnPoint);
            GameObject foreshadowInstance = Instantiate(foreshadow, spawnPoint, Quaternion.identity);
            ForeshadowController foreshadowController = foreshadowInstance.GetComponent<ForeshadowController>();
            foreshadowController.secondsToEnemy = secondsToEnemy;
        }
    }

    void SpawnGate()
    {
        Rect bombSpawnRect = ScreenManager.Shrink(ScreenManager.Instance.PlayRectUnits, 1f);
        Vector3 gateSpawnPoint = GetRandomPointInsideRect(bombSpawnRect);
        GameObject gateInstance = Instantiate(gate, gateSpawnPoint, Quaternion.identity) as GameObject;
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.BOOT:
                //Reset player position and clear enemies/gates/etc.
                break;
            case GameState.PLAY:
                //Now add some foreshadows with a shorter fuse so the player doesn't have to wait for enemies.
                player.resetPosition();
                DestroyAllInstantiatables();
                SpawnForeshadow(0f);
                break;
        }
    }

    Vector2 GetRandomPointAlongRect(Rect bounds)
    {
        Vector2 point = Vector2.zero;
        float randomSide = Random.Range(0, 4);

        //Clockwise
        if (randomSide < 1) //Top
        {
            point.y = bounds.yMax;
            point.x = Random.Range(bounds.xMin, bounds.xMax);
        } else if (randomSide < 2) //Right
        {
            point.x = bounds.xMax;
            point.y = Random.Range(bounds.yMin, bounds.yMax);
        } else if (randomSide < 3) //Bottom
        {
            point.y = bounds.yMin;
            point.x = Random.Range(bounds.xMin, bounds.xMax);
        } else //Left
        {
            point.x = bounds.xMin;
            point.y = Random.Range(bounds.yMin, bounds.yMax);
        }

        return point;
    }


    Vector2 GetRandomPointInsideRect(Rect bounds)
    {
        float x = Random.Range(bounds.xMin, bounds.xMax);
        float y = Random.Range(bounds.yMin, bounds.yMax);
        return new Vector2(x, y);
    }

    float RandomSign()
    {
        if (Random.value > 0.5)
        {
            return -1f;
        } else
        {
            return 1f;
        }
    }

    void DestroyAllInstantiatables()
    {
        EnemyController.DestroyAll();
        ForeshadowController.DestroyAll();
        GateController.DestroyAll();
        ExplosionController.DestroyAll();
        SelfDestruct.DestroyAll();
    }
}
