using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rb.velocity;

        // Horizontal
        newVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        rb.velocity = newVelocity;
    }
}
