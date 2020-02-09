using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject gate;
    [SerializeField] float enemySpawnTime;
    [SerializeField] float gateSpawnTime;
    [SerializeField] PlayerController player;

    List<GameObject> enemies;
    List<GameObject> gates;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, enemySpawnTime);
        InvokeRepeating("SpawnGate", 0, gateSpawnTime);
    }

    void SpawnEnemy()
    {
        Vector3 enemySpawnPoint = GetRandomPointAlongRect(GameManager.Instance.screenBounds);
        enemies.Add(Instantiate(enemy, enemySpawnPoint, Quaternion.identity));
    }

    void SpawnGate()
    {
        Vector3 gateSpawnPoint = GetRandomPointInsideRect(GameManager.Instance.screenBounds);
        gates.Add(Instantiate(gate, gateSpawnPoint, Quaternion.identity));
    }

    void SpawnPlayer()
    {
        
    }

    public void resetGame()
    {
        player.resetPosition();
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
}
