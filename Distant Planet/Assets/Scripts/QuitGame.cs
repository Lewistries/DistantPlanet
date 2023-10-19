using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void Close()
    {
        Application.Quit();
    }
}
