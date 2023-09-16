using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterHealthOnTouch : MonoBehaviour
{
    public int healthDelta;

    private void OnTriggerEnter(Collider other)
    {
        HasHealth hasHealth = CheckForHasHealth(other.gameObject);

        if(hasHealth)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(-1 * healthDelta, damagePos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HasHealth hasHealth = CheckForHasHealth(collision.gameObject);

        if (hasHealth)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(-1 * healthDelta, damagePos);
        }
    }

    // Check for HasHealth component in parent (because Player's HasHealth component
    // is in parent of the collider) and self if not found in parent
    private HasHealth CheckForHasHealth(GameObject gameObject)
    {
        HasHealth hasHealth = gameObject.GetComponentInParent<HasHealth>();

        if (!hasHealth)
            hasHealth = gameObject.GetComponent<HasHealth>();

        return hasHealth;
    }
}
