using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public int enemy_per_wave = 2;

    public Transform enemyPrefab;

    public Transform spawnPoint;

    public float timeBtwWaves = 5f;


    private float timeBeforeFirstWave = 2f;
    
    void SpawnEnemy() {
        Instantiate(this.enemyPrefab, this.spawnPoint.position, this.spawnPoint.rotation);
    }

    IEnumerator SpawnWave() {

            for (int i = 0; i < this.enemy_per_wave; i++)
            {
                this.SpawnEnemy();
                yield return new WaitForSeconds(2f);
                

            }
        
    
    }

    void Update() {
        if (this.timeBeforeFirstWave <= 0f)
        {
            this.StartCoroutine(this.SpawnWave());
            this.timeBeforeFirstWave = this.timeBtwWaves;
        }

        this.timeBeforeFirstWave -= Time.deltaTime;
    }

    
   
    }
