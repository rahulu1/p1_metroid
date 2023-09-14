using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    [SerializeField] private float timeOpen;
    [SerializeField] private float maxTimeOpen = 5f;

    private bool open = false;

    private Collider doorCollider;
    private SpriteRenderer[] doorRenderers = new SpriteRenderer[3];
    private LayerMask excludeSamusAndBullets;
    private LayerMask excludeNothing;
    
    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<Collider>();
        doorRenderers = GetComponentsInChildren<SpriteRenderer>();
        excludeSamusAndBullets = LayerMask.GetMask("Player", "Bullet");
        excludeNothing = LayerMask.GetMask("Nothing");
        timeOpen = maxTimeOpen;
    }

    private void Update()
    {
        if (timeOpen >= maxTimeOpen)
        {
            open = false;
            doorCollider.excludeLayers = excludeNothing;
            foreach (SpriteRenderer doorRenderer in doorRenderers)
            {
                doorRenderer.enabled = true;
            }
            //TODO: replace w animation
        }
        else
            timeOpen += Time.deltaTime;
    }

    public void OnProjectileCollision()
    {
        open = true;
        doorCollider.excludeLayers = excludeSamusAndBullets;
        timeOpen = 0;
        foreach (SpriteRenderer doorRenderer in doorRenderers)
        {
            doorRenderer.enabled = false;
        }
        //TODO: replace with animation
    }

    public void SetOpen(bool openState)
    {
        open = openState;
        if (open)
        {
            doorCollider.excludeLayers = excludeSamusAndBullets;
            timeOpen = 0;
            foreach (SpriteRenderer doorRenderer in doorRenderers)
            {
                doorRenderer.enabled = false;
            }
        }
        else
        {
            doorCollider.excludeLayers = excludeNothing;
            foreach (SpriteRenderer doorRenderer in doorRenderers)
            {
                doorRenderer.enabled = true;
            }
        }
    }
}
