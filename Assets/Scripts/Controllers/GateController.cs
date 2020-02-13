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
            GameObject explosionInstance = Instantiate(explosion, rb.position, Quaternion.Euler(0,0,rb.rotation)) as GameObject;
            ExplosionController explosionController = explosionInstance.GetComponent<ExplosionController>();

            Vector2 collisionPoint = collision.GetContact(0).point;
            float collisionDistance = (collisionPoint - rb.position).magnitude;
            float explosionMagnitude = 1f - collisionDistance / maxCollisionDistance;
            explosionController.SetMagnitude(explosionMagnitude);
            Destroy(this.gameObject);
        }
    }
}
