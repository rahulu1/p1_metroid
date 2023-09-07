using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public int health;
    public float invincibiltyTime;
    public float knockbackForce = 5f;

    private float timeSinceDamage;
    private Rigidbody rb;
     // Time entity is invincible

    void Awake()
    {
        timeSinceDamage = 0.8f;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        timeSinceDamage += Time.deltaTime;
    }

    public int getHealth()
    {
        return health;
    }

    public void addHealth(int i)
    {
        health += i;
    }


    // damageXPos is the X Position of the source of the damage and
    // it's passed in by the attacking entity (bullet, enemy, etc)
    // through the attacking entity's OnColliderEnter function
    public void takeDamage(int i, float damageXPos)
    {
        if(timeSinceDamage >= invincibiltyTime)
        {
            if (health - i < 0)
                health = 0;
            else
                health -= i;

            timeSinceDamage = 0f;
            DamageReact(damageXPos);
        }
    }

    void DamageReact(float damageXPos)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            DamageReactPlayer(damageXPos);
        }
    }

    void DamageReactPlayer(float damageXPos)
    {
        GameObject player = this.gameObject;
        if(health == 0)
        {
            player.GetComponent<PlayerRun>().enabled = false;
            player.GetComponentInChildren<PlayerJump>().enabled = false;
            player.GetComponentInChildren<PlayerWeapon>().enabled = false;
            player.GetComponent<PlayerState>().DeathSequence();

        }
        else
        {
            float xDiff = this.gameObject.transform.position.x - damageXPos;

            IEnumerator blink = player.GetComponent<PlayerState>().Blink();
            StartCoroutine(blink);

        }
    }
}
