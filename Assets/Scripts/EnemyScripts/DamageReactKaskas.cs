using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReactKaskas : DamageReactEnemy
{
    protected override void Start()
    {
        entityHealth = GetComponent<HasHealth>();
        rigid = GetComponentInParent<Rigidbody>();
    }

    public override void ReactToDamage(int i, DamageSource source, Vector3 damagePos)
    {
        if (source == DamageSource.Projectile)
        {
            if(DamageFromBehind(damagePos))
            {
                entityHealth.DirectDamage(i);
                if (entityHealth.GetHealth() <= 0)
                {
                    Die();
                    return;
                }
                bool freezesOnHit = !Mathf.Approximately(freezeDuration, 0f);
                if (freezesOnHit)
                {
                    GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.2f, 0.2f, 1f);
                    rigid.constraints = RigidbodyConstraints.FreezePosition;
                    StartCoroutine(RestoreEnemyMovement(freezeDuration));
                }
            }
        }
    }

    bool DamageFromBehind(Vector3 DamagePos)
    {
        if (transform.right == Vector3.right)
        {
            if (transform.position.x > DamagePos.x)
                return true;
            else 
                return false;
        }
        else
        {
            if (transform.position.x < DamagePos.x)
                return true;
            else
                return false;
        }
    }
}
