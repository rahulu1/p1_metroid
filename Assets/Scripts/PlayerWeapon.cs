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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            GameObject bulletInstance = GameObject.Instantiate(bulletPrefab);

            if (pd.IsLookingUp())
            {
                bulletInstance.transform.position = firingPosForward.position;
                bulletInstance.GetComponent<Rigidbody>().velocity = Vector3.up * firingSpeed;
            }
            else
            {
                bulletInstance.transform.position = firingPosUp.position;
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