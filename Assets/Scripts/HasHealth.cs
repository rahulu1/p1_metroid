using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public int health;
    public float invincibiltyTime;
    public float knockbackForce = 5f;
    public float knockbackTime;

    private float timeSinceDamage;
    private Rigidbody rb;
    private PlayerState playerState;
     // Time entity is invincible

    void Awake()
    {
        timeSinceDamage = 0.8f;
        rb = this.gameObject.GetComponent<Rigidbody>();
        playerState = this.gameObject.GetComponent<PlayerState>();
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


    // damagePos is the (x, y) position of the source of the damage and
    // it's passed in by the attacking entity (bullet, enemy, etc)
    // through the attacking entity's OnColliderEnter function
    public void takeDamage(int i, Vector3 damagePos)
    {
        if(timeSinceDamage >= invincibiltyTime)
        {
            if (health - i < 0)
                health = 0;
            else
                health -= i;

            timeSinceDamage = 0f;
            DamageReact(damagePos);
        }
    }

    void DamageReact(Vector3 damagePos)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            DamageReactPlayer(damagePos);
        }
    }

    void DamageReactPlayer(Vector3 damagePos)
    {
        GameObject player = this.gameObject;
        if(health == 0)
        {
            PlayerDie(player);
        }
        else
        {
            IEnumerator blink = player.GetComponent<PlayerState>().Blink();
            StartCoroutine(blink);

            PlayerKnockback(damagePos);
        }
    }

    void PlayerDie(GameObject player)
    {
        player.GetComponent<PlayerRun>().enabled = false;
        player.GetComponentInChildren<PlayerJump>().enabled = false;
        player.GetComponentInChildren<PlayerWeapon>().enabled = false;
        player.GetComponent<PlayerDirection>().enabled = false;
        player.GetComponent<PlayerState>().DeathSequence();
    }

    void PlayerKnockback(Vector3 damagePos)
    {
        Vector3 knockbackDirection = (this.transform.position - damagePos).normalized;

        StartCoroutine(playerState.DisablePlayerControls(knockbackTime));

        Vector3 knockback = knockbackDirection * knockbackForce;
        rb.AddForce(knockback, ForceMode.Impulse);

        StartCoroutine(StopKnockback(knockbackTime));
    }

    // Stops knockback motion after knockbackTime seconds
    IEnumerator StopKnockback(float knockbackTime)
    {
        yield return new WaitForSeconds(knockbackTime);

        rb.velocity = Vector3.zero;
    }
}
