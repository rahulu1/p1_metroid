using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZebSpawner : EnemyController
{
    public float aggroDistance;
    public float timeBetweenSpawns;

    // Set to -1 for infinite spawns
    public int maxSpawns;

    public GameObject zebPrefab;

    private Transform playerTransform;
    private GameObject currentSpawnedZeb;

    private int numZebSpawned = 0;
    private bool inSpawnCooldown = false;

    public override EnemyController GetController()
    {
        return this;
    }

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if(ShouldSpawn())
        {
            Spawn();
            StartCoroutine(SpawnCooldown(timeBetweenSpawns));
        }
    }

    private bool ShouldSpawn()
    {
        bool playerWithinAggroDistance =
            Utilities.WithinXDistance(
            playerTransform, this.transform, aggroDistance);

        bool lastSpawnedZebDestroyed = !currentSpawnedZeb;

        bool maxSpawnsExceeded;

        if(maxSpawns == -1)
            maxSpawnsExceeded = false;
        else
            maxSpawnsExceeded = (numZebSpawned >= maxSpawns);

        return playerWithinAggroDistance && 
               lastSpawnedZebDestroyed && 
               !inSpawnCooldown &&
               !maxSpawnsExceeded;
    }

    private void Spawn()
    {
        currentSpawnedZeb = Instantiate(
            zebPrefab, this.transform.position, Quaternion.Euler(0, 0, 0));
        ++numZebSpawned;
    }

    IEnumerator SpawnCooldown(float duration)
    {
        inSpawnCooldown = true;

        yield return new WaitForSeconds(duration);

        inSpawnCooldown = false;
    }
}
