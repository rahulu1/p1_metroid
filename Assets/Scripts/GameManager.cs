using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static bool hasMorphBall = false;
    private static bool hasLongBeam = false;
    private static bool missilesUnlocked = false;
    private static bool beamerangUnlocked = false;

    private static GameManager instance;

    private bool restart = false;


    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        Debug.Log("Missiles unlocked: " + missilesUnlocked.ToString());
    }

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

    public GameManager GetInstance()
    {
        return instance;
    }

    public bool MissilesUnlocked()
    {
        return missilesUnlocked;
    }

    public bool LongBeamUnlocked()
    {
        return hasLongBeam;
    }

    public bool MorphBallUnlocked()
    {
        return hasMorphBall;
    }

    public bool BeamerangUnlocked()
    {
        return beamerangUnlocked;
    }


    public void UnlockMorphBall()
    {
        hasMorphBall = true;
    }

    public void UnlockMissiles()
    {
        missilesUnlocked = true;
    }

    public void UnlockLongBeam()
    {
        hasLongBeam = true;
    }

    public void UnlockBeamerang()
    {
        beamerangUnlocked = true;
    }
}
