namespace Assets.Scripts
{
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

        private void Start()
        {

        }

        private void Update()
        {
            
        }

        #region Internal Functions

        private void Spawn()
        {

        }

        #endregion
    }
}