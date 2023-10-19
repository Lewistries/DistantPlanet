using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform[] stopPoints;

    public float moveSpeed;

    private int currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = stopPoints[currentPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Vector2.Distance(transform.position, stopPoints[currentPoint].position) < 0.02f) 
        {
            currentPoint++;
            if (currentPoint == stopPoints.Length)
            {
                currentPoint = 0;
            }       
        }
        transform.position = Vector2.MoveTowards(transform.position, stopPoints[currentPoint].position, moveSpeed * Time.deltaTime);
    }
}
