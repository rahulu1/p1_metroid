using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTriggerEnter : MonoBehaviour
{
    // Doesn't destroy itself when colliding with layers in layerMask
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        int colliderLayer = other.gameObject.layer;

        if (!Utilities.LayerInMask(layerMask, colliderLayer))
            Destroy(this.gameObject);
    }

}
