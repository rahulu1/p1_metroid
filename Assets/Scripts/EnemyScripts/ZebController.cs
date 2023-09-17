using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZebController : EnemyController
{
    public float ySpeed;
    public float xSpeed;

    private Rigidbody rb;
    private Transform playerTransform;

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

        if(AtPlayerHeight())
            state = ZebState.Attacking;

        newVelocity.y = ySpeed;
        return newVelocity;
    }

    private Vector3 Attacking()
    {

    }

    private bool AtPlayerHeight()
    {
        return Mathf.Approximately(
            this.transform.position.y, playerTransform.position.y);
    }
}
