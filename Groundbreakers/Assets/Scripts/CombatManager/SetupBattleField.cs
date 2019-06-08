namespace CombatManager
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Characters;

    using Core;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.UI;

    /// <inheritdoc />
    /// <summary>
    ///     One time script. Setup the battle field by calling the setup functions of children in
    ///     the right order.
    /// </summary>
    public class SetupBattleField : MonoBehaviour
    {
        public static WaveInformation CurrentWaveInformation;

        private SpawnIndicators indicators;

        private PartyManager party;

        private Tilemap tilemap;

        private TilemapEnter tileEnter;

        private List<Spanwer> spanwers;

        [InfoBox("Setup the map and let battle begins.")]
        [Button]
        public void Setup(int depth, int risk)
        {
            this.StartCoroutine(this.Begin());
        }

        [InfoBox("End the battle when necessary.")]
        [Button]
        public void EndBattle()
        {
            this.StartCoroutine(this.Terminate());
        }

        protected void OnEnable()
        {
            DOTween.Init(true, true);

            this.tilemap = this.GetComponentInChildren<Tilemap>();
            this.indicators = this.GetComponentInChildren<SpawnIndicators>();
            this.party = this.GetComponentInChildren<PartyManager>();
            this.tileEnter = this.GetComponentInChildren<TilemapEnter>();

            this.spanwers = SetupSpawner();
        }

        protected void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                this.Setup(50, 1);
            }
        }

        private static List<Spanwer> SetupSpawner()
        {
            return FindObjectsOfType(typeof(Spanwer)).Select(o => (Spanwer)o).ToList();
        }

        private IEnumerator Begin()
        {
            // Keep generating map until has valid path.
            do
            {
                this.tilemap.SetupMap();
                this.indicators.Initialize();
            }
            while (!this.indicators.HasValidPath());

            yield return new WaitForSeconds(0.1f);

            // Initialize Environment
            this.tilemap.PostSetupMap();

            yield return new WaitForSeconds(0.1f);

            this.tileEnter.Begin();

            yield return new WaitForSeconds(3.0f);

            this.indicators.RevealIndicators();

            FindObjectOfType<TileController>().Activate();

            yield return new WaitForSeconds(0.1f);

            this.StartCoroutine(this.StartTimer());
        }

        private IEnumerator Terminate()
        {
            this.KillAllEnemies();
            this.KillAllMisc();

            this.party.RetrieveAllCharacters();

            yield return new WaitForSeconds(0.01f);

            this.indicators.HideIndicators();

            this.tileEnter.Terminate();

            yield return new WaitForSeconds(3.0f);
        }

        private void KillAllEnemies()
        {
            foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(e);
            }
        }

        private void KillAllMisc()
        {
            foreach (var e in GameObject.FindGameObjectsWithTag("Loot"))
            {
                Destroy(e);
            }
        }

        private IEnumerator StartTimer()
        {
            const int Waves = 5;
            const int Duration = 30;

            for (var i = 0; i < Waves; i++)
            {
                this.SpawnWaves();

                for (var j = 0; j < Duration; j++)
                {
                    // Debug.Log($"{CurrentWaveInformation.WaveNumber}-{CurrentWaveInformation.Time}");
                    this.UpdateUiTimer();

                    CurrentWaveInformation.WaveNumber = i;
                    CurrentWaveInformation.Time = j;

                    yield return new WaitForSeconds(1.0f);
                }
            }

            this.EndBattle();
        }

        private void SpawnWaves()
        {
            foreach (var spanwer in this.spanwers)
            {
                spanwer.ShouldSpawnWave();
            }
        }

        private void UpdateUiTimer()
        {
            // shitty
            var ui = GameObject.Find("NextWave");

            ui.transform.GetChild(0).GetComponent<Text>().text = $"Wave {CurrentWaveInformation.WaveNumber + 1}";
            ui.transform.GetChild(1).GetComponent<Text>().text = $"{30 - CurrentWaveInformation.Time}";
        }

        public struct WaveInformation
        {
            public int WaveNumber;

            public float Time;
        }
    }
}