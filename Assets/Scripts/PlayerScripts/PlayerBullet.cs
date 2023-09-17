using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    private int damage;
    private void OnTriggerEnter(Collider other)
    {
        // if it hits an enemy, deal damage
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<HasHealth>().TakeDamage(damage, this.gameObject.transform.position);
        }
        else if (other.gameObject.GetComponent<DoorCollider>() != null)
        {
            other.gameObject.GetComponent<DoorCollider>().OnProjectileCollision();
        }
        else if (other.gameObject.CompareTag("Chozo"))
        {
            other.GetComponent<ChozoController>().ProjectileEnter();
            Debug.Log("Hit Chozo");
        }
        Destroy(this.gameObject);
    }

    public void SetDamage(int i)
    {
        damage = i;
    }
}
