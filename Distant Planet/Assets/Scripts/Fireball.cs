using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody2D rb;

    // Controls how much force will be applied to a fireball when it bounces
    public int heightMultiplier = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * heightMultiplier, ForceMode2D.Impulse);
    }

    // When a gameobject with this script touches an object with a trigger condition that isn't a player,
    // it will call the jump function.
    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Fall")) {
            Jump();
        }
    }
}
