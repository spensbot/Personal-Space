using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float rotationSpeed = 50; //Degrees per second
    Rigidbody2D rb;

    void Start()
    {
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
            Vector2 collisionPoint = collision.GetContact(0).point;
            float collisionDistance = (collisionPoint - rb.position).magnitude;
            GameObject explosionInstance = Instantiate(explosion, rb.transform);
            Destroy(this);
        }
    }
}
