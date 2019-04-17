namespace TileMaps
{
    using System;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     Container for actual tile GameObjects. Provide API to construct tiles, modify tiles.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CustomTerrain))]
    [RequireComponent(typeof(NavigationMap))]
    public class Tilemap : MonoBehaviour
    {
        private Transform[,] blocks = new Transform[Tilemap.Dimension, Tilemap.Dimension];

        private TileStatus[,] cachedStatus = new TileStatus[Tilemap.Dimension, Tilemap.Dimension];

        private NavigationMap navigationMap;

        private ITerrainData mapData;

        // below are all temp
        [SerializeField]
        private GameObject tileA;

        [SerializeField]
        private GameObject tileB;

        [SerializeField]
        private GameObject tileC;

        [SerializeField]
        private GameObject water;

        /// <summary>
        ///     Gets the squared map's dimension
        /// </summary>
        public static int Dimension { get; } = 8;

        public GameObject GetTileBlockAt(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            return this.GetTileBlockAt(x, y);
        }

        public GameObject GetTileBlockAt(int x, int y)
        {
            return this.blocks[x, y].gameObject;
        }

        public TileStatus GetTileStatusAt(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            return this.GetTileStatusAt(x, y);
        }

        /// <summary>
        ///     Get the cached tile status at position x, y.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <returns>
        ///     The <see cref="TileStatus" />.
        /// </returns>
        public TileStatus GetTileStatusAt(int x, int y)
        {
            return this.cachedStatus[x, y];
        }

        public void SetTileBlock(Vector3 position, Transform block)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            this.blocks[x, y] = block;
            this.cachedStatus[x, y] = block.GetComponent<TileStatus>();
        }

        /// <summary>
        ///     The setup map.
        /// </summary>
        [Button]
        public void SetupMap()
        {
            this.mapData.Initialize();

            this.ClearAllTiles();
            this.InstantiateTiles(this.mapData);
        }

        public void OnTileChanges()
        {
        }

        protected void OnEnable()
        {
            this.mapData = this.GetComponent<CustomTerrain>();
            this.navigationMap = this.GetComponent<NavigationMap>();
        }

        private void ClearAllTiles()
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (var go in tiles)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(go);
                }
                else
                {
                    Destroy(go);
                }
            }
        }

        /// <summary>
        ///     Given the tile type, instantiate a GameObject from corresponding prefab at the
        ///     location (x,y).
        /// </summary>
        /// <param name="tileType">
        ///     The tile type <see cref="Tiles" />
        /// </param>
        /// <param name="x">
        ///     The X coordinate of the desired grid to place the tile.
        /// </param>
        /// <param name="y">
        ///     The Y coordinate of the desired grid to place the tile.
        /// </param>
        /// <returns>
        ///     The <see cref="GameObject" />.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Should not happen.
        /// </exception>
        private GameObject InstantiateTileAt(Tiles tileType, int x, int y)
        {
            GameObject tile;

            // A function that maps Enum -> GameObject.
            switch (tileType)
            {
                case Tiles.Grass:
                    tile = this.tileA;
                    break;
                case Tiles.Stone:
                    tile = this.tileB;
                    break;
                case Tiles.Wall:
                    tile = this.tileC;
                    break;
                case Tiles.Water:
                    tile = this.water;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Finally Instantiate it.
            var instance = Instantiate(tile, new Vector3(x, y, 0.0f), Quaternion.identity);

            // Setting order and parent
            instance.transform.SetParent(this.transform);

            instance.GetComponent<TileStatus>().UpdateTileType(tileType);

            return instance;
        }

        /// <summary>
        ///     Instantiate all tiles. Must call generator.Initialize before using this function.
        ///     This function will destroy any existing tileBlock GameObjects.
        /// </summary>
        /// <param name="sourceData">
        ///     The source Data.
        /// </param>
        private void InstantiateTiles(ITerrainData sourceData)
        {
            // Re instantiate all tiles
            for (var x = 0; x < Tilemap.Dimension; x++)
            {
                for (var y = 0; y < Tilemap.Dimension; y++)
                {
                    var tileType = sourceData.GetTileTypeAt(x, y);

                    var instance = this.InstantiateTileAt(tileType, x, y);

                    this.blocks[x, y] = instance.transform;
                    this.cachedStatus[x, y] = instance.GetComponent<TileStatus>();
                }
            }
        }
    }
}