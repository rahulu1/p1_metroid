using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerController : EnemyController
{
    public float zoomerSpeed;
    public bool moveRight;
    public LayerMask layerMask;

    private Rigidbody rb;
    private Vector3 turnAngle;
    private Vector3 down;

    private float directionMultiplier;
    private float timeSinceRotate;
    private float minTime;

    private const float downDistance = 0.5f, aheadDistance = 0.5f;
    private const float pixelShift = 0.03f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        minTime = (1f / zoomerSpeed) + 0.05f;
        timeSinceRotate = minTime;

        if (!moveRight)
        {
            turnAngle = new Vector3(0, 0, 90f);
            directionMultiplier = -1;
        }
        else
        {
            turnAngle = new Vector3(0, 0, -90f);
            directionMultiplier = 1;
        }
        zoomerSpeed *= directionMultiplier;
    }
    void Update()
    {
        timeSinceRotate += Time.deltaTime;
        down = transform.up * -1f;
        // if out of ground, turn 90 degrees down
        if (timeSinceRotate >= minTime)
        {
            if (!(IsGrounded()))
            {
                Debug.Log("No ground");
                this.transform.Rotate(turnAngle);
                AdjustPosition();
            }
            else if (!(HasSpace()))
            {
                Debug.Log("No space");
                this.transform.Rotate(turnAngle * -1f);
                AdjustPosition();
            }
        }
        rb.velocity = transform.right * zoomerSpeed;
    }

    // Checks if running out of ground
    bool IsGrounded()
    {
        Vector3 origin = this.transform.position - (this.transform.right * 0.49f * directionMultiplier);
        bool groundBelow = Physics.Raycast(origin, down, downDistance, layerMask);

        return groundBelow;
    }

    // Checks if running into wall
    bool HasSpace()
    {
        Vector3 origin = this.transform.position;

        bool wallInFront = Physics.Raycast(
            origin, this.transform.right * directionMultiplier, aheadDistance, layerMask);
        bool groundBelow = Physics.Raycast(origin, down, downDistance, layerMask);

        return !(wallInFront && groundBelow);
    }

    void AdjustPosition()
    {
        Vector3 newPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        this.transform.position = newPos - (transform.up * pixelShift);
        timeSinceRotate = 0f;
    }

    public override EnemyController GetController()
    {
        return this;
    }
}