using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public int health;
    public float invincibiltyTime;
    public float knockbackForce = 5f;
    public float freezeDuration;
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

    public int GetHealth()
    {
        return health;
    }

    public void AddHealth(int i)
    {
        health += i;
    }


    // damagePos is the (x, y) position of the source of the damage and
    // it's passed in by the attacking entity (bullet, enemy, etc)
    // through the attacking entity's OnColliderEnter function
    public void TakeDamage(int i, Vector3 damagePos)
    {
        if(timeSinceDamage >= invincibiltyTime)
        {
            if (health - i < 0)
                health = 0;
            else
                health -= i;


            if (this.gameObject.CompareTag("Player"))
            {
                if (playerState.CheatEnabled())
                    health += i;
                else
                    DamageReactPlayer(damagePos);
            }
            else
                DamageReactEnemy();
        }
    }

    void DamageReactPlayer(Vector3 damagePos)
    {
        GameObject player = this.gameObject;

        timeSinceDamage = 0f;

        if (health == 0)
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

    void DamageReactEnemy()
    {
        if(health == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            StartCoroutine(RestoreEnemyMovement(freezeDuration));
        }
        // TODO:
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

    IEnumerator RestoreEnemyMovement(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);

        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }
}
