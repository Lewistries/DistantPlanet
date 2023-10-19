using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{

    public float scrollSpeedX;
    public float scrollSpeedY;

    new private MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 speed = new Vector2(scrollSpeedX, scrollSpeedY);
        renderer.material.mainTextureOffset = speed * Time.realtimeSinceStartup;
    }
}
