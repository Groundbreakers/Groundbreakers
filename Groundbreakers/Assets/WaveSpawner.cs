using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public float  timeBtwWaves= 5f;
    private float timeBeforeFirstWave = 2f;
    public int enemy_per_wave = 2;
    

    void Update()
    {
        if (timeBeforeFirstWave <= 0f) {
            StartCoroutine(SpawnWave());
            timeBeforeFirstWave = timeBtwWaves;

        }

        timeBeforeFirstWave -= Time.deltaTime;
    }

    IEnumerator SpawnWave() {
        for (int i = 0; i < enemy_per_wave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy() {
       Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
