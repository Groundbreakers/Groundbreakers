namespace CombatManager
{
    using Characters;

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

        private void OnEnable()
        {
            this.tilemap = this.GetComponentInChildren<Tilemap>();
            this.indicators = this.GetComponentInChildren<SpawnIndicators>();
            this.characters = this.GetComponentInChildren<SpawnCharacters>();
            this.navigation = this.GetComponentInChildren<NavigationMap>();

            this.tilemap.SetupMap();
            this.indicators.Initialize();
            this.characters.Initialize();
        }
    }
}