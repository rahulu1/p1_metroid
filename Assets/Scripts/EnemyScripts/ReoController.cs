using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReoController : EnemyController
{
    public float initialYDescentSpeed;
    public float initialYAscentSpeed;
    public float initialXSpeed;
    public float yAccelerationFactor;
    public float xAccelerationFactor;
    public float aggroDistance;
    public float timeBetweenAttacks;

    // When the reo gets this far away from the ground, it stops
    // its swooping
    public float targetDistanceFromGround;
    public LayerMask layerMask;

    private Rigidbody rb;
    private Transform playerTransform;
    private Vector3 swoopDirection;

    private float initialSwoopYPosition;
    private float timeOfAttackEnd;
    private KeyCode jumpKey = KeyCode.Z;
    private bool isAttacking = false;
    private bool isAscending = false;


    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        timeOfAttackEnd = Time.time;
    }

    private void Update()
    {
        if(TimeToAttack())
        {
            if(!isAttacking)
            {
                swoopDirection = CalculateSwoopDirection();
                initialSwoopYPosition = this.transform.position.y;
                Attack();
                isAttacking = true;
            }
            else // Already attacking
            {
                Attack();
            }
        }
    }

    // Returns true if Reo should attack
    private bool TimeToAttack()
    {
        float XDistanceFromPlayer =
            this.transform.position.x - playerTransform.position.x;
        bool playerWithinAggroDistance = 
            XDistanceFromPlayer <= aggroDistance;

        bool timeBetweenAttacksEnded =
            (Time.time - timeOfAttackEnd) >= timeBetweenAttacks;

        return playerWithinAggroDistance && timeBetweenAttacksEnded;
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

    private void Attack()
    {
        Vector3 newVelocity = rb.velocity;

        if (!isAttacking)
        {
            newVelocity.x = initialXSpeed * swoopDirection.x;
            newVelocity.y = -initialYDescentSpeed;
        }

        if(!AtTargetHeight() && !isAscending)
        {
            newVelocity.y *= yAccelerationFactor;
            newVelocity.x *= xAccelerationFactor;
        }
        else if(isAscending)
        {
            Ascend();
        }
        else // At target height, not ascending
        {
            // Stop moving in y direction, and stop accelerating
            // in x direction
            newVelocity.y = 0f;

            if(CheckForPlayerJump())
                newVelocity = Ascend();
        }

        rb.velocity = newVelocity;
        //Debug.Log(rb.velocity.ToString());
    }

    private bool AtTargetHeight()
    {
        Vector3 origin = this.transform.position;
        Vector3 downwards = Vector3.down;
        float rayLength = targetDistanceFromGround;

        return Physics.Raycast(origin, downwards, rayLength, layerMask);
    }

    private bool CheckForPlayerJump()
    {
        return Input.GetKeyDown(jumpKey) ||
               Input.GetKey(jumpKey);
    }

    private Vector3 Ascend()
    {
        if(AtCeiling())
        {
            Debug.Log("At ceiling");
            isAttacking = false;
            timeOfAttackEnd = Time.time;
            return Vector3.zero;
        }

        Vector3 newVelocity = rb.velocity;

        if(!isAscending)
        {
            newVelocity.y = initialYAscentSpeed;
            isAscending = true;
        }

        newVelocity.x *= (1 / xAccelerationFactor); 
        newVelocity.y *= (1 / yAccelerationFactor);

        return newVelocity;
    }

    private bool AtCeiling()
    {
        Vector3 origin = this.transform.position;
        Vector3 upwards = Vector3.up;
        float rayLength = 0.65f;

        return Physics.Raycast(origin, upwards, rayLength, layerMask);
    }
}
