using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    [SerializeField]
    private string scene;

    [SerializeField]
    private bool click = false;

    public void OnMouseOver()
    {
        
    }

    public void OnMouseOut() { 

    }

    public void OnClick()
    {
        click = true;
        if (scene != null && scene != "")
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
