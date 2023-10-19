using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    // How high the player will go when they land on one of these platforms
    public float heightBoost = 50;

    // Determines what happens once certain objects collide with this object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If a player object collides with this object, execute the following code
        if (collision.gameObject.CompareTag("Player"))
        {
            // Send the player upwards at a rate multiplied by the heightBoost immediately
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * heightBoost, ForceMode2D.Impulse);
        }
    }
}
