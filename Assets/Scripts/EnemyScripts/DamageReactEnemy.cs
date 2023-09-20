using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReactEnemy : DamageReact
{
    public float freezeDuration;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void ReactToDamage(DamageSource source, Vector3 DamagePos)
    {
        if(entityHealth.GetHealth() <= 0)
        {
            Die();
        }
        else if(source == DamageSource.Projectile)
        {
            bool freezesOnHit = !Mathf.Approximately(freezeDuration, 0f);
            if (freezesOnHit)
            {
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

    IEnumerator RestoreEnemyMovement(float freezeDuration)
    {
        yield return new WaitForSeconds(freezeDuration);

        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.constraints = RigidbodyConstraints.FreezePositionZ;
    }
}
