//namespace Assets.Scripts
//{
//    using System.Collections;
//    using System.Collections.Generic;

//    using Assets.Enemies.Scripts;

//    using Sirenix.OdinInspector;

//    using UnityEngine;

//    /// <inheritdoc />
//    /// <summary>
//    /// This version of the spawn evenly distribute the enemies packs within the 25s period.
//    /// It also provide some utility functions that enhance the performance and readability.
//    /// </summary>
//    [RequireComponent(typeof(EnemyGroups))]
//    public class MobSpawner : MonoBehaviour
//    {
//        #region Inspector Field

//        [SerializeField]
//        [Range(10.0f, 25.0f)]
//        private float duration = 25.0f;

//        [SerializeField]
//        [ReadOnly]
//        private Sprite[] spawnIndicators;

//        #endregion

//        #region Internal Fields

//        private EnemyGroups pack;

//        private List<Vector3> pathA = new List<Vector3>();

//        private List<Vector3> pathB = new List<Vector3>();

//        private GameObject[] activeIndicators;

//        #endregion

//        #region Public Functions

//        /// <summary>
//        /// Clear both the paths. Should be called before AddingPoints. 
//        /// </summary>
//        public void ClearPoints()
//        {
//            this.pathA.Clear();
//            this.pathB.Clear();
//        }

//        /// <summary>
//        /// Heritage from Austin. 
//        /// </summary>
//        /// <param name="point">
//        /// The point.
//        /// </param>
//        /// <param name="path">
//        /// The path.
//        /// </param>
//        public void AddPoint(Vector3 point, int path)
//        {
//            if (path == 1)
//            {
//                this.pathA.Add(point);
//            }
//            else
//            {
//                this.pathB.Add(point);
//            }
//        }

//        public IEnumerator SpawnWave(int pathId = 1)
//        {
//            var count = this.pack.GetCount(pathId);
//            var delta = this.duration / count;
//            Debug.Log("total " + count + " enemies in this wave. We should emit at rate " + (int)delta);

//            while (!this.pack.Done(pathId))
//            {
//                this.InstantiateEnemyAtSpawnPoint(this.pack.GetNextMob(pathId), pathId);

//                yield return new WaitForSeconds(delta);
//            }
//        }

//        #endregion

//        #region Unity Callbacks

//        private void OnEnable()
//        {
//            this.pack = this.GetComponent<EnemyGroups>();

//            BattleManager.StartListening("block ready", this.CreateIndicators);
//            BattleManager.StartListening("spawn wave", this.ShouldSpawnWave);
//            BattleManager.StartListening(
//                "end",
//                () =>
//                    {
//                        this.StopAllCoroutines();
//                        this.pack.ResetPack();
//                        this.ClearPoints();
//                        this.ClearIndicators();
//                    });
//        }

//        #endregion

//        #region Internal Functions

//        /// <summary>
//        /// The has path.
//        /// </summary>
//        /// <returns>
//        /// The <see cref="bool"/>.
//        /// </returns>
//        private bool PathExists()
//        {
//            return this.pathA.Count != 0 && this.pathB.Count != 0;
//        }

//        private void CreateIndicators()
//        {
//            if (!this.PathExists())
//            {
//                return;
//            }

//            this.activeIndicators = new[] { new GameObject(), new GameObject(), new GameObject(), new GameObject() };

//            var map = GameObject.Find("TileMap");
//            foreach (var go in this.activeIndicators)
//            {
//                go.transform.SetParent(map.transform);
//            }

//            this.activeIndicators[0].transform.SetPositionAndRotation(
//                this.pathA[0], Quaternion.identity);
//            this.activeIndicators[1].transform.SetPositionAndRotation(
//                this.pathA[this.pathA.Count - 1], Quaternion.identity);
//            this.activeIndicators[2].transform.SetPositionAndRotation(
//                this.pathB[0], Quaternion.identity);
//            this.activeIndicators[3].transform.SetPositionAndRotation(
//                this.pathB[this.pathB.Count - 1], Quaternion.identity);

//            for (int i = 0; i < 4; i++)
//            {
//                var renderer = this.activeIndicators[i].AddComponent<SpriteRenderer>();
//                renderer.sprite = this.spawnIndicators[i];
//                renderer.sortingLayerName = "Mobs";
//            }
//        }

//        private void ClearIndicators()
//        {
//            var gameObjects = this.activeIndicators;

//            if (gameObjects == null)
//            {
//                return;
//            }

//            foreach (var go in gameObjects)
//            {
//                GameObject.Destroy(go);
//            }
//        }

//        private void InstantiateEnemyAtSpawnPoint(GameObject minion, int pathId)
//        {
//            var path = pathId == 1 ? this.pathA : this.pathB;

//            var startingPoint = path[0];
//            var instance = Instantiate(minion, startingPoint, Quaternion.identity);

//            // Set enemies path, ~Heritage from Austin
//            //instance.GetComponent<Enemy_Generic>().waypointList = path;
//            instance.transform.SetParent(this.transform);
//        }

//        private void ShouldSpawnWave()
//        {
//            if (!this.PathExists())
//            {
//                return;
//            }

//            this.pack.ResetPack();
//            this.StopAllCoroutines();
//            this.StartCoroutine(this.SpawnWave(1));
//            this.StartCoroutine(this.SpawnWave(2));
//        }

//        #endregion
//    }
//}