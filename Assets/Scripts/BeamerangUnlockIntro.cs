using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamerangUnlockIntro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.on_player_unlock += OnPlayerUnlock;
        GetComponent<Animator>().SetTrigger("Charge");
    }

    // Update is called once per frame
    void OnPlayerUnlock(string unlockedItem)
    {
        Debug.Log(unlockedItem);
        if (unlockedItem == "Beamerang")
        {
            GetComponent<Animator>().SetTrigger("Discharge");
        }
    }
}
