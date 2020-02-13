using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : Trackable
{
    [SerializeField] [Range(1, 3)] float baseRadius = 2f;
    [SerializeField] [Range(3, 6)] float maxRadius = 4f;

    [SerializeField] GameObject discExplosion;

    ParticleSystem pSystem;
    CircleCollider2D explosionCollider;

    //Magnitude should be between 0 and 1
    public void SetMagnitude(float magnitude)
    {
        magnitude = Mathf.Clamp(magnitude, 0f, 1f);
        float radius = magnitude * (maxRadius - baseRadius) + baseRadius;
        float speed = radius * 3f;

        pSystem = GetComponent<ParticleSystem>();
        explosionCollider = GetComponent<CircleCollider2D>();

        if (magnitude < 0.5f)
        {
            discExplosion.SetActive(false);
        }

        var main = pSystem.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(speed / 2, speed);

        explosionCollider.radius = radius;
    }

    private void OnDrawGizmos()
    {
        if (explosionCollider.isActiveAndEnabled)
        {
            Gizmos.DrawWireSphere(this.gameObject.transform.position, explosionCollider.radius);
        }

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        Invoke("DisableExplosionCollider", 0.2f);

        float totalDuration = pSystem.main.duration + pSystem.main.startLifetime.constantMax;
        Destroy(this.gameObject, totalDuration);     
    }

    void DisableExplosionCollider() { explosionCollider.enabled = false; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Strings.Tags.enemy))
        {
            collision.collider.gameObject.GetComponent<EnemyController>().Die();
        }
    }
}
