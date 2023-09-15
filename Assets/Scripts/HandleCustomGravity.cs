using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCustomGravity : MonoBehaviour
{
    public float customGravity = 16f;

    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleGravity();
    }

    void HandleGravity()
    {
        rigid.velocity -= Vector3.up * customGravity * Time.fixedDeltaTime;
    }
}
