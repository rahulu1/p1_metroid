using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public int health;
    private bool invincible = false;

    private DamageReact damageReact;

    void OnEnable()
    {
        damageReact = this.gameObject.GetComponent<DamageReact>();
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int i)
    {
        health = i;
    }

    public void AddHealth(int i)
    {
        health += i;
    }


    // damagePos is the (x, y) position of the source of the damage and
    // it's passed in by the attacking entity (bullet, enemy, etc)
    // through the attacking entity's OnColliderEnter function
    public void TakeDamage(int i, DamageReact.DamageSource source, Vector3 damagePos)
    {
        if (!invincible)
        {
            damageReact.ReactToDamage(i, source, damagePos);
        }
    }

    public void DirectDamage(int i)
    {
        health -= i;
    }

    public void EnableInvincibility() { invincible = true; }
    public void DisableInvincibility() { invincible = false; }
}
