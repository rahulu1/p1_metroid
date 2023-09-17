using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipperController : EnemyController
{
    public float ripperSpeed;
    public bool movingRight;

    // Ripper will only detect layers in layerMask
    public LayerMask layerMask;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;

    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        SetStartingVelocity();
    }

    private void SetStartingVelocity()
    {
        Vector3 startingVelocity;

        if (movingRight)
            startingVelocity = Vector3.right * ripperSpeed;
        else
            startingVelocity = Vector3.left * ripperSpeed;

        rb.velocity = startingVelocity;
    }

    private void Update()
    {
        RestoreHealth();
        RipperMotion();
    }

    // Hack to make Ripper unkillable; right now, HasHealth is tied
    // to too many other things, like knockback, so can't just remove it;
    // should probably split HasHealth into multiple components at some
    // point if we have time
    private void RestoreHealth()
    {
        HasHealth hasHealth = this.GetComponent<HasHealth>();
        if(hasHealth.GetHealth() < 9999)
        {
            hasHealth.SetHealth(9999);
        }
    }

    private void RipperMotion()
    {
        if(RunningIntoWall())
        {
            FlipSprite();
            FlipVelocity();
        }
    }

    private bool RunningIntoWall()
    {
        Vector3 origin = this.transform.position;

        // Ray should be pointed in the direction of motion
        Vector3 direction = (rb.velocity).normalized;
        float rayLength = 0.5f;


        return Physics.Raycast(origin, direction, rayLength, layerMask);
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void FlipVelocity()
    {
        Vector3 newVelocity = rb.velocity;
        newVelocity.x *= -1;
        rb.velocity = newVelocity;
    }
}