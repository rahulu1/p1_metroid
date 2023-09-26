using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static System.Action<string> on_player_unlock;
    private GameManager gameManager;
    private HasHealth playerHealth;
    private PlayerWeapon playerWeapon;
    private AudioPlayer audioPlayer;

    private Collider col;
    private RaycastHit hit;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>().GetInstance();
        playerHealth = this.gameObject.GetComponent<HasHealth>();
        playerWeapon = this.gameObject.GetComponentInChildren<PlayerWeapon>();
        audioPlayer = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();



    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MorphBall"))
        {
            audioPlayer.playJingle("ItemJingle");
            // TODO: Add Morph Ball Unlock Sequence
            Destroy(other.gameObject);
            gameManager.UnlockMorphBall();
        }
        if (other.gameObject.CompareTag("MissileTank"))
        {
            audioPlayer.playJingle("ItemJingle");
            // Add Missile Tank sequence
            gameManager.UnlockMissiles();
            playerWeapon.IncreaseMaxMissiles(5);
            playerWeapon.AddMissiles(5);

            Destroy(other.transform.parent.gameObject);
        }
        else if (other.gameObject.CompareTag("LongBeam"))
        {
            audioPlayer.playJingle("ItemJingle");
            Destroy(other.gameObject);
            gameManager.UnlockLongBeam();
        }
        else if (other.gameObject.CompareTag("BeamerangPlasma"))
        {
            audioPlayer.playJingle("ItemJingle");
            Destroy(other.gameObject);
            gameManager.UnlockBeamerang();
            on_player_unlock("Beamerang");
        }
        else if (other.gameObject.CompareTag("HealthOrb"))
        {
            audioPlayer.playSfxClip("HealthPickup");
            Destroy(other.gameObject);
            playerHealth.AddHealth(5);
        }
        else if (other.gameObject.CompareTag("MissileAmmo"))
        {
            // Ensure that if the player has missiles unlocked, they are 
            // able to pick up ammo
            if(MissilesUnlocked() && playerWeapon.GetMaxMissiles() == 0)
                playerWeapon.IncreaseMaxMissiles(5);

            audioPlayer.playSfxClip("MissilePickup");
            Destroy(other.gameObject);
            playerWeapon.AddMissiles(2);
        }
    }

    public bool HasMorphBall()
    {
        return gameManager.MorphBallUnlocked();
    }

    public bool HasLongBeam()
    {
        return gameManager.LongBeamUnlocked();
    }

    public bool MissilesUnlocked()
    {
        return gameManager.MissilesUnlocked();
    }

    public bool BeamerangUnlocked()
    {
        return gameManager.BeamerangUnlocked();
    }

    public void UnlockMissiles()
    {
        gameManager.UnlockMissiles();
    }
}
