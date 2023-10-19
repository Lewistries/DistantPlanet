using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField]
    public string scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

  
    public void Load()
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

}
