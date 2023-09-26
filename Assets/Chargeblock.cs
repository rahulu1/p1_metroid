using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargeblock : Lockable
{
    private bool unlockedOnce = false;
    // Update is called once per frame
    void Update()
    {
        if (!unlockedOnce)
        {
            if (unlocked)
            {
                GetComponent<Animator>().SetTrigger("Charge");
                unlockedOnce = true;
            }
        }
    }
}
