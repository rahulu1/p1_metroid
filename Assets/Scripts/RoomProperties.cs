using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    [SerializeField] private int roomNumber;
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float minRoomOffset, maxRoomOffset, roomMiddle;

    public int GetRoomNumber() { return roomNumber; }

    public bool IsHorizontal() { return isHorizontal; }

    public float GetMinRoomOffset () { return minRoomOffset; }

    public float GetMaxRoomOffset () { return maxRoomOffset; }

    public float GetRoomMiddle () { return roomMiddle; }
}
