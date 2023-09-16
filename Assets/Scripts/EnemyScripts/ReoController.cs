using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReoController : EnemyController
{
    public float reoInitialYSpeed;
    public float reoInitialXSpeed;
    public float reoYAccelerationFactor;
    public float reoXAccelerationFactor;
    public float aggroDistance;
    public float timeBeforeSwoopingAgain;

    // When the reo gets this far away from the ground, it stops
    // its swooping
    public float targetDistanceFromGround;
    public LayerMask layerMask;

    private Rigidbody rb;
    private Transform playerTransform;
    private Vector3 swoopDirection;

    private float initialSwoopYPosition;
    private float distanceFromGround;
    private bool isAttacking = false;


    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        distanceFromGround = CalculateDistanceFromGround();
    }

    private void Update()
    {
        if(!isAttacking)
        {
            if(TimeToAttack())
            {
                swoopDirection = CalculateSwoopDirection();
                initialSwoopYPosition = this.transform.position.y;
                Swoop();
                isAttacking = true;
            }
        }
        else
        {
            Swoop();
        }
    }

    // Returns true when Samus is within aggroDistance
    private bool TimeToAttack()
    {
        float XDistanceFromPlayer =
            this.transform.position.x - playerTransform.position.x;
        return XDistanceFromPlayer <= aggroDistance;
    }

    private Vector3 CalculateSwoopDirection()
    {
        bool playerToLeft = 
            playerTransform.position.x < this.transform.position.x;

        if(playerToLeft)
            return Vector3.left;
        else // Player is to the right or directly below
            return Vector3.right;
    }

    private void Swoop()
    {
        Vector3 newVelocity = rb.velocity;

        if (!isAttacking)
        {
            newVelocity.x = reoInitialXSpeed * swoopDirection.x;
            newVelocity.y = -reoInitialYSpeed;
        }

        if(!AtTargetHeight())
        {
            newVelocity.y *= reoYAccelerationFactor;
            // Before getting to target height, accelerates in 
            // x direction
            newVelocity.x *= reoXAccelerationFactor;
        }
        else // At target height
        {
            // Stop moving in y direction, and stop accelerating
            // in x direction
            newVelocity.y = 0f;
        }

        rb.velocity = newVelocity;
        Debug.Log(rb.velocity.ToString());
    }

    private bool AtTargetHeight()
    {
        Vector3 origin = this.transform.position;
        Vector3 downwards = Vector3.down;
        float rayLength = targetDistanceFromGround;

        return Physics.Raycast(origin, downwards, rayLength, layerMask);
    }

    private float CalculateDistanceFromGround()
    {
        Vector3 origin = this.transform.position;
        Vector3 downwards = Vector3.down;
        float rayLength = 0.1f;

        while(!Physics.Raycast(origin, downwards, rayLength, layerMask))
        {
            rayLength += 0.1f;
        }

        return rayLength;
    }
}
