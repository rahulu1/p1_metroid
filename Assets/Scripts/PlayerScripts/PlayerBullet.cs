using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    private int damage;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit " + other.gameObject.name);
        // if it hits an enemy, deal damage
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<HasHealth>().TakeDamage(damage, DamageReact.DamageSource.Projectile, this.gameObject.transform.position);
            Debug.Log("Gottem");
        }
        else if (other.gameObject.GetComponent<DoorCollider>() != null)
        {
            other.gameObject.GetComponent<DoorCollider>().OnProjectileCollision();
        }
        else if (other.gameObject.CompareTag("Chozo"))
        {
            other.GetComponent<ChozoController>().ProjectileEnter();
        }
        Destroy(this.gameObject);
    }

    public void SetDamage(int i)
    {
        damage = i;
    }
}
