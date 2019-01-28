namespace Assets.Scripts
{
    using System.Collections;
    using System.Collections.Generic;
    using Assets.Enemies.Scripts;

    using UnityEngine;

    [RequireComponent(typeof(EnemyGroup))]
    public class MobSpawner : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private int totalWaves = 5;

        [SerializeField]
        private float waveDuration = 30.0f;

        [SerializeField]
        private float timeBeforeFirstWave = 2.0f;

        #endregion

        #region Internal fields

        private List<Vector3> associatedPath;

        private bool active;

        private int currentWave;

        private EnemyGroup group;

        #endregion

        public void SetAssociativePath(List<Vector3> path)
        {
            this.associatedPath = path;
        }

        #region Unity callbacks

        private void Start()
        {
            this.group = this.GetComponent<EnemyGroup>();
        }

        private void OnEnable()
        {
            // Setup event listener
            // BattleManager.StartListening("spawn", this.InstantiateEnemyAtSpawnPoint);
        }

        private void Update()
        {
            if (BattleManager.GameState != BattleManager.Stages.Combating)
            {
                return;
            }

            this.StartWaves();
        }

        #endregion

        #region Internal Functions

        private void StartWaves()
        {
            if (!this.active)
            {
                this.active = true;
                this.StartCoroutine(this.SpawnWave());
            }
        }

        /// <summary>
        /// The main function that spawns the 5 waves
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator SpawnWave()
        {
            yield return new WaitForSeconds(this.timeBeforeFirstWave);

            while (this.currentWave < this.totalWaves)
            {
                while (!this.group.Done())
                {
                    this.InstantiateEnemyAtSpawnPoint(this.group.GetNext());

                    yield return new WaitForSeconds(1.0f);
                }


                this.currentWave++;
                BattleManager.Instance.OnWaveUpdate(this.currentWave);

                // Wait until next round
                yield return new WaitForSeconds(this.waveDuration);
            }

            // Now we have finished this level
        }

        private void InstantiateEnemyAtSpawnPoint(GameObject minion)
        {
            var startingPoint = this.transform.position;

            var instance = Instantiate(minion, startingPoint, Quaternion.identity);

            // Set enemies path
            instance.GetComponent<Enemy_Generic>().SetWayPoints(this.associatedPath);
            instance.transform.SetParent(this.transform.parent);
        }

        #endregion
    }
}