using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChozoController : MonoBehaviour
{
    public float timeToClose = 6.5f;

    private Animator chozoAnimator;

    [SerializeField]
    private GameObject ChozoPickup;
    private void Start()
    {
        chozoAnimator = GetComponent<Animator>();
    }
    public void ProjectileEnter()
    {
        chozoAnimator.SetTrigger("Open");

        StartCoroutine(CloseAfterTime(timeToClose));
    }

    void EnablePickupIfExists()
    {
        if (ChozoPickup != null)
            ChozoPickup.SetActive(true);
    }

    IEnumerator CloseAfterTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if(ChozoPickup != null)
            ChozoPickup.SetActive(false);
        chozoAnimator.SetTrigger("Close");

    }
}
