using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockable : MonoBehaviour
{
    protected bool unlocked = false;

    public bool IsUnlocked()
    {
        return unlocked;
    }

    public virtual void Unlock()
    {
        unlocked = true;
    }
}
