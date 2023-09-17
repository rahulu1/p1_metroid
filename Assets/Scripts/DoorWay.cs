using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
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
    private CameraController mainCamController;

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
            mainCamController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

            lerpRight = new Vector3(lerpDistance, 0f, 0f);
            lerpLeft = new Vector3(-lerpDistance, 0f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCollider"))
        {
            playerRB.velocity = Vector3.zero;
            playerRB.isKinematic = true;
            playerState.DisablePlayerCollider();
            StartCoroutine(playerState.PausePlayerControls(lerpDuration + 0.1f));

            originalPosition = playerTransform.position;

            if (moveRight)
                destinationPosition = originalPosition + lerpRight;
            else
                destinationPosition = originalPosition + lerpLeft;

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(playerTransform, originalPosition, destinationPosition, lerpDuration));
            mainCamController.StartRoomTransition(toRoom, moveRight);
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
        playerRB.isKinematic = false;
        playerState.EnablePlayerCollider();
        enterDoor.SetTransition(false);
        exitDoor.SetTransition(false);
    }
}
