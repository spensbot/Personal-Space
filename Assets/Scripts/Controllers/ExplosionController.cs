using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    ParticleSystem pSystem;
    CircleCollider2D explosionCollider;

    public void UpdateSettings()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        explosionCollider = GetComponent<CircleCollider2D>();
        pSystem.Play();
        float totalDuration = pSystem.main.duration + pSystem.main.startLifetime.constantMax;
        Destroy(this, totalDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Strings.Tags.enemy))
        {
            collision.collider.gameObject.GetComponent<EnemyController>().Die();
        }
    }
}
