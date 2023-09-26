using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public GameObject beamerangPrefab;

    public Transform firingPosForward;
    public Transform firingPosUp;

    public float bulletSpeed = 10f;
    public float startingBeamBulletDuration;
    public float infiniteDuration = -1;

    private GameObject weaponPrefab;
    private GameObject bulletInstance;

    private BeamerangController beamerangController;

    private SpriteRenderer bulletRenderer;

    private PlayerState playerState;
    private PlayerDirection playerDirection;
    private PlayerInventory playerInventory;
    private AmmoUIManager ammoUIManager;
    private AudioPlayer audioPlayer;
    private Animator playerAnimator;

    private SpriteLibrary playerSpriteLib;

    [SerializeField]
    private SpriteLibraryAsset defaultSamusSprites;
    [SerializeField]
    private SpriteLibraryAsset missileSamusSprites;
    [SerializeField]
    private SpriteLibraryAsset beamerangSamusSprites;

    private enum weapon {  beam, missile, beamerang }
    [SerializeField]
    private PlayerWeapon.weapon weaponEquipped;

    private const int beamDamage = 1, missileDamage = 16, beamerangDamage = 1;
    private int weaponDamage;

    private int maxMissiles;
    private int missileCount = 0;

    void Awake()
    {
        playerState = GetComponentInParent<PlayerState>();
        playerDirection = GetComponentInParent<PlayerDirection>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        ammoUIManager = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<AmmoUIManager>();
        audioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
        playerAnimator = GetComponentInParent<Animator>();
        playerSpriteLib = GetComponentInParent<SpriteLibrary>();

        maxMissiles = 0;
        weaponEquipped = weapon.beam;
        weaponDamage = beamDamage;
        weaponPrefab = bulletPrefab;
    }

    // TODO: Refactor this code. Lots of duplication here
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SwitchWeapon();

        if (Input.GetKeyDown(KeyCode.X))
            UseWeapon();
    }

    void SwitchWeapon()
    {
        // Rotate between beam, missiles, and beamerang with spacebar
        if (weaponEquipped == weapon.beam)
        {
            if (playerInventory.MissilesUnlocked())
            {
                // Need to check that maxMissiles isn't 0 if missiles are unlocked, or
                // player won't be able to shoot missiles
                if (maxMissiles == 0)
                    IncreaseMaxMissiles(5);

                weaponEquipped = weapon.missile;
                weaponDamage = missileDamage;
                weaponPrefab = missilePrefab;
                playerSpriteLib.spriteLibraryAsset = missileSamusSprites;
            }
            else if (playerInventory.BeamerangUnlocked())
            {
                weaponEquipped = weapon.beamerang;
                weaponDamage = beamerangDamage;
                weaponPrefab = beamerangPrefab;
                playerSpriteLib.spriteLibraryAsset = beamerangSamusSprites;
            }
        }
        else if (weaponEquipped == weapon.missile)
        {
            if (playerInventory.BeamerangUnlocked())
            {
                weaponEquipped = weapon.beamerang;
                weaponDamage = beamerangDamage;
                weaponPrefab = beamerangPrefab;
                playerSpriteLib.spriteLibraryAsset = beamerangSamusSprites;
            }
            else
            {
                weaponEquipped = weapon.beam;
                weaponDamage = beamDamage;
                weaponPrefab = bulletPrefab;
                playerSpriteLib.spriteLibraryAsset = defaultSamusSprites;
            }
        }
        else if (weaponEquipped == weapon.beamerang)
        {
            weaponEquipped = weapon.beam;
            weaponDamage = beamDamage;
            weaponPrefab = bulletPrefab;
            playerSpriteLib.spriteLibraryAsset = defaultSamusSprites;
        }
    }

    void UseWeapon()
    {
        if (weaponEquipped == weapon.beam)
        {
            audioPlayer.playSfxClip("Fire");
            bulletInstance = GameObject.Instantiate(weaponPrefab);
            DestroyOnTime destroyTimer = bulletInstance.GetComponent<DestroyOnTime>();
            if (playerInventory.HasLongBeam())
                destroyTimer.destroyTime = infiniteDuration;
            else
                destroyTimer.destroyTime = startingBeamBulletDuration;

        }
        else if (weaponEquipped == weapon.missile)
        {
            if (missileCount <= 0) // if no missiles, don't shoot anything
                return;

            audioPlayer.playSfxClip("Missile");
            bulletInstance = GameObject.Instantiate(weaponPrefab);
            DestroyOnTime destroyTimer = bulletInstance.GetComponent<DestroyOnTime>();

            destroyTimer.destroyTime = infiniteDuration;

            if (!(playerState.CheatEnabled()))
                missileCount--;
        }
        else // if (weaponEquipped == weapon.beamerang)
        {
            audioPlayer.playSfxClip("Fire");

            // If there's no active beamerang, instantiate one
            if (beamerangController == null)
            {
                bulletInstance = GameObject.Instantiate(weaponPrefab);
                beamerangController = bulletInstance.GetComponent<BeamerangController>();
            }
            // If there's an active beamerang, toggle its state
            else
            {
                beamerangController.ToggleBeamerangState(transform.position);
                return;
            }
        }

        playerAnimator.SetTrigger("Shoot");

        // TODO: Delete this!!!
        if (weaponEquipped != weapon.beamerang)
            bulletInstance.GetComponent<PlayerBullet>().SetDamage(weaponDamage);

        bulletRenderer = bulletInstance.GetComponent<SpriteRenderer>();

        if (playerDirection.IsLookingUp())
        {
            bulletRenderer.flipY = true;
            bulletInstance.transform.Rotate(Vector3.forward * 90);
            bulletInstance.transform.position = firingPosUp.position;
            //bulletInstance.transform.rotation = firingPosUp.rotation;
            if(weaponEquipped != weapon.beamerang)
                bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.up * bulletSpeed;
        }
        else
        {
            bulletInstance.transform.position = firingPosForward.position;

            if(weaponEquipped != weapon.beamerang)
            {
                if (playerDirection.IsFacingRight())
                {
                    bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.right * bulletSpeed;
                }
                else
                {
                    bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.left * bulletSpeed;
                }
            }
        }
        if (!(playerDirection.IsFacingRight()) && !playerDirection.IsLookingUp())
        {
            bulletInstance.transform.Rotate(Vector3.up * 180);
        }
    }

    public int GetMissileCount()
    {
        return missileCount;
    }

    public int GetMaxMissiles()
    {
        return maxMissiles;
    }

    public void AddMissiles(int i)
    {
        if ((missileCount + i) <= maxMissiles)
            missileCount += i;
        else
            missileCount = maxMissiles;
    }

    public void IncreaseMaxMissiles(int i)
    {
        if (maxMissiles == 0)
            ammoUIManager.ShowAmmoUI();


        if ((maxMissiles + i) <= 255)
            maxMissiles += i;
        else
            maxMissiles = 255;
    }
}