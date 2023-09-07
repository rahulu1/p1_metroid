using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firingPosForward;
    public Transform firingPosUp;

    public float firingSpeed = 10f;
    
    private PlayerDirection pd;

    void Awake()
    {
        pd = GetComponentInParent<PlayerDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject bulletInstance = GameObject.Instantiate(bulletPrefab);

            if (pd.IsLookingUp())
            {
                bulletInstance.transform.position = firingPosUp.position;
                bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.up * firingSpeed;
            }
            else
            {
                bulletInstance.transform.position = firingPosForward.position;
                if(pd.IsFacingRight())
                {
                    bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.right * firingSpeed;
                }
                else
                {
                    bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.left * firingSpeed;
                }
            }
        }
    }
}