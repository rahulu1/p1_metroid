using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firingPosForward;
    public Transform firingPosUp;

    public float bulletSpeed = 10f;
    public float startingBeamBulletDuration;
    public float longBeamBulletDuration;
    
    private PlayerDirection pd;
    private PlayerInventory playerInventory;

    void Awake()
    {
        pd = GetComponentInParent<PlayerDirection>();
        playerInventory = this.GetComponentInParent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject bulletInstance = GameObject.Instantiate(bulletPrefab);
            DestroyOnTime destroyTimer = bulletInstance.GetComponent<DestroyOnTime>();

            if (playerInventory.HasLongBeam())
                destroyTimer.destroyTime = longBeamBulletDuration;
            else
                destroyTimer.destroyTime = startingBeamBulletDuration;

            if (pd.IsLookingUp())
            {
                bulletInstance.transform.position = firingPosUp.position;
                bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.up * bulletSpeed;
            }
            else
            {
                bulletInstance.transform.position = firingPosForward.position;
                if(pd.IsFacingRight())
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
}