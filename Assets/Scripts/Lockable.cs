using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockable : MonoBehaviour
{
    protected bool unlocked = true; // true by default, should be locked by other scripts like Lock.cs

    public bool IsUnlocked()
    {
        return unlocked;
    }

    public virtual void Lock()
    {
        unlocked = false;
    }
    public virtual void Unlock()
    {
        unlocked = true;
    }
}
