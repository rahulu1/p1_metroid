using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool restart = false;

    void Update()
    {
        if (restart && Input.GetKeyDown(KeyCode.Return))
        {
            restart = false;
            Camera.main.cullingMask = LayerMask.GetMask("Everything");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
    }

    public void enableRestart()
    {
        restart = true;
        Debug.Log(restart);
    }
}
