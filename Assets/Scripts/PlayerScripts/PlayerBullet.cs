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
            // Need to check because not all enemies have health; rippers
            // are invincible 
            HasHealth hasHealth = other.gameObject.GetComponent<HasHealth>();
            if(hasHealth)
            {
                hasHealth.TakeDamage(damage, DamageReact.DamageSource.Projectile, this.gameObject.transform.position);
                Debug.Log("Gottem");
            }
        }
        else if (other.gameObject.GetComponent<DoorCollider>() != null)
        {
            other.gameObject.GetComponent<DoorCollider>().OnProjectileCollision(this.gameObject);
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
