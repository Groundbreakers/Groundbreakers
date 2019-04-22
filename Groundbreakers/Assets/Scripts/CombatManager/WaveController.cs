namespace CombatManager
{
    using System.Collections;

    using Sirenix.OdinInspector;

    using UnityEngine;

    public class WaveController
    {
        [SerializeField]
        private int totalWaves = 5;

        [SerializeField]
        private float waveDuration = 30.0f;

        [SerializeField]
        private float timeBeforeFirstWave = 3.0f;

        private int currentWave;

        [Button]
        public void StartLevel()
        {
            // this.StartCoroutine(this.StartLevelTimer());
        }

        public void ResetTimer()
        {
            this.currentWave = 0;
        }

        private IEnumerator StartLevelTimer()
        {
            Debug.Log("start");

            yield return new WaitForSeconds(3.0f);

            Debug.Log("block ready");

            yield return new WaitForSeconds(this.timeBeforeFirstWave);

            while (this.currentWave < this.totalWaves)
            {
                this.currentWave++;

                // Wait until next round
                yield return new WaitForSeconds(this.waveDuration);
            }

            yield return new WaitForSeconds(3.0f);
        }
    }
}