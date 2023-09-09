using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;

    bool isJumping = false;
    bool isRunningJump = false;
    bool balledUp = false;

    float jumpStartTime;
    int balledUpSpriteIndex;

    public float initialJumpForce = 10f;
    public float continuousJumpForce = 1f;
    public float maxJumpTime = 1.5f; // seconds
    public float tapJumpPressDuration = 0.1f;
    public float timeBeforeBallingUp = 0.5f;

    public SpriteRenderer spriteRenderer;

    public List<Sprite> balledUpSprites;
    public Sprite spriteJumping;
    public Sprite spriteStanding;

    private void Awake()
    {
        rigid = this.GetComponentInParent<Rigidbody>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpControl();
    }

    // Controls mechanics and motion similar to all jumps
    void JumpControl()
    {
        UnityEngine.KeyCode jumpKey = KeyCode.Z;
        bool jumpKeyPressed = Input.GetKeyDown(jumpKey);
        bool jumpKeyHeldDown = Input.GetKey(jumpKey);

        bool grounded = IsGrounded();
        bool running = rigid.velocity.x != 0;

        if (jumpKeyPressed && grounded)
        {
            rigid.AddForce(Vector3.up * initialJumpForce, ForceMode.Impulse);

            isJumping = true;
            jumpStartTime = Time.time;

            if (running)
                isRunningJump = true;
        }
        else if (jumpKeyHeldDown && isJumping)
        {
            float currJumpDuration = Time.time - jumpStartTime;

            if (isRunningJump)
                RunningJumpControl(currJumpDuration, grounded);

            bool maxJumpTimeReached = currJumpDuration >= maxJumpTime;

            if (maxJumpTimeReached)
            {
                isJumping = false;
                return;
            }

            // For consistency of tap jumps
            if (currJumpDuration > tapJumpPressDuration)
            {
                rigid.AddForce(Vector3.up * continuousJumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            if (isRunningJump)
            {
                // Need to keep running this even after button is no
                // longer pressed to keep rotation going while still
                // airborne
                RunningJumpControl(Time.time - jumpStartTime, grounded);
            }

            isJumping = false;
        }
    }

    // Controls balling up and rotating of running jumps
    void RunningJumpControl(float currJumpDuration, bool grounded)
    {
        bool timeToBallUp = currJumpDuration >= timeBeforeBallingUp;

        if (timeToBallUp && !balledUp)
        {
            BallUp();
            balledUp = true;
        }
        else if (balledUp)
        {
            if (grounded)
            {
                UndoBallUp();
                balledUp = false;
                isRunningJump = false;
                return;
            }

            RotateInAir();
        }
    }

    // Changes sprite and collider to balled up position
    void BallUp()
    {
        spriteRenderer.sprite = balledUpSprites[0];
        balledUpSpriteIndex = 0;

        Vector3 colliderCenter = new Vector3(0f, 0f, 0f);
        float colliderRadius = 0.7f;
        float colliderHeight = 1.6f;

        capsuleCollider.center = colliderCenter;
        capsuleCollider.radius = colliderRadius;
        capsuleCollider.height = colliderHeight;
    }

    // Change sprite and collider back to normal standing position
    void UndoBallUp()
    {
        spriteRenderer.sprite = spriteStanding;

        Vector3 colliderCenter = new Vector3(0f, -0.1f, 0f);
        float colliderRadius = 0.4f;
        float colliderHeight = 1.8f;

        capsuleCollider.center = colliderCenter;
        capsuleCollider.radius = colliderRadius;
        capsuleCollider.height = colliderHeight;
    }

    bool IsGrounded()
    {
        // Ray from the center of the collider down
        Ray ray = new Ray(capsuleCollider.bounds.center, Vector3.down);

        // Smaller than actual radius to prevent getting caught on walls
        float radius = capsuleCollider.bounds.extents.x - .05f;

        // Ray will extend a bit below the bottom of the collider
        float fullDistance = capsuleCollider.bounds.extents.y + .05f;

        return Physics.SphereCast(ray, radius, fullDistance);
    }

    // Changes to next balled up sprite in rotation
    void RotateInAir()
    {
        bool indexWillBeOutOfBounds = balledUpSpriteIndex >= balledUpSprites.Count - 1;
        if (indexWillBeOutOfBounds)
        {
            balledUpSpriteIndex = 0;
        }
        else
        {
            ++balledUpSpriteIndex;
        }

        spriteRenderer.sprite = balledUpSprites[balledUpSpriteIndex];
    }

    public bool IsRunningJump()
    {
        return isRunningJump;
    }
}
