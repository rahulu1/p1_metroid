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
}
