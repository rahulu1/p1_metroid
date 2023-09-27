using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintText : MonoBehaviour
{
    protected int timesCharged = 0;
    protected int hintEvery = 2;
    // Start is called before the first frame update
    protected void Start()
    {
        Chargeable.on_charge += DisplayHint;
    }

    // Update is called once per frame
    protected void DisplayHint(int set)
    {
        if(set == 33)
        {
            if((timesCharged % hintEvery) == 0)
            {
                StartCoroutine(ShowHint());
            }
            timesCharged++;
        }
    }

    protected virtual IEnumerator ShowHint()
    {
        GetComponent<TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(6f);
        GetComponent<TextMeshProUGUI>().enabled = false;
    }
}
