using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [Serializable]
    public class setReqs
    {
        public int set; // Which set this requirement is for
        public int totalLocks; // How many locks to unlock from this set
        public int unlockedSoFar = 0;
    }

    private int totalSets; // total sets of locks
    private int unlockedSets;


    [SerializeField]
    private List<setReqs> lockRequirements = new();

    [SerializeField]
    private Lockable lockedComponent;

    void Start()
    {
        lockedComponent.Lock();
        Chargeable.on_charge += OnCharge;
        Chargeable.on_discharge += OnDischarge;

        totalSets = lockRequirements.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCharge(int fromSet)
    {
        foreach (setReqs requirement in  lockRequirements)
        {
            if (requirement.set == fromSet)
            {
                requirement.unlockedSoFar++;
                if (requirement.unlockedSoFar == requirement.totalLocks)
                {
                    unlockedSets++;
                    if (unlockedSets == totalSets)
                    {
                        lockedComponent.Unlock();
                    }
                }
            }
        }
    }

    void OnDischarge(int fromSet)
    {
        foreach (setReqs requirement in lockRequirements)
        {
            if (requirement.set == fromSet)
                requirement.unlockedSoFar--;
        }
    }
}
