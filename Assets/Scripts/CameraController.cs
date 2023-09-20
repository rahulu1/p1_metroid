using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> rooms;
    [SerializeField]
    private List<RoomProperties> roomData;

    private Rigidbody playerRB;
    private PlayerState playerState;

    private bool roomTransitioning = false;
    private bool moveRight = true;

    private Vector3 newCamPosition;
    private Vector3 lerpRight, lerpLeft;

    [SerializeField]
    private int currRoom = 0;
    private int prevRoom;
    [SerializeField]
    private bool currIsHorizontal = true;
    [SerializeField]
    private float currMinRoomOffset;
    [SerializeField]
    private float currMaxRoomOffset;
    [SerializeField]
    private float currRoomMiddle;

    private float playerOffset;
    private float minPlayerOffset = -1f;
    private float maxPlayerOffset = 1f;

    private float lerpDuration = 1.9f;

    void Start()
    {
        lerpRight = new Vector3 (16, 0, 0);
        lerpLeft = new Vector3 (-16, 0, 0);

        newCamPosition = transform.position;

        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        UpdateCurrData ();

        StartCoroutine(WaitForRoomTransition());
    }

    IEnumerator WaitForRoomTransition()
    {
        while(true)
        {
            newCamPosition = transform.position;
            if(!roomTransitioning)
            {
                if(currMinRoomOffset != currMaxRoomOffset)
                {
                    // Camera Movement for Horizontal rooms
                    if (currIsHorizontal)
                    {
                        playerOffset = playerRB.position.x - transform.position.x;

                        // If player is too far right
                        if (playerOffset > maxPlayerOffset)
                        {
                            // If camera is not at right boundary
                            if ((playerRB.position.x - maxPlayerOffset) < currMaxRoomOffset)
                            {
                                newCamPosition.x = playerRB.position.x - maxPlayerOffset;
                            }
                            else
                                newCamPosition.x = currMaxRoomOffset;
                        }
                        // If player is too far left
                        else if (playerOffset < minPlayerOffset)
                        {
                            // If camera is not at left boundary
                            if ((playerRB.position.x - minPlayerOffset) > currMinRoomOffset)
                            {
                                newCamPosition.x = playerRB.position.x - minPlayerOffset;
                            }
                            else
                                newCamPosition.x = currMinRoomOffset;
                        }
                        newCamPosition.y = currRoomMiddle;
                    }
                    // Camera Movement for Vertical rooms
                    else
                    {
                        playerOffset = playerRB.position.y - transform.position.y;

                        // If player is too far up
                        if (playerOffset > maxPlayerOffset)
                        {
                            // If camera is not at uppper boundary
                            if ((playerRB.position.y - maxPlayerOffset) < currMaxRoomOffset)
                            {
                                newCamPosition.y = playerRB.position.y - maxPlayerOffset;
                            }
                            else
                                newCamPosition.y = currMaxRoomOffset;
                        }
                        // If player is too far down
                        else if (playerOffset < minPlayerOffset)
                        {
                            // If camera is not at lower boundary
                            if ((playerRB.position.y - minPlayerOffset) > currMinRoomOffset)
                            {
                                newCamPosition.y = playerRB.position.y - minPlayerOffset;
                            }
                            else newCamPosition.y = currMinRoomOffset;
                        }
                        newCamPosition.x = currRoomMiddle;
                    }
                    transform.position = newCamPosition;
                }
            }
            // Room is transitioning
            else
            {
                Vector3 destination;
                if (moveRight)
                    destination = transform.position + lerpRight;
                else
                    destination = transform.position + lerpLeft;

                yield return StartCoroutine(CoroutineUtilities.MoveObjectOverTime(transform, transform.position, destination, lerpDuration));
                UpdateCurrData();
                roomTransitioning = false;
            }
            /* We must yield here to let time pass, or we will hardlock the game (due to infinite while loop) */
            yield return null;
        }
    }

    public void StartRoomTransition(int toRoom, bool mR)
    {
        StartCoroutine(playerState.PausePlayerControls(lerpDuration + 0.1f));
        roomTransitioning = true;
        prevRoom = currRoom;
        currRoom = toRoom;
        moveRight = mR;
    }

    void UpdateCurrData()
    {
        currIsHorizontal = roomData[currRoom].IsHorizontal();
        currMinRoomOffset = roomData[currRoom].GetMinRoomOffset();
        currMaxRoomOffset = roomData[currRoom].GetMaxRoomOffset();
        currRoomMiddle = roomData[currRoom].GetRoomMiddle();
    }

    public bool IsCurrRoomHorizontal()
    {
        return currIsHorizontal;
    }

    public void EnableNextRoom(int toRoom)
    {
        rooms[toRoom].SetActive(true);
    }

    public void DisablePrevRoom()
    {
        rooms[prevRoom].SetActive(false);
    }
}
