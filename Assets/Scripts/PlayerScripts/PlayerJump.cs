using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerState playerState;
    private Rigidbody rigid;

    [SerializeField]
    private float minJumpTime = 0.4f;
    private float currJumpTime = 0f;

    [SerializeField]
    private float jumpSpeed = 0.1f;

    [SerializeField]
    private bool grounded = true;
    private bool pressedJump = false;
    private bool holdingJump = false; // lets us know to keep adding jump velocity
    private bool isJumping = false; // used for animations
    private bool isRunningJump = false;
    private bool peaked = true;

    private KeyCode jumpKey = KeyCode.Z;

    private void Awake()
    {
        playerState = GetComponentInParent<PlayerState>();
        rigid = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame

    void Update()
    {
        JumpPhysics();    
    }

    // Controls mechanics and motion similar to all jumps
    void JumpPhysics()
    {
        pressedJump = Input.GetKeyDown(jumpKey);

        if (Input.GetKeyUp(jumpKey))
            holdingJump = false;

        grounded = playerState.IsGrounded();
        bool running = rigid.velocity.x != 0;

        if (grounded)
        {
            // starting a jump
            if (pressedJump)
            {
                AddJumpVelocity(jumpSpeed);

                holdingJump = true;
                isJumping = true;
                peaked = false;
                currJumpTime = 0f;

                if (running)
                    isRunningJump = true;
            }
            else
            {
                if(Mathf.Approximately(rigid.velocity.y, 0f))
                {
                    isJumping = false;
                    isRunningJump = false;
                }
                    
            }
        }
        else if (!peaked)
        {
            bool minJumpTimeReached = currJumpTime >= minJumpTime;
            currJumpTime += Time.deltaTime;
            if (minJumpTimeReached && !holdingJump)
            {
                Vector3 stopVelocity = rigid.velocity;
                stopVelocity.y = 0f;
                rigid.velocity = stopVelocity;
                peaked = true;
            }
            if (isJumping && (rigid.velocity.y <= 0f))
                peaked = true;
        }
    }

    void AddJumpVelocity(float addJumpSpeed)
    {
        Vector3 newVelocity = rigid.velocity + (Vector3.up * addJumpSpeed);
        rigid.velocity = newVelocity;
    }

    public bool IsRunningJump()
    {
        return isRunningJump;
    }
}
