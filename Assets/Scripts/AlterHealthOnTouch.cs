using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterHealthOnTouch : MonoBehaviour
{
    public int healthDelta;

    // Only alters health of layers in layerMask
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        HasHealth hasHealth = CheckForHasHealth(other.gameObject);

        if(hasHealth && ShouldAlterHealth(other.gameObject))
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(-1 * healthDelta, damagePos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HasHealth hasHealth = CheckForHasHealth(collision.gameObject);

        if (hasHealth && ShouldAlterHealth(collision.gameObject))
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

    // Returns true if layer of gameObject isn't in layerMask, meaning
    // we should alter its health
    private bool ShouldAlterHealth(GameObject gameObject)
    {
        int layer = gameObject.layer;
        return Utilities.LayerInMask(layerMask, layer);
    }
}
