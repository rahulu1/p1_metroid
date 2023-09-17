using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class ReoController2 : EnemyController
{

    private HandleCustomGravity reoGravity;
    private Collider reoCollider;

    private Rigidbody reoRigid;
    private Rigidbody playerRigid;

    private Vector3 reoDirection;
    private Vector3 targetPosition;
    private float minDistFromPlayer = 256f;
    private float targetAltitude;
    private bool swooping = false;


    void Start()
    {
        reoGravity = GetComponent<HandleCustomGravity>();
        reoCollider = GetComponent<Collider>();

        reoRigid = GetComponent<Rigidbody>();
        playerRigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        targetPosition = playerRigid.position;
        reoDirection = new Vector3(0, playerRigid.position.x - reoRigid.position.x, 0).normalized;

        StartCoroutine(ReoMove());
    }
    public override EnemyController GetController()
    {
        return this;
    }

    IEnumerator ReoMove()
    {
        while(true)
        {
            reoDirection = new Vector3(0, targetPosition.x - reoRigid.position.x, 0).normalized;

            // If reo is close enough
            if (Vector3.SqrMagnitude(playerRigid.position - reoRigid.position) < minDistFromPlayer)
            {
                targetPosition = playerRigid.position;

                // If not already swooping
                if (!swooping)
                {
                    targetAltitude = targetPosition.y;

                    if (!Mathf.Approximately(reoRigid.position.y, targetAltitude))
                    {
                        // if reo is below player, let gravity swoop it back up
                        if (targetAltitude > reoRigid.position.y)
                        {
                            reoGravity.enabled = true;
                            swooping = true;
                        }
                        // if reow is above player, have it "jump"
                        else
                            SwoopDown();
                    }
                }
            }
            yield return null;
        }
    }

    void SwoopDown()
    {

    }

    bool HasSpace()
    {
        Ray ray = new Ray(reoCollider.bounds.center, reoDirection);

        float fullDistance = reoCollider.bounds.extents.x + .05f;

        if (Physics.Raycast(ray, fullDistance))
            return false;
        else
            return true;
    }
}
