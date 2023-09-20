using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeController : EnemyController
{
    public float skreeSpeed;
    public float aggroDistance;
    public float timeBeforeExploding;
    public float distanceToStopAdjusting;
    public float bulletSpeed;
    public float bulletDuration;

    public GameObject skreeBulletPrefab;

    public LayerMask layerMask;

    private Rigidbody rb;
    private Transform playerTransform;

    private bool hasAttacked = false;
    private bool hasLanded = false;
    private bool hasExploded = false;
    private float timeOfAttacking;
    private float timeOfLanding;

    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RecordVelocity(rb);

        if (hasExploded)
            return;

        if (!hasAttacked)
        {
            WaitingToAttack();
        }
        else if (hasLanded)
        {
            WaitingToExplode();
        }
        else // if currently attacking and in the air
        {
            CurrentlyAttacking();
        }
    }

    private void WaitingToAttack()
    {
        float XDistanceFromPlayer =
                this.transform.position.x - playerTransform.position.x;
        bool timeToAttack = XDistanceFromPlayer <= aggroDistance;

        if (timeToAttack)
            Attack();
    }

    private void WaitingToExplode()
    {
        float timeSinceLanding = Time.time - timeOfLanding;
        bool timeToExplode = timeSinceLanding >= timeBeforeExploding;

        if (timeToExplode)
            Explode();
    }

    private void CurrentlyAttacking()
    {
        if (IsGrounded())
        {
            hasLanded = true;
            timeOfLanding = Time.time;
            rb.velocity = Vector3.zero;
        }
        else
        {
            float YDistanceFromPlayer =
                this.transform.position.y - playerTransform.position.y;
            bool keepAdjusting = YDistanceFromPlayer > distanceToStopAdjusting;

            if (keepAdjusting)
                AdjustTrajectory();
            else
                rb.velocity = lastNonzeroVelocity;
        }
    }

    private void Attack()
    {
        hasAttacked = true;
        timeOfAttacking = Time.time;
        AdjustTrajectory();
    }

    private void AdjustTrajectory()
    {
        Vector3 attackTrajectory =
            (playerTransform.position - this.transform.position).normalized;

        Vector3 attackSpeed = attackTrajectory * skreeSpeed;
        rb.velocity = attackSpeed;
    }

    private bool IsGrounded()
    {
        Vector3 origin = this.transform.position;
        float rayLength = 0.75f;

        return Physics.Raycast(origin, Vector3.down, rayLength, layerMask);
    }

    private void Explode()
    {
        Vector3 rightBulletDirection = Vector3.right;
        Vector3 leftBulletDirection = Vector3.left;
        Vector3 topLeftBulletDirection = (Vector3.left + Vector3.up).normalized;
        Vector3 topRightBulletDirection = (Vector3.right + Vector3.up).normalized;

        Vector3 skreePos = this.transform.position;

        List<GameObject> bullets = new List<GameObject>(4);

        for (int i = 0; i < 4; ++i)
        {
            GameObject bullet = GameObject.Instantiate(skreeBulletPrefab);
            bullet.transform.position = skreePos;
            bullet.GetComponent<DestroyOnTime>().destroyTime = bulletDuration;
            bullets.Add(bullet);
        }

        GameObject rightBullet = bullets[0];
        GameObject leftBullet = bullets[1];
        GameObject topLeftBullet = bullets[2];
        GameObject topRightBullet = bullets[3];

        rightBullet.GetComponent<Rigidbody>().velocity = rightBulletDirection * bulletSpeed;
        leftBullet.GetComponent<Rigidbody>().velocity = leftBulletDirection * bulletSpeed;
        topLeftBullet.GetComponent<Rigidbody>().velocity = topLeftBulletDirection * bulletSpeed;
        topRightBullet.GetComponent<Rigidbody>().velocity = topRightBulletDirection * bulletSpeed;

        hasExploded = true;
        Destroy(this.gameObject);
    }
}