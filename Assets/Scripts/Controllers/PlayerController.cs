using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] GameObject playerSpawnPoint;

    Rigidbody2D rb;
    public Vector2 position { get { return rb.position; } }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        InputManager.Instance.updateInput();
    }


    void FixedUpdate()
    {
        Input2D input = InputManager.Instance.Input;
        if (input.magnitude > 0.0f)
        {
            rb.position += input.vector * Time.deltaTime * DifficultyManager.Instance.playerSpeed;
            rb.rotation = input.angle - 90;
        }
    }

    public void resetPosition()
    {
        rb.position = playerSpawnPoint.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Strings.Tags.enemy))
        {
            Die();
        }
    }

    void Die()
    {
        EventManager.NotifyPlayerDied();
    }
}

