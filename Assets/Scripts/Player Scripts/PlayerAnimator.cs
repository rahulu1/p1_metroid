using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimator : MonoBehaviour
{
    public GameObject samusMorphedObject;
    public SpriteLibraryAsset defaultSpriteLibrary;
    public SpriteLibraryAsset missileSpriteLibrary;

    private Animator playerAnimator;
    private SpriteLibrary spriteLibrary;

    private PlayerState playerState;
    private PlayerDirection playerDirection;
    private PlayerWeapon playerWeapon;
    private PlayerJump playerJump;
    private Rigidbody playerRigidbody;

    public bool running = false;
    public bool lookingUp = false;
    public bool shooting = false;
    public bool grounded = true;
    public bool morphed = false;
    public bool jumping = false;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        playerState = GetComponent<PlayerState>();
        playerDirection = GetComponent<PlayerDirection>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerJump = GetComponentInChildren<PlayerJump>();
    }

    void Update()
    {
        lookingUp = playerDirection.IsLookingUp();
        shooting = playerWeapon.IsShooting();
        grounded = playerState.IsGrounded();
        morphed = samusMorphedObject.activeInHierarchy;
        jumping = playerJump.IsMidJump();

        if ((Input.GetAxis("Horizontal") != 0) && grounded)
            running = true;
        else
            running = false;

        playerAnimator.SetBool("Running", running);
        playerAnimator.SetBool("LookingUp", lookingUp);
        playerAnimator.SetBool("Shooting", shooting);
        playerAnimator.SetBool("Grounded", grounded);
        playerAnimator.SetBool("Morphed", morphed);
        playerAnimator.SetBool("Jumping", jumping);
    }

    public void SetSpriteLibrary(int weaponEquipped)
    {
        if (weaponEquipped == 0)
            spriteLibrary.spriteLibraryAsset = defaultSpriteLibrary;
        else if (weaponEquipped == 1)
            spriteLibrary.spriteLibraryAsset = missileSpriteLibrary;
    }
}
