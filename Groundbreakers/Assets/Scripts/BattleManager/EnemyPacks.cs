namespace Asset.Script
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class EnemyPacks : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        private GameObject firebatPrefab;

        #endregion

        #region Internal Fields

        // Currenlty, hard code the level to a fixed value, we will change it when level gen is done.
        private List<Enemies>[] packs = new List<Enemies>[5];

        private int currentWave = -1;

        #endregion

        #region PublicProperties

        public enum Enemies
        {
            FireBat
        }

        #endregion

        #region Public Functions

        /// <summary>
        ///     Check if the current wave queue has finished.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> True if the current wave queue is empty.
        /// </returns>
        public bool Done()
        {
            return this.GetCount() == 0;
        }

        public int GetCount()
        {
            return this.packs[this.currentWave].Count;
        }

        /// <summary>
        ///     Get the next enemy from this current group.
        /// </summary>
        /// <returns>
        ///     The <see cref="GameObject" /> a random enemy from this group.
        /// </returns>
        public GameObject GetNextMob()
        {
            const int Head = 0;
            var type = this.packs[this.currentWave][Head];
            this.packs[this.currentWave].RemoveAt(Head);
            return this.GetFromType(type);
        }

        /// <summary>
        /// Switching the pack to the next pack.
        /// </summary>
        /// <param name="waveNumber">
        /// The wave Number.
        /// </param>
        public void SetCurrentWave(int waveNumber)
        {
            waveNumber = Mathf.Clamp(waveNumber, 0, 4);
            this.currentWave = waveNumber;
        }

        /// <summary>
        /// Should be called after every battle.
        /// </summary>
        public void ResetPack()
        {
            this.currentWave = -1;
            this.InitializePacks();
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            // Well, eventually we ready from an XML/JSON File
            this.InitializePacks();
        }

        #endregion

        #region Internal Functions

        [Obsolete("Well, eventually we ready from an XML/JSON File")]
        private void InitializePacks()
        {
            this.packs[0] = new List<Enemies>
                                {
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                };

            this.packs[1] = new List<Enemies>
                                {
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat,
                                };

            this.packs[2] = new List<Enemies>
                                {
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                };

            this.packs[3] = new List<Enemies>
                                {
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat,
                                };

            this.packs[4] = new List<Enemies>
                                {
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                    Enemies.FireBat, Enemies.FireBat, Enemies.FireBat,
                                };
        }

        private GameObject GetFromType(Enemies type)
        {
            switch (type)
            {
                case Enemies.FireBat:
                    return this.firebatPrefab;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}