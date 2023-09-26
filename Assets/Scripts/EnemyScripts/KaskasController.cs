using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaskasController : EnemyController
{
    [SerializeField]
    private float kaskasSpeed;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float bulletDuration;

    private Vector3 moveDirection;

    [SerializeField]
    private GameObject kaskasBulletPrefab;

    [SerializeField]
    private PlayerInRange aggroChecker;
    private Rigidbody kaskasRigid;
    private Rigidbody playerRigid;


    public override EnemyController GetController()
    {
        return this;
    }

    // Start is called before the first frame update
    void Start()
    {
        kaskasRigid = GetComponentInParent<Rigidbody>();
        playerRigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        StartCoroutine(ShootPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        // if player in aggro distance
        if ( aggroChecker.GetPlayerInRange())
        {
            // update move direction to move towards the player
            moveDirection = (playerRigid.position - kaskasRigid.position).normalized;
            kaskasRigid.velocity = moveDirection * kaskasSpeed;

            Vector3 lookDirection = new Vector3(moveDirection.x, 0, 0).normalized;
            if (transform.right == (lookDirection * -1))
            {
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
        else
            kaskasRigid.velocity = Vector3.zero;
    }

    IEnumerator ShootPlayer()
    {
        while(true)
        {
            if (aggroChecker.GetPlayerInRange())
            {
                GameObject bullet = GameObject.Instantiate(kaskasBulletPrefab);
                bullet.transform.position = transform.position;
                bullet.GetComponent<DestroyOnTime>().destroyTime = bulletDuration;
                bullet.GetComponent<Rigidbody>().velocity = moveDirection * bulletSpeed;
                yield return new WaitForSeconds(fireRate);
            }
            else
                yield return null;
        }
    }
}
