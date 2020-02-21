using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : Trackable
{
    [SerializeField] GameObject explosion;
    [SerializeField] float rotationSpeed = 50; //Degrees per second
    [SerializeField] float maxCollisionDistance = 1.3f;
    Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.rotation += rotationSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag(Strings.Tags.player))
        {
            //Calculate the magnitude of the explosion based on collision distance from center
            Vector2 collisionPoint = collision.GetContact(0).point;
            float collisionDistance = (collisionPoint - rb.position).magnitude;
            float explosionMagnitude = 1f - collisionDistance / maxCollisionDistance;

            //Instantiate an explosion particle effect with appropriate magnitude
            GameObject explosionInstance = Instantiate(explosion, rb.position, Quaternion.Euler(0,0,rb.rotation)) as GameObject;
            ExplosionController explosionController = explosionInstance.GetComponent<ExplosionController>();
            explosionController.SetMagnitude(explosionMagnitude);

            //Play explosion sound
            playExplosionSound(explosionMagnitude);

            //Vibrate device
            explosionVibrate(explosionMagnitude);

            //Slow Time
            explosionTimeSlow(explosionMagnitude);

            Destroy(this.gameObject);

            //I put in lots of effot to make gate hits impactful :)
        }
    }

    private void playExplosionSound(float magnitude)
    {
        if (magnitude > 0.5f)
        {
            AudioManager.Instance.PlaySfx(SfxID.EXPLOSION_LARGE);
        } else
        {
            AudioManager.Instance.PlaySfx(SfxID.EXPLOSION_SMALL);
        }
    }

    private void explosionVibrate(float magnitude)
    {
        if (magnitude > 0.5f)
        {
            VibrationManager.Instance.DecayingVibrate(800, 255, 60, true);
            //RDG.Vibration.Vibrate(new long[] { 40, 40, 60, 60, 80 }, new int[] { 255, 0, 150, 0, 75 });
        }
        else
        {
            VibrationManager.Instance.DecayingVibrate(400, 125, 30, false);
        }
    }

    private void explosionTimeSlow(float magnitude)
    {
        if (magnitude > 0.5f)
        {
            TimeManager.Instance.SlowTimeEffect(0.2f, 0.4f);
        }
        else
        {
            TimeManager.Instance.SlowTimeEffect(0.3f, 0.3f);
        }
    }
}
