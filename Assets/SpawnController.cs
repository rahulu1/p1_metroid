using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject entityPrefab; // prefab of the enemy spawned by the anchor
    private GameObject spawnedEntity; // used to track or destroy spawned entity
    [SerializeField]
    private GameObject healthOrbPrefab; // used to spawn health orb
    [SerializeField]
    private GameObject missileAmmoPrefab; // used to spawn missile ammo
    private PlayerInventory playerInventory;

    private Transform camTransform; // used to determine camera position
    private CameraController mainCamController; // used to check room orientation

    [SerializeField]
    private float initialSpawnChance; // Chance they spawn before dying
    [SerializeField]
    private float respawnChance; // Chance they respawn after dying
    private float currentSpawnChance; // Set this to respawnChance after the first death
    [SerializeField]
    private float healthDropChance; // Chance an entity drops health orbs after dying
    [SerializeField]
    private float missileDropChance; // Chance an entity drops Missile Ammo after dying
    [SerializeField]
    private float maxCamOffsetX = 8.5f;
    [SerializeField]
    private float maxCamOffsetY = 8f;

    private bool frozen = false;

    void Awake()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();   
        camTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        mainCamController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        currentSpawnChance = initialSpawnChance;

        // this ensures enemies in room A spawn at the start
        OnEnable();
    }

    void OnEnable()
    {
        if (spawnedEntity == null)
        {
            AttemptToSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the entity is spawned and alive
        if(spawnedEntity != null)
        {
            float entityCamOffset;

            if (mainCamController.IsCurrRoomHorizontal())
            {
                entityCamOffset = Mathf.Abs(spawnedEntity.transform.position.x - camTransform.position.x);

                // if offscreen, freeze entity
                if (entityCamOffset > maxCamOffsetX)
                {
                    frozen = true;
                    spawnedEntity.SetActive(false);
                }
                // if onscreen and currently frozen, unfreeze
                else if (frozen)
                {
                    frozen = false;
                    spawnedEntity.SetActive(true);
                }
            }
            else
            {
                entityCamOffset = Mathf.Abs(spawnedEntity.transform.position.y - camTransform.position.y);

                // if offscreen, freeze entity
                if (entityCamOffset > maxCamOffsetY)
                {
                    frozen = true;
                    spawnedEntity.SetActive(false);
                }
                // if onscreen and currently frozen, unfreeze
                else if (frozen)
                {
                    frozen = false;
                    spawnedEntity.SetActive(true);
                }
            }
        }
    }

    void OnBecameVisible()
    {
        // if the spawned entity is dead when the anchor becomes visible
        if(spawnedEntity == null)
        {
            AttemptToSpawn();
        }    
    }

    // use this in DamageReactEnemy instead of the Destroy function
    public void KillEntity()
    {
        if (currentSpawnChance != respawnChance)
            currentSpawnChance = respawnChance;


        Vector3 deathLocation = spawnedEntity.transform.position;

        Destroy(spawnedEntity);

        float spawnOrNot = Random.Range(0f, 1f);

        if (playerInventory.MissilesUnlocked())
        {
            if (spawnOrNot <= healthDropChance)
            {
                spawnedEntity = Instantiate(healthOrbPrefab, this.transform);
                spawnedEntity.transform.position = deathLocation;
            }
            else if (spawnOrNot <= missileDropChance)
            {
                spawnedEntity = Instantiate(missileAmmoPrefab, this.transform);
                spawnedEntity.transform.position = deathLocation;
            }
        }
        else
        {
            if (spawnOrNot <= (healthDropChance + missileDropChance))
            {
                spawnedEntity = Instantiate(healthOrbPrefab, this.transform);
                spawnedEntity.transform.position = deathLocation;
            }
        }
    }

    void AttemptToSpawn()
    {
        float spawnOrNot = Random.Range(0f, 1f);

        // Roll the dice. If it is within threshold, spawn
        if (spawnOrNot <= currentSpawnChance)
        {
            spawnedEntity = Instantiate(entityPrefab, this.transform);

            // spawn it at the spawn anchor
            spawnedEntity.transform.position = transform.position;
            Debug.Log(spawnedEntity.transform.position);
        }
    }
}
