using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReoController : EnemyController
{
    public float initialYDescentSpeed;
    public float initialYAscentSpeed;
    public float initialXSpeed;
    public float yAccelerationFactor;
    public float xAccelerationFactor;
    public float aggroDistance;
    public float timeBetweenAttacks;
    public float delayBeforeAscending;

    // When the reo gets this far away from the ground, it stops
    // its swooping
    public float targetDistanceFromGround;
    public LayerMask layerMask;

    private enum ReoState
    {
        Waiting,
        Swooping,
        Chasing,
        Ascending
    }

    private ReoState state;

    private Rigidbody rb;
    private Transform playerTransform;
    private Vector3 swoopDirection;

    private float timeOfAttackEnd;
    private float timeOfPlayerJump;
    private KeyCode jumpKey = KeyCode.Z;
    private bool waitingToAscend = false;


    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        state = ReoState.Waiting;
        rb = this.GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        timeOfAttackEnd = Time.time;
    }

    private void Update()
    {
        Vector3 newVelocity = Vector3.zero;

        switch (state)
        {
            case ReoState.Waiting:
                newVelocity = Waiting();
                break;

            case ReoState.Swooping:
                newVelocity = Swooping();
                break;

            case ReoState.Chasing:
                newVelocity = Chasing();
                break;

            case ReoState.Ascending:
                newVelocity = Ascending();
                break;

            default:
                break;
        }

        rb.velocity = newVelocity;
    }

    private Vector3 Waiting()
    {
        Vector3 newVelocity = Vector3.zero;

        if (TimeToAttack())
        {
            state = ReoState.Swooping;
            Debug.Log("Swooping");

            swoopDirection = CalculateSwoopDirection();
            newVelocity.x = initialXSpeed * swoopDirection.x;
            newVelocity.y = -initialYDescentSpeed;
        }

        return newVelocity;
    }

    private Vector3 Swooping()
    {
        Vector3 newVelocity = rb.velocity;

        if (AtTargetHeight())
        {
            state = ReoState.Chasing;
            Debug.Log("Chasing");

            newVelocity.y = 0f;
        }
        else
        {
            if (HittingWall())
                ReverseXDirection(ref newVelocity);

            newVelocity.y *= yAccelerationFactor;
            newVelocity.x *= xAccelerationFactor;
        }

        return newVelocity;
    }

    private Vector3 Chasing()
    {
        Vector3 newVelocity = rb.velocity;

        if (PlayerJumped() && !waitingToAscend)
            StartCoroutine(WaitToAscend(delayBeforeAscending));

        // Still need to check for wall collisions when waiting to
        // ascend
        if (HittingWall())
            ReverseXDirection(ref newVelocity);

        return newVelocity;
    }

    private Vector3 Ascending()
    {
        Vector3 newVelocity = rb.velocity;

        if (AtCeiling())
        {
            state = ReoState.Waiting;
            Debug.Log("Waiting");

            timeOfAttackEnd = Time.time;
            newVelocity = Vector3.zero;
        }
        else
        {
            newVelocity.x /= xAccelerationFactor;
            newVelocity.y /= yAccelerationFactor;
        }

        return newVelocity;
    }

    private bool TimeToAttack()
    {
        float XDistanceFromPlayer =
            Mathf.Abs(this.transform.position.x -
            playerTransform.position.x);

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

        if (playerToLeft)
            return Vector3.left;
        else // Player is to the right or directly below
            return Vector3.right;
    }

    private bool AtTargetHeight()
    {
        Vector3 origin = this.transform.position;
        Vector3 downwards = Vector3.down;
        float rayLength = targetDistanceFromGround;

        return Physics.Raycast(
            origin, downwards, rayLength, layerMask);
    }

    private bool PlayerJumped()
    {
        return Input.GetKeyDown(jumpKey) ||
               Input.GetKey(jumpKey);
    }

    private bool AtCeiling()
    {
        Vector3 origin = this.transform.position;
        Vector3 upwards = Vector3.up;
        float rayLength = 0.65f;

        return Physics.Raycast(
            origin, upwards, rayLength, layerMask);
    }

    private bool HittingWall()
    {
        Vector3 origin = this.transform.position;
        float rayLength = 0.8f;

        return Physics.Raycast(
            origin, swoopDirection, rayLength, layerMask);
    }

    private IEnumerator WaitToAscend(float duration)
    {
        waitingToAscend = true;
        yield return new WaitForSeconds(duration);

        state = ReoState.Ascending;
        waitingToAscend = false;
        Debug.Log("Ascending");

        Vector3 newVelocity = rb.velocity;
        newVelocity.y = initialYAscentSpeed;
        rb.velocity = newVelocity;
    }

    private void ReverseXDirection(ref Vector3 newVelocity)
    {
        swoopDirection *= -1;
        newVelocity.x *= -1;
    }
}