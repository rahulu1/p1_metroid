using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWay : MonoBehaviour
{
    [SerializeField] private float lerpDuration = 4f;
    private float lerpProgress = 0f;
    [SerializeField] private int toRoom;
    [SerializeField] private bool moveRight;
    [SerializeField] private DoorCollider enterDoor;
    [SerializeField] private DoorCollider exitDoor;

    private Vector3 lerpRight, lerpLeft;

    private Transform playerTransform;
    private Rigidbody playerRB;
    private Collider playerCol;
    private PlayerRun playerRun;
    private PlayerJump playerJump;
    private PlayerWeapon playerWeapon;
    private PlayerDirection playerDirection;
    // private Animator playerAnimator;

    void Start()
    {
        // if this doorway tile doesn't have a collider, don't run this script
        if(GetComponent<Collider>() == null)
        {
            enabled = false;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerTransform = player.transform;
            playerRB = player.GetComponent<Rigidbody>();
            playerCol = player.GetComponent<Collider>();
            playerRun = player.GetComponent<PlayerRun>();
            playerJump = player.GetComponentInChildren<PlayerJump>();
            playerWeapon = player.GetComponentInChildren<PlayerWeapon>();
            playerDirection = player.GetComponent<PlayerDirection>();

            // playerAnimator.speed = player.GetComponent<Animator>();

            lerpRight = new Vector3(4, 0, 0);
            lerpLeft = new Vector3(-4, 0, 0);
        }
    }

    void Update()
    {
        if (lerpProgress < lerpDuration)
        {
            lerpProgress += Time.deltaTime;
            if (Mathf.Approximately(lerpProgress, lerpDuration / 2f))
            {
                enterDoor.SetOpen(false);
                exitDoor.SetOpen(false);
            }
            else if (Mathf.Approximately(lerpProgress, 0.75f * lerpDuration))
            {
                enterDoor.SetOpen(true);
                exitDoor.SetOpen(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lerpProgress = 0f;
            StartCoroutine(RoomTransition());
        }    
    }

    IEnumerator RoomTransition()
    {
        playerRB.velocity = Vector3.zero;
        playerRB.isKinematic = true;
        playerCol.enabled = false;
        playerRun.enabled = false;
        playerJump.enabled = false;
        playerWeapon.enabled = false;
        playerDirection.enabled = false;

        Vector3 originalPosition = playerTransform.position;
        Vector3 newPosition;

        float lerpFraction = lerpProgress / lerpDuration;

        if (moveRight)
            newPosition = playerTransform.position + lerpRight;
        else
            newPosition = playerTransform.position + lerpLeft;

        while(lerpProgress <= lerpDuration)
        {
            playerTransform.position = Vector3.Lerp(originalPosition, newPosition, lerpFraction);
            yield return null;
        }

        playerTransform.position = newPosition;
        playerRB.isKinematic = false;
        playerCol.enabled = true;
        playerRun.enabled = true;
        playerJump.enabled = true;
        playerWeapon.enabled = true;
        playerDirection.enabled = true;
    }
}
