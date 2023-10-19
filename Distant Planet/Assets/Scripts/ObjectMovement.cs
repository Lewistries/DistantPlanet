using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveObject(transform, startPos, endPos, 3.0f);
        MoveObject(transform, endPos, startPos, 3.0f);
    }


    void MoveObject(Transform thisTransform, Vector3 start, Vector3 end, float time)
    {
        float i = 0;
        float rate = 1.0f / time;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(start, end, i);
        }
    }
}
