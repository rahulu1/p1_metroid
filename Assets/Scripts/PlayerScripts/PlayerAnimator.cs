using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerState playerState;
    private PlayerDirection playerDirection;
    private PlayerRun playerRun;

    private bool grounded = true;

    // Every state returns to standing by default
    // Use triggers for all shooting and morph - player weapon and state could do this
    // PlayerAnimator constantly checks for falling - could do this in playerstate
    // but Player Jump can trigger jump anim from
    // Any state except morphed

    /* Triggers, shoot, morph
     * Bools - running, looking up, In Air
     * 
     * covers running and jumping entirely
     * as well as morph and run jump
     * make sure all animations can exit to standing
     */


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
        playerDirection = GetComponent<PlayerDirection>();
        playerRun = GetComponent<PlayerRun>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if ground condition has changed
        if (grounded != playerState.IsGrounded())
        {
            grounded = playerState.IsGrounded();

            // If not on ground anymore, trigger InAir animation
            if (!grounded)
                animator.SetTrigger("InAir");
        }

        animator.SetBool("Grounded", playerState.IsGrounded());
        animator.SetBool("Running", playerRun.IsRunning());
        animator.SetBool("LookingUp", playerDirection.IsLookingUp());
    }
}
