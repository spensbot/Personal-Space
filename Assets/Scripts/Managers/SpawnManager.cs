using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] [Range(1f, 5f)] const float defaultForeshadow = 3f;

    [SerializeField] GameObject gate;
    [SerializeField] GameObject foreshadow;
    [SerializeField] PlayerController player;

    List<GameObject> enemies;
    List<GameObject> gates;
    List<GameObject> foreshadows;

    float secondsToNextGate = 0f;
    float secondsToNextEnemy = 0f;

    void Start()
    {
        enemies = new List<GameObject>();
        gates = new List<GameObject>();
        foreshadows = new List<GameObject>();
        EventManager.EnterState += EnterState;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.EnterState -= EnterState;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        secondsToNextEnemy -= dt;
        secondsToNextGate -= dt;

        if(secondsToNextGate < 0)
        {
            SpawnGate();
            secondsToNextGate = DifficultyManager.Instance.gateSpawnTime;
        }

        if(secondsToNextEnemy < 0)
        {
            SpawnForeshadow();
            secondsToNextEnemy = DifficultyManager.Instance.enemySpawnTime;
        }
    }

    void SpawnForeshadow(float secondsToEnemy = defaultForeshadow)
    {
        Vector3 spawnPoint = GetRandomPointAlongRect(GameManager.Instance.screenBounds);
        GameObject foreshadowInstance = Instantiate(foreshadow, spawnPoint, Quaternion.identity);
        ForeshadowController foreshadowController = foreshadowInstance.GetComponent<ForeshadowController>();
        foreshadowController.secondsToEnemy = secondsToEnemy;
        foreshadowController.enemies = enemies;
        foreshadows.Add(foreshadowInstance);
    }

    void SpawnGate()
    {
        Vector3 gateSpawnPoint = GetRandomPointInsideRect(GameManager.Instance.screenBounds);
        GameObject gateInstance = Instantiate(gate, gateSpawnPoint, Quaternion.identity) as GameObject;
        gates.Add(gateInstance);
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.BOOT:
                //Reset player position and clear enemies/gates
                player.resetPosition();
                clearList(enemies);
                clearList(gates);
                clearList(foreshadows);
                break;
            case GameState.PLAY:
                //Now add some foreshadows with a shorter fuse so the player doesn't have to wait for enemies.
                SpawnForeshadow(3f);
                SpawnForeshadow(2f);
                SpawnForeshadow(1f);
                SpawnForeshadow(0f);
                SpawnGate();
                break;
        }
    }

    Vector2 GetRandomPointAlongRect(Vector2 bounds)
    {
        Vector2 point = Vector2.zero;
        float random = Random.Range(0, bounds.x + bounds.y);
        if (random > bounds.x)
        {
            point.x = bounds.x;
            point.y = random - bounds.x;
        } else
        {
            point.y = bounds.y;
            point.x = random;
        }
        point.x *= RandomSign();
        point.y *= RandomSign();

        return point;
    }


    Vector2 GetRandomPointInsideRect(Vector2 bounds)
    {
        float x = Random.Range(-bounds.x, bounds.x);
        float y = Random.Range(-bounds.y, bounds.y);
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

    void clearList(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            Destroy(obj);
        }
        list.Clear();
    }
}
