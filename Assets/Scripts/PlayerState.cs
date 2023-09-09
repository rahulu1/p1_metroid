using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class PlayerState : MonoBehaviour
{
    public GameObject standing;
    public GameObject morphed;
    public GameObject gameOverText;
    public GameObject HUD;

    public GameManager gameManager;

    private Collider col;
    private bool isStanding;
    private bool cheatEnabled;
    private PlayerInventory playerInventory;

    private void Start()
    {
        isStanding = true;
        cheatEnabled = false;
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cheatEnabled = !cheatEnabled;
        }

        if (isGrounded())
        {
            if (isStanding && Input.GetKeyDown(KeyCode.DownArrow) && playerInventory.HasMorphBall())
            {
                standing.SetActive(false);
                morphed.SetActive(true);
                isStanding = false;
            }
            if (!isStanding && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                if (hasSpace())
                {
                    standing.SetActive(true);
                    morphed.SetActive(false);
                    isStanding = true;
                }
            }
        }
    }

    public bool isGrounded()
    {
        col = this.GetComponentInChildren<Collider>();

        Ray ray = new Ray(col.bounds.center, Vector3.down);

        float radius = col.bounds.extents.x - .05f;

        float fullDistance = col.bounds.extents.y + .05f;

        if (Physics.SphereCast(ray, radius, fullDistance))
            return true;
        else
            return false;
    }

    bool hasSpace()
    {
        col = this.GetComponentInChildren<Collider>();

        Ray ray = new Ray(col.bounds.center, Vector3.up);

        float radius = col.bounds.extents.x - .05f;

        float fullDistance = col.bounds.extents.y + .05f;

        if (Physics.SphereCast(ray, radius, fullDistance))
            return false;
        else
            return true;
    }

    public bool CheatEnabled()
    {
        return cheatEnabled;
    }

    public IEnumerator Blink()
    {
        int blinkFrames = 31;
        Color blinkColor = new Color(0.46f, 0.38f, 0.38f);

        while (blinkFrames >= 0)
        {
            if ((blinkFrames % 2) == 1)
            {
                this.gameObject.GetComponentInChildren<SpriteRenderer>().color = blinkColor;
            }
            else
                this.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;

            yield return new WaitForSeconds(1f/60f);
            blinkFrames--;
        }
    }

    // Disables all player controls for timeDisabled seconds
    public IEnumerator DisablePlayerControls(float timeDisabled)
    {
        this.GetComponent<PlayerRun>().enabled = false;
        this.GetComponentInChildren<PlayerJump>().enabled = false;
        this.GetComponent<PlayerDirection>().enabled = false;

        yield return new WaitForSeconds(timeDisabled);

        this.GetComponent<PlayerRun>().enabled = true;
        this.GetComponentInChildren<PlayerJump>().enabled = true;
        this.GetComponent<PlayerDirection>().enabled = true;
    }

    public void DeathSequence()
    {

        gameOverText.SetActive(true);
        HUD.SetActive(false);

        Destroy(this.gameObject);
        gameManager.enableRestart();
    }
}
