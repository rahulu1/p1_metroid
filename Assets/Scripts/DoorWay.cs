using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWay : MonoBehaviour
{
    [SerializeField] private float lerpDuration = 3f;
    [SerializeField] private float lerpDistance = 4f;

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

            lerpRight = new Vector3(lerpDistance, 0f, 0f);
            lerpLeft = new Vector3(-lerpDistance, 0f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCollider"))
        {
            playerRB.velocity = Vector3.zero;
            StartCoroutine(playerState.DisablePlayerControls(lerpDuration + 0.1f));

            originalPosition = playerTransform.position;

            if (moveRight)
                destinationPosition = originalPosition + lerpRight;
            else
                destinationPosition = originalPosition + lerpLeft;

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(playerTransform, originalPosition, destinationPosition, lerpDuration));
            StartCoroutine(AnimateDoorsTransition());
            
        }    
    }

    IEnumerator AnimateDoorsTransition()
    {
        enterDoor.SetTransition(true);
        exitDoor.SetTransition(true);

        exitDoor.OpenDoor();
        yield return new WaitForSeconds(0.3f);
        enterDoor.CloseDoor();
        exitDoor.CloseDoor();
        yield return new WaitForSeconds(lerpDuration - 1.4f);
        exitDoor.OpenDoor();
        yield return new WaitForSeconds(0.2f);
        exitDoor.CloseDoor();
        enterDoor.SetTransition(false);
        exitDoor.SetTransition(false);
    }
}
