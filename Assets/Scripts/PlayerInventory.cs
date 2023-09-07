using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    bool hasMB = false;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MorphBall"))
        {
            Destroy(other.gameObject);
            hasMB = true;
        }
    }

    public bool HasMorphBall()
    {
        return hasMB;
    }
}
