using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject missilePrefab;

    public Transform firingPosForward;
    public Transform firingPosUp;

    public float bulletSpeed = 10f;
    public float startingBeamBulletDuration;
    public float infiniteDuration = -1;

    private GameObject weaponPrefab;
    private GameObject bulletInstance;

    private SpriteRenderer bulletRenderer;

    private PlayerState playerState;
    private PlayerDirection playerDirection;
    private PlayerInventory playerInventory;
    private AmmoUIManager ammoUIManager;

    private const int beamEquipped = 0, missileEquipped = 1;
    private int weaponEquipped;

    private const int beamDamage = 1, missileDamage = 16;
    private int weaponDamage;

    private int maxMissiles;
    private int missileCount = 0;

    void Awake()
    {
        playerState = GetComponentInParent<PlayerState>();
        playerDirection = GetComponentInParent<PlayerDirection>();
        playerInventory = this.GetComponentInParent<PlayerInventory>();
        ammoUIManager = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<AmmoUIManager>();

        maxMissiles = 0;
        weaponEquipped = beamEquipped;
        weaponDamage = beamDamage;
        weaponPrefab = bulletPrefab;
    }

    // TODO: Refactor this code. Lots of duplication here
    void Update()
    {
        // Toggle between missiles and beam with shift
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (weaponEquipped == beamEquipped && playerInventory.MissilesUnlocked())
            {
                weaponEquipped = missileEquipped;
                weaponDamage = missileDamage;
                weaponPrefab = missilePrefab;
            }
            else if (weaponEquipped == missileEquipped)
            {
                weaponEquipped = beamEquipped;
                weaponDamage = beamDamage;
                weaponPrefab = bulletPrefab;
            }
                
        }

        if(Input.GetKeyDown(KeyCode.S))
        {

            if (weaponEquipped == beamEquipped)
            {
                bulletInstance = GameObject.Instantiate(weaponPrefab);
                DestroyOnTime destroyTimer = bulletInstance.GetComponent<DestroyOnTime>();
                if (playerInventory.HasLongBeam())
                    destroyTimer.destroyTime = infiniteDuration;
                else
                    destroyTimer.destroyTime = startingBeamBulletDuration;

            }
            else // if (weaponEquipped == missileEquipped)
            {
                if (missileCount <= 0) // if no missiles, don't shoot anything
                    return;

                bulletInstance = GameObject.Instantiate(weaponPrefab);
                DestroyOnTime destroyTimer = bulletInstance.GetComponent<DestroyOnTime>();

                destroyTimer.destroyTime = infiniteDuration;

                if(!(playerState.CheatEnabled()))
                    missileCount--;
            }

            bulletInstance.GetComponent<DestroyOnTriggerEnter>().SetDamage(weaponDamage);

            bulletRenderer = bulletInstance.GetComponent<SpriteRenderer>();

            bulletRenderer.flipX = playerDirection.IsFacingRight() ? false : true;

            if (playerDirection.IsLookingUp())
            {
                bulletRenderer.flipY = true;
                bulletInstance.transform.position = firingPosUp.position;
                bulletInstance.transform.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
                bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.up * bulletSpeed;
            }
            else
            {
                bulletInstance.transform.position = firingPosForward.position;
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
    }

    public int GetMissileCount()
    {
        return missileCount;
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