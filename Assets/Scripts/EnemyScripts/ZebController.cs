using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZebController : EnemyController
{
    public float ySpeed;
    public float xSpeed;
    public LayerMask layerMask;

    private Rigidbody rb;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private Vector3 attackDirection;

    private enum ZebState
    { 
        Ascending, 
        Attacking
    }

    private ZebState state;


    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").transform;
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        attackDirection = CalculateAttackDirection();
        FaceTowardsPlayer(attackDirection);

        state = ZebState.Ascending;
    }

    private void Update()
    {
        Vector3 newVelocity = Vector3.zero;

        switch (state)
        {
            case ZebState.Ascending:
                newVelocity = Ascending();
                break;

            case ZebState.Attacking:
                newVelocity = Attacking();
                break;

            default:
                break;
        }

        rb.velocity = newVelocity;
    }

    private Vector3 Ascending()
    {
        Vector3 newVelocity = rb.velocity;
        
        if(AtPlayerHeight() || AtCeiling())
        {
            state = ZebState.Attacking;
            newVelocity = attackDirection * xSpeed;
        }
        else
        {
            newVelocity.y = ySpeed;
        }

        return newVelocity;
    }

    private Vector3 Attacking()
    {
        Vector3 newVelocity = rb.velocity;

        if(AtWall())
            ReverseXDirection(ref newVelocity);

        return newVelocity;
    }

    private bool AtPlayerHeight()
    {
        //return Mathf.Approximately(
        //    this.transform.position.y, playerTransform.position.y);

        float acceptableHeightDiff = 0.1f;
        float heightDiff = Mathf.Abs(
            this.transform.position.y - playerTransform.position.y);

        return heightDiff <= acceptableHeightDiff;
    }

    private bool AtCeiling()
    {
        Vector3 origin = this.transform.position;
        Vector3 upwards = Vector3.up;
        float rayLength = 0.5f;

        return Physics.Raycast(origin, upwards, rayLength, layerMask);
    }

    private bool AtWall()
    {
        Vector3 origin = this.transform.position;
        Vector3 direction = (rb.velocity).normalized;
        float rayLength = 0.5f;

        return Physics.Raycast(origin, direction, rayLength, layerMask);
    }

    private Vector3 CalculateAttackDirection()
    {
        bool playerToLeft = 
            playerTransform.position.x < this.transform.position.x; 

        if(playerToLeft)
            return Vector3.left;
        else
            return Vector3.right;
    }

    private void ReverseXDirection(ref Vector3 newVelocity)
    {
        newVelocity.x *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void FaceTowardsPlayer(Vector3 attackDirection)
    {
        bool playerToRight = attackDirection == Vector3.right;

        // By default, sprite is facing left
        if (playerToRight)
            spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
