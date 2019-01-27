using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackLists : MonoBehaviour
{
    // References to all enemy prefabs go here
    public Transform FireBatPrefab;

    // List of enemy packs. Stored as an array of Lists of prefabs. CHANGE THIS NUMBER WHEN YOU ADD PACKS
    public List<Transform>[] Packs = new List<Transform>[2];

    void Awake()
    {
        this.PackInitialization();
    }

    // Add all packs here using "new List<Transform>(new Transform[]{ENEMY PREFABS});
    void PackInitialization()
    {
        this.Packs[0] = new List<Transform>(new Transform[]{ this.FireBatPrefab, this.FireBatPrefab, this.FireBatPrefab, this.FireBatPrefab, this.FireBatPrefab });
        this.Packs[1] = new List<Transform>(new Transform[]{ this.FireBatPrefab, this.FireBatPrefab, this.FireBatPrefab });
    }
}
