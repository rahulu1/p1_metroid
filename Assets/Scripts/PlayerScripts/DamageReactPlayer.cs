using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReactPlayer : DamageReact
{
    [SerializeField]
    private float invincibilityTime = 0.8f;
    [SerializeField]
    private float knockbackForce = 15f;
    [SerializeField]
    private float knockbackTime = 0.2f;

    public bool blinking = false;
    public int blinkFrames;

    private PlayerState playerState;
    
    protected override void Start()
    {
        base.Start();
        playerState = GetComponent<PlayerState>();
    }

    public override void ReactToDamage(DamageSource source, Vector3 DamagePos)
    {
        if(entityHealth.GetHealth() <= 0)
        {
            // make invincible
            Die();
        }
        else if(source == DamageSource.Enemy)
        {
            entityHealth.EnableInvincibility();
            StartCoroutine(DisableInvincibilityAfterTime(invincibilityTime));

            Knockback(DamagePos);
            if(!blinking)
            {
                blinking = true;
                blinkFrames = 31;
                StartCoroutine(Blink());
            }
            else
                blinkFrames = 31;
        }
        else if(source == DamageSource.Lava)
        {
            entityHealth.EnableInvincibility();
            StartCoroutine(DisableInvincibilityAfterTime(8f / 60f));

            if (!blinking)
            {
                blinking = true;
                blinkFrames = 7;
                StartCoroutine(Blink());
            }
            else
                blinkFrames = 7;
        }

    }

    protected override void Die()
    {
        playerState.DisablePlayerControls();
        playerState.DeathSequence();
    }

    IEnumerator DisableInvincibilityAfterTime(float timeInvincible)
    {
        yield return new WaitForSeconds(timeInvincible);
        entityHealth.DisableInvincibility();
    }

    void Knockback(Vector3 damagePos)
    {
        Vector3 knockbackDirection = (this.transform.position - damagePos).normalized;

        StartCoroutine(playerState.PausePlayerControls(knockbackTime));

        Vector3 knockback = knockbackDirection * knockbackForce;
        rigid.AddForce(knockback, ForceMode.Impulse);

        StartCoroutine(StopKnockback(knockbackTime));
    }

    // Stops knockback motion after knockbackTime seconds
    IEnumerator StopKnockback(float knockbackTime)
    {
        yield return new WaitForSeconds(knockbackTime);

        rigid.velocity = Vector3.zero;
    }

    IEnumerator Blink()
    {
        while (blinkFrames >= 0)
        {
            if ((blinkFrames % 2) == 1)
            {
                GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            else
                GetComponentInChildren<SpriteRenderer>().enabled = true;
            blinkFrames--;
            yield return null;
        }
        blinking = false;
    }
}
