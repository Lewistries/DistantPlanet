using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float timeToTogglePlatform = 2;
    public float currentTime = 0;
    public bool visible = true;
    // Start is called before the first frame update
    void Start()
    {
        visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToTogglePlatform) {
            currentTime = 0;
            togglePlatform();
        }
    }

    private void togglePlatform()
    {
        visible = !visible;
        foreach(Transform child in gameObject.transform) 
        {
            if (child.tag != "Player")
            {
                child.gameObject.SetActive(visible);
            }
        }
    }
}
