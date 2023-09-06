using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 15f;
    
    private Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rb.velocity;

        // Horizontal
        newVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Vertical
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded())
        {
            newVelocity.y = jumpPower;
        }

        rb.velocity = newVelocity;
    }

    bool isGrounded()
    {
        Collider col = this.GetComponentInChildren<Collider>();

        Ray ray = new Ray(col.bounds.center, Vector3.down);

        float radius = col.bounds.extents.x - .05f;

        float fullDistance = col.bounds.extents.y + .05f;

        if (Physics.SphereCast(ray, radius, fullDistance))
            return true;
        else
            return false;
    }
}
