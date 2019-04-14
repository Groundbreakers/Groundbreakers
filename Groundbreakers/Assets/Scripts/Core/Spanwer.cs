namespace Core
{
    using System.Collections;

    using Assets.Enemies.Scripts;
    using Assets.Scripts;

    using CombatManager;

    using Sirenix.OdinInspector;

    using UnityEngine;

    public class Spanwer : MonoBehaviour
    {
        [SerializeField]
        private GameObject debugMinion;

        [SerializeField]
        [Range(10.0f, 25.0f)]
        private float duration = 25.0f;

        private EnemyGroups pack;

        [SerializeField]
        private bool A; //temp

        [Button]
        public void ShouldSpawnWave()
        {
            if (!this.PathExists())
            {
                return;
            }

            this.pack.ResetPack();
            this.StopAllCoroutines();
            this.StartCoroutine(this.SpawnWave());
        }

        public IEnumerator SpawnWave(int pathId = 1)
        {
            // var count = this.pack.GetCount(pathId);
            var count = 15;
            var delta = this.duration / count;
            Debug.Log("total " + count + " enemies in this wave. We should emit at rate " + (int)delta);

            while (!this.pack.Done(pathId))
            {
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
            var path = GameObject.Find("Battle Field").GetComponent<SetupBattleField>();
            instance.GetComponent<Enemy_Generic>().waypointList = this.A ? path.pathA : path.pathB;
            instance.transform.SetParent(this.transform);
        }

        private void OnEnable()
        {
            var db = GameObject.Find("Enemy Groups");
            this.pack = db.GetComponent<EnemyGroups>();
        }

        private bool PathExists()
        {
            return true;
        }
    }
}