using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Enemies.Scripts;

using UnityEngine;

using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public Vector3 spawnPoint1;
    public Vector3 spawnPoint2;

    // Waypoint lists
    public List<Vector3> path1 = new List<Vector3>();
    public List<Vector3> path2 = new List<Vector3>();

    // Packs for paths 1 and 2. Stored as an array of Lists of prefabs.
    public List<Transform>[] pack1 = new List<Transform>[1]; //[5], modified for development convenience- javy
    public List<Transform>[] pack2 = new List<Transform>[0];

    void Start()
    {
        GetNewPacks(0,1);
    }

    // Spawns a wave at each lane, then increments the wave number.
    public IEnumerator SpawnWave(int num)
    {
        Transform thisEnemy;
        for (int i = 0; i < this.pack1[num].Count || i < this.pack2[num].Count; i++)
        {
            if (i < this.pack1[num].Count)
            {
                thisEnemy = Instantiate(this.pack1[num][i], this.spawnPoint1, Quaternion.identity);
                thisEnemy.GetComponent<Enemy_Generic>().waypointList = this.path1;
            }

            if (i < this.pack2[num].Count)
            {
                thisEnemy = Instantiate(this.pack2[num][i], this.spawnPoint2, Quaternion.identity);
                thisEnemy.GetComponent<Enemy_Generic>().waypointList = this.path2;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // Adds a waypoint to a path. Presumably called by the terrain generator.
    public void AddPoint(Vector3 point, int path)
    {
        if (path == 1)
        {
            this.path1.Add(point);
        }
        else
        {
            this.path2.Add(point);
        }
    }

    // Gets the packs for a level. Use lowerRange and upperRange to specify which packs to use.
    public void GetNewPacks(int lowerRange, int upperRange)
    {
        int diceroll1, diceroll2;
        for (int i = 0; i < 5; i++)
        {
            diceroll1 = Random.Range(lowerRange, upperRange + 1);
            this.pack1[i] = new List<Transform>();
            this.pack1[i].AddRange(this.GetComponent<PackLists>().Packs[diceroll1]);

            diceroll2 = Random.Range(lowerRange, upperRange + 1);
            this.pack2[i] = new List<Transform>();
            this.pack2[i].AddRange(this.GetComponent<PackLists>().Packs[diceroll2]);
        }
    }
}