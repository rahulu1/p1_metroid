using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float destroyTime;
    // Start is called before the first frame update
    
    // if destroyTime == -1, destroy when exiting screenspace
    void Start()
    {
        if(destroyTime >= 0)
        {
            Destroy(this.gameObject, destroyTime);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
