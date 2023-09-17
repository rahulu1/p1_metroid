using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    bool hasMorphBall = false;
    bool hasLongBeam = false;
    bool missilesUnlocked = false;

    private HasHealth playerHealth;
    private PlayerWeapon playerWeapon;

    private Collider col;
    private RaycastHit hit;

    void Start()
    {
        playerHealth = this.gameObject.GetComponent<HasHealth>();
        playerWeapon = this.gameObject.GetComponentInChildren<PlayerWeapon>();

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MorphBall"))
        {
            // TODO: Add Morph Ball Unlock Sequence
            Destroy(other.gameObject);
            hasMorphBall = true;
        }
        if (other.gameObject.CompareTag("MissileTank"))
        {

            // Add Missile Tank sequence
            missilesUnlocked = true;
            playerWeapon.IncreaseMaxMissiles(5);
            playerWeapon.AddMissiles(5);

            Destroy(other.transform.parent.gameObject);
        }
        else if (other.gameObject.CompareTag("LongBeam"))
        {
            Destroy(other.gameObject);
            hasLongBeam = true;
        }
        else if (other.gameObject.CompareTag("HealthOrb"))
        {
            Destroy(other.gameObject);
            playerHealth.AddHealth(5);
        }
        else if (other.gameObject.CompareTag("MissileAmmo"))
        {
            Destroy(other.gameObject);
            playerWeapon.AddMissiles(2);
        }
    }

    public bool HasMorphBall()
    {
        return hasMorphBall;
    }

    public bool HasLongBeam()
    {
        return hasLongBeam;
    }

    public bool MissilesUnlocked()
    {
        return missilesUnlocked;
    }

    public void UnlockMissiles()
    {
        Debug.Log("Missiles Unlocked (Cheat)");
        missilesUnlocked=true;
    }
}
