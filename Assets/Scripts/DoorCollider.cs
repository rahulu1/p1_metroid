using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    private float timeOpen;
    [SerializeField] private float maxTimeOpen = 3.5f;

    private bool midTransition = false;
    protected bool open = false;

    protected Collider doorCollider;
    protected SpriteRenderer[] doorRenderers = new SpriteRenderer[3];
    protected LayerMask excludeSamusAndBullets;
    protected LayerMask excludeNothing;
    
    // Start is called before the first frame update
    void Awake()
    {
        doorCollider = GetComponent<Collider>();
        doorRenderers = GetComponentsInChildren<SpriteRenderer>();
        excludeSamusAndBullets = LayerMask.GetMask("Player", "Bullet");
        excludeNothing = LayerMask.GetMask("Nothing");
        timeOpen = maxTimeOpen;
    }

    protected virtual void Update()
    {
        if (timeOpen >= maxTimeOpen)
        {
            if (!midTransition && open)
                CloseDoor();
        }
        else
            timeOpen += Time.deltaTime;
    }

    public virtual void OnProjectileCollision(GameObject projectile)
    {
        OpenDoor();
    }

    public virtual void OpenDoor()
    {
        open = true;
        timeOpen = 0f;
        doorCollider.excludeLayers = excludeSamusAndBullets;
        foreach (SpriteRenderer doorRenderer in doorRenderers)
        {
            doorRenderer.enabled = false;
        }
        //TODO: replace with animation
    }

    public virtual void CloseDoor()
    {
        open = false;
        doorCollider.excludeLayers = excludeNothing;
        foreach (SpriteRenderer doorRenderer in doorRenderers)
        {
            doorRenderer.enabled = true;
        }
        //TODO: replace w animation
    }

    public virtual void SetTransition(bool isMidTransition)
    {
        midTransition = isMidTransition;
    }
}
