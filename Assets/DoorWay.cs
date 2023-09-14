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
    private Vector3 originalPosition;
    private Vector3 destinationPosition;

    private PlayerState playerState;
    private Transform playerTransform;
    private Rigidbody playerRB;
    private Collider playerCol;

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
            playerState = player.GetComponent<PlayerState>();
            playerTransform = player.transform;
            playerRB = player.GetComponent<Rigidbody>();
            playerCol = player.GetComponent<Collider>();

            lerpRight = new Vector3(4, 0, 0);
            lerpLeft = new Vector3(-4, 0, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCollider"))
        {
            StartCoroutine(playerState.DisablePlayerControls(lerpDuration + 0.2f));

            originalPosition = playerTransform.position;

            if (moveRight)
                destinationPosition = originalPosition + lerpRight;
            else
                destinationPosition = destinationPosition + lerpLeft;
            
            lerpProgress = 0f;
            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(playerTransform, originalPosition, destinationPosition, lerpDuration));
        }    
    }
}
