using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTriggerEnter : MonoBehaviour
{

    private int damage;
    private void OnTriggerEnter(Collider other)
    {
        // if it hits an enemy, deal damage
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<HasHealth>().TakeDamage(damage, this.gameObject.transform.position);
        }
        else if(other.gameObject.GetComponent<DoorCollider>() != null)
        {
            other.gameObject.GetComponent<DoorCollider>().OnProjectileCollision();
        }
        Destroy(this.gameObject);
    }

    public void SetDamage(int i)
    {
        damage = i;
    }
}
