using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerState>() != null)
            other.gameObject.GetComponentInParent<PlayerState>().SetInLava(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerState>() != null)
            other.gameObject.GetComponentInParent<PlayerState>().SetInLava(false);
    }
}
