using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject standing;
    public GameObject morphed;
    public GameObject gameOverText;
    public GameObject HUD;

    public GameManager gameManager;

    [SerializeField]
    private Collider activeCollider;
    private CapsuleCollider capsuleCollider;
    private SphereCollider sphereCollider;

    private bool isStanding;
    private bool cheatEnabled;

    private PlayerInventory playerInventory;
    private PlayerWeapon playerWeapon;
    private Rigidbody playerRigidbody;
    private void Start()
    {
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        sphereCollider = GetComponentInChildren<SphereCollider>();
        activeCollider = capsuleCollider;

        isStanding = true;
        cheatEnabled = false;
        playerInventory = GetComponent<PlayerInventory>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Cheat Enabled");
            playerInventory.UnlockMissiles();
            playerWeapon.IncreaseMaxMissiles(255);
            playerWeapon.AddMissiles(255);
            cheatEnabled = !cheatEnabled;
        }

        if (IsGrounded())
        {
            if (isStanding && Input.GetKeyDown(KeyCode.DownArrow) && playerInventory.HasMorphBall())
            {
                activeCollider = sphereCollider;
                standing.SetActive(false);
                morphed.SetActive(true);
                isStanding = false;
            }
            if (!isStanding && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                if (HasSpace())
                {
                    activeCollider = capsuleCollider;
                    standing.SetActive(true);
                    morphed.SetActive(false);
                    isStanding = true;
                }
            }
        }
    }

    public bool IsGrounded()
    {
        // Ray from the center of the collider down
        Ray ray = new Ray(activeCollider.bounds.center, Vector3.down);

        // Smaller than actual radius to prevent getting caught on walls
        float radius = activeCollider.bounds.extents.x - .05f;

        // Ray will extend a bit below the bottom of the collider
        float fullDistance = activeCollider.bounds.extents.y - .28f;

        return Physics.SphereCast(ray, radius, fullDistance);
    }

    bool HasSpace()
    {
        activeCollider = this.GetComponentInChildren<Collider>();

        Ray ray = new Ray(activeCollider.bounds.center, Vector3.up);

        float radius = activeCollider.bounds.extents.x - .05f;

        float fullDistance = activeCollider.bounds.extents.y + .05f;

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
                GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            else
                GetComponentInChildren<SpriteRenderer>().enabled = true;
            blinkFrames--;
            yield return null;
        }
    }

    // Disables all player controls for timeDisabled seconds
    public IEnumerator DisablePlayerControls(float timeDisabled)
    {
        GetComponent<PlayerRun>().enabled = false;
        GetComponentInChildren<PlayerJump>().enabled = false;
        GetComponent<HandleCustomGravity>().enabled = false;
        GetComponent<PlayerDirection>().enabled = false;
        GetComponentInChildren<PlayerWeapon>().enabled = false;
        activeCollider.enabled = false;
        playerRigidbody.isKinematic = true;

        yield return new WaitForSeconds(timeDisabled);

        GetComponent<PlayerRun>().enabled = true;
        GetComponentInChildren<PlayerJump>().enabled = true;
        GetComponent<HandleCustomGravity>().enabled = true;
        GetComponent<PlayerDirection>().enabled = true;
        GetComponentInChildren<PlayerWeapon>().enabled = true;
        activeCollider.enabled = true;
        playerRigidbody.isKinematic = false;
    }   

    public void DeathSequence()
    {

        gameOverText.SetActive(true);
        HUD.SetActive(false);

        Destroy(this.gameObject);
        gameManager.enableRestart();
    }
}
