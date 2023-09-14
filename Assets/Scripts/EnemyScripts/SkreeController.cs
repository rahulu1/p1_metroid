using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkreeController : EnemyController
{
    public float skreeSpeed;
    public float distanceBeforeAttacking;
    public float timeBeforeExploding;
    public float distanceToStopAdjusting;
    public float bulletSpeed;

    public LayerMask layerMask;
    public Transform playerTransform;

    private Rigidbody rb;

    private bool hasAttacked = false;
    private bool hasLanded = false;
    private float timeOfAttacking;
    private float timeOfLanding;

    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!hasAttacked)
        {
            WaitingToAttack();
        }
        else if(hasLanded)
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
        bool timeToAttack = XDistanceFromPlayer <= distanceBeforeAttacking;

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

    }
}
