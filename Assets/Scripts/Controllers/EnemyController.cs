using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 3;

    PlayerController player;
    Vector2 move;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
    }


    void FixedUpdate()
    {
        move = player.position - rb.position;
        move.Normalize();
        rb.position += move * Time.deltaTime * maxSpeed;
    }

    public void Die()
    {
        EventManager.NotifyEnemyDied();
        Destroy(this);
    }
}
