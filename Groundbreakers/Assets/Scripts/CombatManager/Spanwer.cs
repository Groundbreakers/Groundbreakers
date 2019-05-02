namespace CombatManager
{
    using System.Collections;

    using Assets.Scripts;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;

    public class Spanwer : MonoBehaviour
    {
        [SerializeField]
        private GameObject debugMinion;

        [SerializeField]
        [Range(10.0f, 25.0f)]
        private float duration = 25.0f;

        private EnemyGroups pack;

        /// <summary>
        ///     Gets a value indicating whether this spawn point is running.
        /// </summary>
        public static bool Busy { get; private set; }

        [Button]
        public void ShouldStartLevel()
        {
            Busy = true;

            this.pack.ResetPack();
            this.StopAllCoroutines();

            this.StartCoroutine(this.StartLevel());
        }

        public void ShouldSpawnWave()
        {
            Busy = true;

            this.pack.ResetPack();
            this.StopAllCoroutines();
            this.StartCoroutine(this.SpawnWave());
        }

        protected void OnEnable()
        {
            var db = GameObject.Find("Enemy Groups");
            this.pack = db.GetComponent<EnemyGroups>();
        }

        private IEnumerator StartLevel()
        {
            const int Waves = 5;

            for (var i = 0; i < Waves; i++)
            {
                this.StartCoroutine(this.SpawnWave());

                yield return new WaitForSeconds(this.duration);

                yield return new WaitForSeconds(5.0f);
            }
        }

        private IEnumerator SpawnWave(int pathId = 1)
        {
            // var count = this.pack.GetCount(pathId);
            var count = 15;
            var delta = this.duration / count;
            Debug.Log("total " + count + " enemies in this wave. We should emit at rate " + (int)delta);

            // !this.pack.Done(pathId)
            while (count --> 0)
            {
                // TODO: FIX this TMP solution
                while (TileController.Busy) 
                {
                    yield return null;
                }

                // this.InstantiateEnemyAtSpawnPoint(this.pack.GetNextMob(pathId));
                this.InstantiateEnemyAtSpawnPoint(this.debugMinion);

                yield return new WaitForSeconds(delta);
            }
        }

        private void InstantiateEnemyAtSpawnPoint(GameObject minion)
        {
            var startingPoint = this.transform.position;
            var instance = Instantiate(minion, startingPoint, Quaternion.identity);

            // TODO: FIX THIS
            instance.transform.SetParent(this.transform);
        }
    }
}