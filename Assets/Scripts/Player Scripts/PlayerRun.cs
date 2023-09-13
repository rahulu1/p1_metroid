using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rigid;
    private PlayerState playerState;
    private Animator playerAnimator;
    private PlayerJump playerJump;

    

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
        playerState = this.GetComponent<PlayerState>();
        playerAnimator = this.GetComponentInChildren<Animator>();
        playerJump = this.GetComponentInChildren<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rigid.velocity;
        float input = Input.GetAxis("Horizontal");


        if (playerState.IsGrounded() && playerState.IsStanding() && input != 0)
            playerAnimator.SetBool("Running", true);
        else
            playerAnimator.SetBool("Running", false);


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
