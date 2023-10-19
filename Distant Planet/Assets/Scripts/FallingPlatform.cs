using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // Controls amount of time before platform starts to fall
    public float fallDelay = 1f;
    // Controls amount of time before platform is destroyed

    //private float destroyDelay = 2f;

    private Vector3 initialPosition;
    public float resetTimer = 5.0f;

    [SerializeField] private Rigidbody2D rb;

    // Determines what happens once certain objects collide with an object with this script
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }   

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(resetTimer);
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = initialPosition;
    }
}
