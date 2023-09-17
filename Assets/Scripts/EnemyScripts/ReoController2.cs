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
    }

    void Update()
    {
        ReoMove();
    }

    public override EnemyController GetController()
    {
        return this;
    }

    void ReoMove()
    {
        if (!HasSpace())
        {

        }
        if (Vector3.SqrMagnitude(playerRigid.position - reoRigid.position) < minDistFromPlayer)
        {
            targetPosition = playerRigid.position;
            if (!swooping)
            {
                reoDirection = new Vector3(0, targetPosition.x - reoRigid.position.x, 0).normalized;
                targetAltitude = targetPosition.y;
                if (targetAltitude)
        }
        }
    }

    void Swoop()
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
