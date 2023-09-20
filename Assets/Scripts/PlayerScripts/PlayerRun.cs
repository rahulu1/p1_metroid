using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    private Rigidbody rigid;

    private PlayerJump playerJump;
    private PlayerState playerState;

    public float moveSpeed = 5;

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
        playerJump = this.GetComponentInChildren<PlayerJump>();
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rigid.velocity;
        float input = Input.GetAxis("Horizontal");

        if (playerState.GetInLava())
            input /= 1.5f;

        if (playerJump.IsRunningJump())
        {
            bool playerInput = !Mathf.Approximately(input, 0f);
            if (playerInput)
            {
                newVelocity.x = input * moveSpeed;
            }

            // If no input, leave newVelocity.x as rigid.velocity.x to keep momentum
            // during running jump
        }
        else
        {
            newVelocity.x = input * moveSpeed;
        }

        rigid.velocity = newVelocity;
    }
}
