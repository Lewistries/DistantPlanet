using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportRock : MonoBehaviour
{
    private List<GameObject> gameObjects;

    private int count = 0;

    public void Start()
    {
        gameObjects = new List<GameObject>
        {
            GameObject.Find("TeleporterA"),
            GameObject.Find("TeleporterB"),
        };
        count++;

        if(GameObject.Find("TeleporterC") != null && GameObject.Find("TeleporterD") != null) {
            gameObjects.Add(GameObject.Find("TeleporterC"));
            gameObjects.Add(GameObject.Find("TeleporterD"));
            count++;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && gameObject.Equals(gameObjects[0])) {
            collision.GetComponent<Rigidbody2D>().transform.position = gameObjects[1].gameObject.transform.position;
        }
        else if(collision.CompareTag("Player") && gameObject.Equals(gameObjects[1]))
        {
            collision.GetComponent<Rigidbody2D>().transform.position = gameObjects[0].gameObject.transform.position;
        }
        else if (count > 1 && collision.CompareTag("Player") && gameObject.Equals(gameObjects[2]))
        {
            collision.GetComponent<Rigidbody2D>().transform.position = gameObjects[3].gameObject.transform.position;
        }
        else if (count > 1 && collision.CompareTag("Player") && gameObject.Equals(gameObjects[3]))
        {
            collision.GetComponent<Rigidbody2D>().transform.position = gameObjects[2].gameObject.transform.position;
        }
    }


}


