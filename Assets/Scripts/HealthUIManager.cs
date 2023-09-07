using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    public Sprite[] digitSprites = new Sprite[10];
    public SpriteRenderer[] displayHealthDigits = new SpriteRenderer[3];

    private int displayHealth;
    private HasHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HasHealth>();
        displayHealth = playerHealth.health;
    }

    // Update is called once per frame
    void Update()
    {
        if(displayHealth != playerHealth.getHealth())
        {
            setEn();
        }
    }

    // Update Health UI to match Player Health
    void setEn()
    {
        displayHealth = playerHealth.getHealth();

        string healthString = displayHealth.ToString();

        for(int i = 0; i < 3; i++)
        {
            // If digits of health < current UI digit, maake UI digit invisible
            if(healthString.Length < i + 1)
            {
                displayHealthDigits[i].enabled = false;
            }
            else
            {
                displayHealthDigits[i].enabled = true;
                displayHealthDigits[i].sprite = digitSprites[(int)char.GetNumericValue(healthString, i)];
            }
        }
    }
}
