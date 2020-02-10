using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] GameObject gate;
    [SerializeField] GameObject foreshadow;
    [SerializeField] float enemySpawnTime;
    [SerializeField] float gateSpawnTime;
    [SerializeField] PlayerController player;

    List<GameObject> enemies;
    List<GameObject> gates;
    List<GameObject> foreshadows;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, enemySpawnTime);
        InvokeRepeating("SpawnGate", 0, gateSpawnTime);
        enemies = new List<GameObject>();
        gates = new List<GameObject>();
        foreshadows = new List<GameObject>();
    }

    void spawnForeshadow(float secondsToEnemy)
    {

    }

    void SpawnEnemy()
    {
        Vector3 enemySpawnPoint = GetRandomPointAlongRect(GameManager.Instance.screenBounds);
        GameObject enemyInstance = Instantiate(enemy, enemySpawnPoint, Quaternion.identity) as GameObject;
        Debug.Log(enemyInstance);
        enemies.Add(enemyInstance);
    }

    void SpawnGate()
    {
        Vector3 gateSpawnPoint = GetRandomPointInsideRect(GameManager.Instance.screenBounds);
        GameObject gateInstance = Instantiate(gate, gateSpawnPoint, Quaternion.identity) as GameObject;
        gates.Add(gateInstance);
    }

    void SpawnPlayer()
    {
        
    }

    public void resetGame()
    {
        player.resetPosition();
        clearList(enemies);
        clearList(gates);
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
