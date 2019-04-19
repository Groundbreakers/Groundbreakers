namespace CombatManager
{
    using System.Collections;

    using Characters;

    using Core;

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

        [InfoBox("Setup the map and let battle begins")]
        [Button]
        public void Setup()
        {
            this.StartCoroutine(this.Begin());
        }

        protected void OnEnable()
        {
            this.tilemap = this.GetComponentInChildren<Tilemap>();
            this.indicators = this.GetComponentInChildren<SpawnIndicators>();
            this.characters = this.GetComponentInChildren<SpawnCharacters>();
            this.navigation = this.GetComponentInChildren<NavigationMap>();
            this.tileEnter = this.GetComponentInChildren<TilemapEnter>();
        }

        private IEnumerator Begin()
        {
            this.tilemap.SetupMap();
            this.tileEnter.Begin();

            yield return new WaitForSeconds(3.0f);

            this.characters.Initialize();

            yield return new WaitForSeconds(3.0f);

            this.indicators.Initialize();

            yield return new WaitForSeconds(0.1f);

            var objs = FindObjectsOfType(typeof(Spanwer));

            foreach (var o in objs)
            {
                ((Spanwer)o).ShouldSpawnWave();
            }
        }
    }
}