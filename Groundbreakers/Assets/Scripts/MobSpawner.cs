namespace Assets.Scripts
{
    using System.Collections.Generic;
    using Assets.Enemies.Scripts;

    using UnityEngine;

    public class MobSpawner : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private GameObject minion = null;

        [SerializeField]
        private int totalWaves = 5;

        [SerializeField]
        private float waveDuration = 30.0f;

        [SerializeField]
        private float timeBeforeFirstWave = 2.0f;

        #endregion

        #region Internal fields

        private List<Vector3> associatedPath;

        #endregion

        private void OnEnable()
        {
            // Setup event listener
            BattleManager.StartListening("spawn", this.SpawnEnemy);
        }

        private void Start()
        {
            var map = BattleManager.Instance.GetComponent<GameMap>();
            this.associatedPath = map.GetPathA();
        }

        private void Update()
        {
            
        }

        #region Internal Functions

        private void SpawnEnemy()
        {
            var startingPoint = this.transform.position;

            // Todo: Generalize the enemies using enemy groups
            var instance = Instantiate(this.minion, startingPoint, Quaternion.identity);

            // Set enemies path
            instance.GetComponent<Enemy_Generic>().SetWayPoints(this.associatedPath);
        }

        #endregion
    }
}