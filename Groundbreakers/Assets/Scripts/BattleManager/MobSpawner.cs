namespace Assets.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using Asset.Script;

    using Assets.Enemies.Scripts;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// This version of the spawn evenly distribute the enemies packs within the 25s period.
    /// It also provide some utility functions that enhance the performance and readability.
    /// </summary>
    [RequireComponent(typeof(EnemyPacks))]
    public class MobSpawner : MonoBehaviour
    {
        #region Inspector Field

        [SerializeField]
        [Range(10.0f, 25.0f)]
        private float duration = 25.0f;

        #endregion

        #region Internal Fields

        private EnemyPacks pack;

        private List<Vector3> pathA = new List<Vector3>();

        private List<Vector3> pathB = new List<Vector3>();

        private Vector3 spawnPointA;

        private Vector3 spawnPointB;

        #endregion

        #region Public Functions

        /// <summary>
        /// Heritage from Austin. 
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public void AddPoint(Vector3 point, int path)
        {
            if (path == 1)
            {
                this.pathA.Add(point);
            }
            else
            {
                this.pathB.Add(point);
            }
        }

        /// <summary>
        /// Currently this function is called by the Timer UI (???????). Which would leave us some
        /// side effects. But I am keeping this for now. 
        /// </summary>
        /// <param name="waveNumber">
        /// The wave number.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator SpawnWave(int waveNumber)
        {
            this.pack.SetCurrentWave(waveNumber);

            var count = this.pack.GetCount();
            var delta = this.duration / count * 2.0f;
            Debug.Log("total " + count + " enemies in this wave. We should emit at rate " + delta);

            while (!this.pack.Done())
            {
                this.InstantiateEnemyAtSpawnPoint(this.pack.GetNextMob(), 1);

                if (!this.pack.Done())
                {
                    this.InstantiateEnemyAtSpawnPoint(this.pack.GetNextMob(), 2);
                }

                yield return new WaitForSeconds(delta);
            }
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.pack = this.GetComponent<EnemyPacks>();
            Time.timeScale = 10;
        }

        #endregion

        #region Internal Functions

        private void InstantiateEnemyAtSpawnPoint(GameObject minion, int pathId)
        {
            var path = pathId == 1 ? this.pathA : this.pathB;

            var startingPoint = path[0];
            var instance = Instantiate(minion, startingPoint, Quaternion.identity);

            // Set enemies path, ~Heritage from Austin
            instance.GetComponent<Enemy_Generic>().waypointList = path;
            instance.transform.SetParent(this.transform.parent);
        }

        #endregion
    }
}