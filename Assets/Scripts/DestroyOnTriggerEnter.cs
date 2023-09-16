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

        if (!LayerInMask(layerMask, colliderLayer))
            Destroy(this.gameObject);
    }

    // Returns true if given layer is in layerMask, false if not
    private bool LayerInMask(LayerMask layerMask, int layer)
    {
        return (layerMask == (layerMask | (1 << layer)));
    }
}
