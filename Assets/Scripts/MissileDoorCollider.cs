using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDoorCollider : DoorCollider
{
    private int missileHits = 0;
    // Start is called before the first frame update
    void Awake()
    {
        doorCollider = GetComponent<Collider>();
        doorRenderers = GetComponentsInChildren<SpriteRenderer>();
        excludeSamusAndBullets = LayerMask.GetMask("Player", "Bullet");
        excludeNothing = LayerMask.GetMask("Nothing");
    }

    protected override void Update()
    {
    }

    public override void OnProjectileCollision(GameObject projectile)
    {
        if(projectile.GetComponent<Missile>() != null)
        {
            if (missileHits == 4)
            {
                OpenDoor();
            }
            else
                missileHits++;
        }
    }

    public override void OpenDoor()
    {
        open = true;
        doorCollider.excludeLayers = excludeSamusAndBullets;
        foreach (SpriteRenderer doorRenderer in doorRenderers)
        {
            doorRenderer.enabled = false;
        }
        //TODO: replace with animation
    }

    public override void CloseDoor()
    {
        return;
    }

    public override void SetTransition(bool isMidTransition)
    {
        return;
    }
}
