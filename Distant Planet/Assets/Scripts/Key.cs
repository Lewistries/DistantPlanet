using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collide) 
    {
        if (collide.gameObject.CompareTag("Player"))
        {
            Destroy(door);
            Destroy(gameObject);
        }
    }

}
