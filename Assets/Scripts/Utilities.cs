using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // Returns true if given layer is in layerMask, false if not
    public static bool LayerInMask(LayerMask layerMask, int layer)
    {
        return (layerMask == (layerMask | (1 << layer)));
    }

    // Returns whether difference between two given transform's x
    // positions is less than given distance
    public static bool WithinXDistance(Transform transform1, 
        Transform transform2, float distance)
    {
        float XDistanceBetween =
            Mathf.Abs(transform1.position.x - transform2.position.x);

        return XDistanceBetween <= distance;
    }

    // Returns whether the given bool is the zero vector
    public static bool IsZeroVector(Vector3 vector)
    {
        return !Mathf.Approximately(vector.magnitude, 0f);
    }
}
