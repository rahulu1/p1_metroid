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

    private bool isStanding;
    private bool cheatEnabled;
    private bool inLava;

    private PlayerInventory playerInventory;
    private PlayerWeapon playerWeapon;
    private HasHealth playerHealth;
    private Animator playerAnimator;
    void Start()
    {
        activeCollider = standing.GetComponent<Collider>();

        isStanding = true;
        cheatEnabled = false;
        inLava = false;
        playerInventory = GetComponent<PlayerInventory>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerHealth = GetComponent<HasHealth>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!cheatEnabled)
            {
                Debug.Log("Cheat Enabled");
                playerInventory.UnlockMissiles();
                playerWeapon.IncreaseMaxMissiles(255);
                playerWeapon.AddMissiles(255);
                playerHealth.EnableInvincibility();
            }
            else
                playerHealth.DisableInvincibility();
            cheatEnabled = !cheatEnabled;
        }

        if (IsGrounded())
        {
            if (isStanding && Input.GetKeyDown(KeyCode.DownArrow) && playerInventory.HasMorphBall())
            {
                standing.SetActive(false);
                morphed.SetActive(true);
                activeCollider = morphed.GetComponent<Collider>();
                isStanding = false;
                playerAnimator.SetTrigger("Morph");
            }
            if (!isStanding && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                if (HasSpace())
                {
                    standing.SetActive(true);
                    morphed.SetActive(false);
                    isStanding = true;
                    activeCollider = standing.GetComponent<Collider>();
                    playerAnimator.SetTrigger("Unmorph");
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
        Debug.Log(activeCollider);

        return Physics.SphereCast(ray, radius, fullDistance);        
    }

    public bool HasSpace()
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

    public bool IsMorphed()
    {
        return morphed.activeInHierarchy;
    }

    public bool CheatEnabled()
    {
        return cheatEnabled;
    }

    public bool GetInLava()
    {
        return inLava;
    }

    public void SetInLava(bool lavaBool)
    {
        inLava = lavaBool;
    }

    public void DisablePlayerControls()
    {
        GetComponent<PlayerRun>().enabled = false;
        GetComponentInChildren<PlayerJump>().enabled = false;
        GetComponent<HandleCustomGravity>().enabled = false;
        GetComponent<PlayerDirection>().enabled = false;
        GetComponentInChildren<PlayerWeapon>().enabled = false;
    }

    public void EnablePlayerControls()
    {
        GetComponent<PlayerRun>().enabled = true;
        GetComponentInChildren<PlayerJump>().enabled = true;
        GetComponent<HandleCustomGravity>().enabled = true;
        GetComponent<PlayerDirection>().enabled = true;
        GetComponentInChildren<PlayerWeapon>().enabled = true;
    }

    // Pauses all player controls for timePaused seconds
    public IEnumerator PausePlayerControls(float timePaused)
    {
        DisablePlayerControls();

        yield return new WaitForSeconds(timePaused);

        EnablePlayerControls();
    }   

    public void DisablePlayerCollider() { activeCollider.enabled = false; }

    public void EnablePlayerCollider() { activeCollider.enabled = true; }
    public void DeathSequence()
    {

        gameOverText.SetActive(true);
        HUD.SetActive(false);
        Camera.main.cullingMask = LayerMask.GetMask("UI");

        Destroy(this.gameObject);
        gameManager.enableRestart();
    }
}
