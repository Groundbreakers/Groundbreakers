namespace TileMaps
{
    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <summary>
    ///     This component provides API for controlling dynamic terrain changes. 
    /// </summary>
    public class DynamicTerrainController : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private GameObject highGroundPrefab;

        [Required]
        [SerializeField]
        private GameObject mushroomPrefab;

        [Required]
        [SerializeField]
        private GameObject grassPrefab;

        private Tilemap tilemap;

        private TerrainGenerator generator;

        /// <summary>
        ///     Re-roll the middle 6 layer of the map.
        /// </summary>
        [Button]
        public void RerollMap()
        {
            // Should generate somewhat a new map
            this.generator.Initialize();

            // Ignore top and bottom row
            for (var i = 0; i < Tilemap.Dimension; i++)
            {
                for (var j = 1; j < Tilemap.Dimension - 1; j++)
                {
                    var newType = this.generator.GetTileTypeAt(i, j);

                    this.UpdateTileIfNecessary(new Vector3(i, j), newType);
                }
            }

            foreach (var pos in this.generator.GetMushroomLocations())
            {
                if (pos.y < 1 && pos.y > Tilemap.Dimension - 1)
                {
                    return;
                }

                var block = this.tilemap.GetTileBlockAt(pos);

                GameObject.Instantiate(this.mushroomPrefab, block.transform);
            }
        }

        private void UpdateTileIfNecessary(Vector3 pos, Tiles newType)
        {
            var currentStatus = this.tilemap.GetTileStatusAt(pos);

            // Skip water tiles
            if (currentStatus.GetTileType() == Tiles.Water)
            {
                return;
            }

            // Should skip this too?
            if (newType == Tiles.Water)
            {
                return;
            }

            var block = this.tilemap.GetTileBlockAt(pos);

            if (!block)
            {
                Debug.Log(pos);
            }

            // Clear all children
            foreach (Transform child in block.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            if (newType == Tiles.HighGround)
            {
                GameObject.Instantiate(this.highGroundPrefab, block.transform);
            }

            if (newType == Tiles.Grass)
            {
                GameObject.Instantiate(this.grassPrefab, block.transform);
            }

            this.tilemap.ChangeTileAt(pos, newType);
        }

        private void OnEnable()
        {
            this.tilemap = this.GetComponent<Tilemap>();
            this.generator = this.GetComponent<TerrainGenerator>();
        }
    }
}