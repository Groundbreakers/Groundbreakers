//namespace Assets.Scripts
//{
//    using System.Collections;
//    using System.Collections.Generic;

//    using Assets.Enemies.Scripts;
//    using Assets.Scripts.Enemies;

//    using UnityEngine;

//    public class WaveSpawner : MonoBehaviour
//    {
//        // Packs for paths 1 and 2. Stored as an array of Lists of prefabs.
//        private List<Transform>[] pack1 = new List<Transform>[5];

//        private List<Transform>[] pack2 = new List<Transform>[5];

//        // Waypoint lists
//        private List<Vector3> path1 = new List<Vector3>();

//        private List<Vector3> path2 = new List<Vector3>();

//        private Transform spawnPoint1;

//        private Transform spawnPoint2;

//        private float timeBeforeFirstWave = 2f;

//        private float timeBtwWaves = 30f;

//        private int WaveNum;

//        // Adds a waypoint to a path. Presumably called by the terrain generator.
//        public void addPoint(Transform point, int path)
//        {
//            if (path == 1)
//            {
//                this.path1.Add(point.position);
//            }
//            else
//            {
//                this.path2.Add(point.position);
//            }
//        }

//        // Gets the packs for a level. Use lowerRange and upperRange to specify which packs to use.
//        public void GetNewPacks(int lowerRange, int upperRange)
//        {
//            int diceroll1, diceroll2;
//            for (var i = 0; i < 5; i++)
//            {
//                diceroll1 = Random.Range(lowerRange, upperRange + 1);
//                this.pack1[i] = new List<Transform>();
//                this.pack1[i].AddRange(this.GetComponent<PackLists>().Packs[diceroll1]);

//                diceroll2 = Random.Range(lowerRange, upperRange + 1);
//                this.pack2[i] = new List<Transform>();
//                this.pack2[i].AddRange(this.GetComponent<PackLists>().Packs[diceroll2]);
//            }
//        }

//        // Spawns a wave at each lane, then increments the wave number.
//        public IEnumerator SpawnWave(int num)
//        {
//            Transform thisEnemy;
//            for (var i = 0; i < this.pack1[num].Count || i < this.pack2[num].Count; i++)
//            {
//                if (i < this.pack1[num].Count)
//                {
//                    thisEnemy = Instantiate(this.pack1[num][i], this.spawnPoint1.position, this.spawnPoint1.rotation);
//                    thisEnemy.GetComponent<EnemyGeneric>().SetWayPoints(this.path1);
//                }

//                if (i < this.pack2[num].Count)
//                {
//                    thisEnemy = Instantiate(this.pack2[num][i], this.spawnPoint2.position, this.spawnPoint2.rotation);
//                    thisEnemy.GetComponent<EnemyGeneric>().SetWayPoints(this.path2);
//                }

//                yield return new WaitForSeconds(1f);
//            }

//            this.WaveNum++;
//        }

//        private void Start()
//        {
//            this.GetNewPacks(0, 1);
//        }

//        // Updates timers, spawning a wave if 30 seconds are up.
//        private void Update()
//        {
//            if (this.timeBeforeFirstWave <= 0f && this.WaveNum < 5)
//            {
//                this.StartCoroutine(this.SpawnWave(this.WaveNum));
//                this.timeBeforeFirstWave = this.timeBtwWaves;
//            }

//            this.timeBeforeFirstWave -= Time.deltaTime;
//        }
//    }
//}