using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRange : MonoBehaviour
{
    private Collider aggroSphere;
    private bool inRange = false;

    void Start()
    {
        aggroSphere = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerCollider"))
            inRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCollider"))
            inRange = false;
    }

    public bool GetPlayerInRange()
    {
        return inRange;
    }
}
