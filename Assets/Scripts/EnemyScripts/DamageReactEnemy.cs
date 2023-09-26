using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReactEnemy : DamageReact
{
    public float freezeDuration;

    public override void ReactToDamage(int i, DamageSource source, Vector3 damagePos)
    {
        if(source == DamageSource.Projectile)
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

    protected override void Die()
    {
        // This destroys the Enemy and has a chance to spawn a collectable in its place
        if (this.GetComponentInParent<SpawnController>() != null)
        {
            this.GetComponentInParent<SpawnController>().KillEntity();
        }
        else
            Destroy(this.gameObject);
    }

    protected IEnumerator RestoreEnemyMovement(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.constraints = RigidbodyConstraints.FreezePositionZ;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
