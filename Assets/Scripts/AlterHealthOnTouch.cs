using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterHealthOnTouch : MonoBehaviour
{
    public int healthDelta;

    private void OnTriggerEnter(Collider other)
    {
        HasHealth hasHealth = other.gameObject.GetComponentInParent<HasHealth>();
        if(hasHealth)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(-1 * healthDelta, damagePos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HasHealth hasHealth = collision.gameObject.GetComponentInParent<HasHealth>();
        if (hasHealth)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(-1 * healthDelta, damagePos);
        }
    }
}
