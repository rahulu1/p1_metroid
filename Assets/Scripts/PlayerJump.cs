using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpPower = 15f;

    private PlayerState ps;
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = this.GetComponentInParent<Rigidbody>();
        ps = this.GetComponentInParent<PlayerState>();
        col = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rb.velocity;

        // Vertical
        if (Input.GetKeyDown(KeyCode.Z) && ps.isGrounded())
        {
            newVelocity.y = jumpPower;
        }

        rb.velocity = newVelocity;
    }

    
}
