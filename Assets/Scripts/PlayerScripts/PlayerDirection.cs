using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerDirection : MonoBehaviour
{
    public GameObject forwardPos;

    public SpriteResolver sr;

    public Sprite spriteLookForward;
    public Sprite spriteLookUp;

    private PlayerState playerState;
    private PlayerJump playerJump;

    private bool facingRight = true;
    private bool lookingUp = false;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerJump = GetComponentInChildren<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal
        float horizontalAxis = Input.GetAxis("Horizontal");
        if(facingRight &&  horizontalAxis < 0)
        {
            facingRight = false;
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (!facingRight && horizontalAxis > 0)
        {
            facingRight = true;
            this.transform.localScale = new Vector3(1, 1, 1);
        }


        // Vertical
        bool holdingUp = Input.GetKey(KeyCode.UpArrow);

        if (playerState.IsGrounded())
        {
            if (lookingUp && !holdingUp)
            {
                lookingUp = false;
            }
            else if (!lookingUp && holdingUp)
            {
                lookingUp = true;
            }
        }
        else if (playerJump.IsJumping())
        {
            if (!lookingUp && holdingUp)
            {
                lookingUp = true;
            }
        }
        else
        {
            lookingUp = false;
        }
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public bool IsLookingUp()
    {
        return lookingUp;
    }
}
