using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassUpTriggerEnterToInventory : MonoBehaviour
{
    private PlayerInventory playerInventory;
    void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInventory.OnTriggerEnter(other);
    }
}
