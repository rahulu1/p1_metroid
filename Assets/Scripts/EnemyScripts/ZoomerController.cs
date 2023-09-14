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
    private int damage = 8;



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
                Debug.Log("No Ground!");
                this.transform.Rotate(turnAngle);
                AdjustPosition();
            }
            else if (!(HasSpace()))
            {
                Debug.Log("No Space!");
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

        //if (Physics.Raycast(origin, down, downDistance))
        //    return true;
        //else
        //    return false;
    }

    // Checks if running into wall
    bool HasSpace()
    {
        Vector3 origin = this.transform.position;

        bool wallInFront = Physics.Raycast(
            origin, this.transform.right * directionMultiplier, aheadDistance, layerMask);
        bool groundBelow = Physics.Raycast(origin, down, downDistance, layerMask);

        return !(wallInFront && groundBelow);

        //if (Physics.Raycast(origin, this.transform.right * directionMultiplier, aheadDistance, layerMask) && 
        //    Physics.Raycast(origin, down, downDistance, layerMask))
        //    return false;
        //else
        //    return true;
    }

    void AdjustPosition()
    {
        Vector3 newPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        this.transform.position = newPos - (transform.up * pixelShift);
        timeSinceRotate = 0f;
    }

    //void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
    //        collision.gameObject.GetComponent<HasHealth>().TakeDamage(damage, damagePos);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        int playerLayer = 8;
        if (other.gameObject.layer == playerLayer)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            other.gameObject.GetComponentInParent<HasHealth>().TakeDamage(damage, damagePos);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 groundOrigin = this.transform.position - (this.transform.right * 0.49f);
        Vector3 groundEnd = groundOrigin + (down * downDistance);

        Vector3 origin = transform.position;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundOrigin, groundEnd);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + (transform.up * -downDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, origin + (transform.right * aheadDistance));
    }

    public override EnemyController GetController()
    {
        return this;
    }
}