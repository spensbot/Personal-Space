using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Trackable
{
    [SerializeField] GameObject enemyDeathEffect;

    PlayerController player;
    Vector2 move;
    Rigidbody2D rb;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AudioManager.Instance.PlaySfx(SfxID.ENEMY_SPAWN);

        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
    }


    void FixedUpdate()
    {
        move = player.position - rb.position;
        move.Normalize();
        rb.position += move * Time.deltaTime * DifficultyManager.Instance.enemySpeed;
    }

    public void Die()
    {
        EventManager.NotifyEnemyDied();
        Instantiate(enemyDeathEffect, rb.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
