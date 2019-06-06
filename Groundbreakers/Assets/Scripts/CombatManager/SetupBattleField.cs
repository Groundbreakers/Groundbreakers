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

    /// <inheritdoc />
    /// <summary>
    ///     One time script. Setup the battle field by calling the setup functions of children in
    ///     the right order.
    /// </summary>
    public class SetupBattleField : MonoBehaviour
    {
        private SpawnCharacters characters;

        private SpawnIndicators indicators;

        private Tilemap tilemap;

        private NavigationMap navigation;

        private TilemapEnter tileEnter;

        private List<Spanwer> spanwers;

        [InfoBox("Setup the map and let battle begins.")]
        [Button]
        public void Setup()
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
            DOTween.Init();

            this.tilemap = this.GetComponentInChildren<Tilemap>();
            this.indicators = this.GetComponentInChildren<SpawnIndicators>();
            this.characters = this.GetComponentInChildren<SpawnCharacters>();
            this.navigation = this.GetComponentInChildren<NavigationMap>();
            this.tileEnter = this.GetComponentInChildren<TilemapEnter>();

            this.spanwers = SetupSpawner();
        }

        protected void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                this.Setup();
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
                this.indicators.RevealIndicators();
            }
            while (!this.indicators.HasValidPath());

            yield return new WaitForSeconds(0.1f);
            
            // Initialize Environment
            this.tilemap.PostSetupMap();

            yield return new WaitForSeconds(0.1f);

            this.tileEnter.Begin();

            yield return new WaitForSeconds(3.0f);

            // this.characters.Initialize();

            FindObjectOfType<TileController>().Activate();

            yield return new WaitForSeconds(0.1f);

            foreach (var spawn in this.spanwers)
            {
                spawn.ShouldStartLevel();
            }
        }

        private IEnumerator Terminate()
        {
            this.KillAllEnemies();

            this.tileEnter.Terminate();
            this.characters.RetrieveAllCharacters();

            yield return new WaitForSeconds(3.0f);
        }

        private void KillAllEnemies()
        {
            foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                GameObject.Destroy(e);
            }
        }
    }
}