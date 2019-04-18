namespace Assets.Scripts
{
    using System.Collections;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;

    /// <summary>
    /// The game object class for the timer. 
    /// </summary>
    public class GameTimer : MonoBehaviour
    {
        #region Inspector variables

        [SerializeField]
        private float timeScale = 1.0f;

        [SerializeField]
        private int totalWaves = 5;

        [SerializeField]
        private float waveDuration = 30.0f;

        /// <summary>
        /// The total enter duration for the map. Recommend set the value to (MaxDelay + Duration)
        /// of the TilePrefab's Enter Behavior setting. While the tiles are entering, the player
        /// can not deploy characters on to the tiles.
        /// <see cref="EnterBehavior"/>
        /// </summary>
        [SerializeField]
        private float tileEnterDuration = 4.0f;

        [SerializeField]
        private float timeBeforeFirstWave = 10.0f;

        #endregion

        #region Internal Variables

        private int currentWave;

        #endregion

        #region Public Functions

        /// <summary>
        /// Should be called by Battle manager when battle start.
        /// </summary>
        [Button]
        public void StartLevel()
        {
            this.StartCoroutine(this.StartLevelTimer());
        }

        /// <summary>
        /// Should be called by Battle manager when battle end.
        /// </summary>
        [Button]
        public void ResetTimer()
        {
            this.currentWave = 0;
        }

        #region Unit Callbacks

        private void OnEnable()
        {
            if (Mathf.Abs(this.timeScale - 1.0f) > Mathf.Epsilon)
            {
                Debug.LogWarning("Note the game is currently running at Debugging speed. "
                                 + "If this is not the case, goto GameTimer Component under the "
                                 + "BattleManager and set the value to be 1.0f.");
            }

            Time.timeScale = this.timeScale;
        }

        #endregion

        #endregion

        #region Internal Functions  

        /// <summary>
        /// The main Timer control unit of each level.
        /// The battle flow might look like the following:
        /// "start" -> 4s -> "block ready" -> 6s -> "spawn wave" -> 30s -> ... -> "end"
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator StartLevelTimer()
        {
            BattleManager.TriggerEvent("start");
            Debug.Log("start");

            yield return new WaitForSeconds(this.tileEnterDuration);

            BattleManager.TriggerEvent("block ready");
            Debug.Log("block ready");

            yield return new WaitForSeconds(
                this.timeBeforeFirstWave - this.tileEnterDuration);

            while (this.currentWave < this.totalWaves)
            {
                // Broadcasting Event "spawn wave".
                BattleManager.TriggerEvent("spawn wave");

                this.currentWave++;

                // Wait until next round
                yield return new WaitForSeconds(this.waveDuration);
            }

            // Broadcasting "end" message: battle finished
            BattleManager.TriggerEvent("end");

            yield return new WaitForSeconds(this.tileEnterDuration);

            BattleManager.TriggerEvent("victory");
        }

        #endregion
    }
}