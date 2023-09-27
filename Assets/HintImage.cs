using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintImage : HintText
{
    protected override IEnumerator ShowHint()
    {
        GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(6f);
        GetComponent<Image>().enabled = false;
    }
}
