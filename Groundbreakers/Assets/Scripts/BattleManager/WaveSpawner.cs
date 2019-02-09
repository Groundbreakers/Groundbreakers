using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Enemies.Scripts;

using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(PackLists))]
public class WaveSpawner : MonoBehaviour
{
    #region Internel Fields

    // Packs for paths 1 and 2. Stored as an array of Lists of prefabs.
    private List<Transform>[] pack1 = new List<Transform>[5];

    private List<Transform>[] pack2 = new List<Transform>[5];

    // Waypoint lists
    private List<Vector3> path1 = new List<Vector3>();

    private List<Vector3> path2 = new List<Vector3>();

    public Vector3 spawnPoint1;

    public Vector3 spawnPoint2;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        this.GetNewPacks(0, 1);
    }

    #endregion

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

    public void GetNewPacks(int lowerRange, int upperRange)
    {
        int diceroll1, diceroll2;
        for (var i = 0; i < 5; i++)
        {
            diceroll1 = Random.Range(lowerRange, upperRange + 1);
            this.pack1[i] = new List<Transform>();
            this.pack1[i].AddRange(this.GetComponent<PackLists>().Packs[diceroll1]);

            diceroll2 = Random.Range(lowerRange, upperRange + 1);
            this.pack2[i] = new List<Transform>();
            this.pack2[i].AddRange(this.GetComponent<PackLists>().Packs[diceroll2]);
        }
    }

    /// <summary>
    /// The main function that spawns the 5 waves
    /// </summary>
    /// <param name="waveNumber">
    /// The num.
    /// </param>
    /// <returns>
    /// The <see cref="IEnumerator"/>.
    /// </returns>
    public IEnumerator SpawnWave(int waveNumber)
    {
        Transform thisEnemy;
        for (var i = 0; i < this.pack1[waveNumber].Count || i < this.pack2[waveNumber].Count; i++)
        {
            if (i < this.pack1[waveNumber].Count)
            {
                thisEnemy = Instantiate(this.pack1[waveNumber][i], this.spawnPoint1, Quaternion.identity);
                thisEnemy.GetComponent<Enemy_Generic>().waypointList = this.path1;

            }

            if (i < this.pack2[waveNumber].Count)
            {
                thisEnemy = Instantiate(this.pack2[waveNumber][i], this.spawnPoint2, Quaternion.identity);
                thisEnemy.GetComponent<Enemy_Generic>().waypointList = this.path2;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void InstantiateEnemyAtSpawnPoint(Transform minion, int pathId)
    {
        var startingPoint = this.transform.position;

        var instance = Instantiate(
            minion,
            startingPoint,
            Quaternion.identity);

        // Set enemies path
        instance.GetComponent<Enemy_Generic>().waypointList = pathId == 0 ? this.path1 : this.path2;
        instance.transform.SetParent(this.transform.parent);
    }
}