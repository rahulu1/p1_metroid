using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterHealthOnTouch : MonoBehaviour
{
    public int healthDelta;
    public DamageReact.DamageSource damageSource;
    Collider col;

    // Only alters health of layers in layerMask
    public LayerMask dontAlter;

    void Start()
    {
        col = GetComponent<Collider>();
        col.excludeLayers = dontAlter;
    }

    private void OnTriggerEnter(Collider other)
    {
        HasHealth hasHealth = CheckForHasHealth(other.gameObject);

        if(hasHealth)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(healthDelta, damageSource, damagePos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HasHealth hasHealth = CheckForHasHealth(collision.gameObject);

        if (hasHealth)
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            hasHealth.TakeDamage(healthDelta, damageSource, damagePos);
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
