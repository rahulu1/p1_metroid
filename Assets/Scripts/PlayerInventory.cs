using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    bool hasMorphBall = false;
    bool hasLongBeam = false;

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MorphBall"))
        {
            Destroy(other.gameObject);
            hasMorphBall = true;
        }
        else if (other.gameObject.CompareTag("LongBeam"))
        {
            Destroy(other.gameObject);
            hasLongBeam = true;
        }
    }

    public bool HasMorphBall()
    {
        return hasMorphBall;
    }

    public bool HasLongBeam()
    {
        return hasLongBeam;
    }
}
