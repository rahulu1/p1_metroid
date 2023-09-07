using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject standing;
    public GameObject morphed;

    private Collider col;
    private bool isStanding;
    private PlayerInventory playerInventory;

    private void Start()
    {
        isStanding = true;
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded())
        {
            if (isStanding && Input.GetKeyDown(KeyCode.DownArrow))
            {
                standing.SetActive(false);
                morphed.SetActive(true);
                isStanding = false;
            }
            if (!isStanding && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                if (hasSpace() && playerInventory.HasMorphBall())
                {
                    standing.SetActive(true);
                    morphed.SetActive(false);
                    isStanding = true;
                }
            }
        }
    }

    public bool isGrounded()
    {
        col = this.GetComponentInChildren<Collider>();

        Ray ray = new Ray(col.bounds.center, Vector3.down);

        float radius = col.bounds.extents.x - .05f;

        float fullDistance = col.bounds.extents.y + .05f;

        if (Physics.SphereCast(ray, radius, fullDistance))
            return true;
        else
            return false;
    }

    bool hasSpace()
    {
        col = this.GetComponentInChildren<Collider>();

        Ray ray = new Ray(col.bounds.center, Vector3.up);

        float radius = col.bounds.extents.x - .05f;

        float fullDistance = col.bounds.extents.y + .05f;

        if (Physics.SphereCast(ray, radius, fullDistance))
            return false;
        else
            return true;
    }
}
