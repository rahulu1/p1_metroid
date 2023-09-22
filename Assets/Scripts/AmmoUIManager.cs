using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : MonoBehaviour
{
    public Sprite[] digitSprites = new Sprite[10];
    public Image missileSprite;
    public Image[] displayAmmoDigits = new Image[3];

    private int displayAmmo = 0;
    private PlayerWeapon playerWeapon;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerWeapon>();
        displayAmmo = playerWeapon.GetMissileCount();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        if (playerInventory.MissilesUnlocked())
        {
            ShowAmmoUI();
            setAmmo();
        }
            
    }
    void Update()
    {
        if (displayAmmo != playerWeapon.GetMissileCount())
        {
            setAmmo();
        }
    }

    void setAmmo()
    {
        displayAmmo = playerWeapon.GetMissileCount();

        displayAmmoDigits[0].sprite = digitSprites[displayAmmo / 100];
        displayAmmoDigits[1].sprite = digitSprites[(displayAmmo % 100) / 10];
        displayAmmoDigits[2].sprite = digitSprites[displayAmmo % 10];
    }

    public void ShowAmmoUI()
    {
        missileSprite.enabled = true;
        foreach(Image displayAmmoDigit in displayAmmoDigits)
        {
            displayAmmoDigit.enabled = true;
        }
    }
}
