namespace TileMaps
{
    using System;

    using UnityEngine;
    using UnityEngine.Assertions;

    [ExecuteInEditMode]
    [RequireComponent(typeof(CustomTerrain))]
    [RequireComponent(typeof(TileData))]
    public class Tilemap : MonoBehaviour
    {
        // below are all temp
        [SerializeField]
        private GameObject tileA;

        [SerializeField]
        private GameObject tileB;

        [SerializeField]
        private GameObject tileC;

        [SerializeField]
        private GameObject water;

        private Transform[,] blocks = new Transform[TileData.Dimension, TileData.Dimension];

        /// <summary>
        /// Contains the map data, which is an abstract data type.
        /// </summary>
        private TileData mapData;

        #region Public API

        /// <summary>
        /// Check if the map is passable at position grid.
        /// </summary>
        /// <param name="grid">
        /// The grid vector, the range should be within the bound of the map.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>. True if the position can pass.
        /// </returns>
        public bool IsMapPassable(Vector3 grid)
        {
            var type = this.mapData.GetTileTypeAt(grid.x, grid.y);

            // temp, currently only water/high ground is not passable.
            if (type == Tiles.Water)
            {
                return false;
            }

            return true;
        }

        public GameObject GetTileBlockAt(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            return this.blocks[x, y].gameObject;
        }

        public void SetTileBlock(Vector3 position, Transform block)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            this.blocks[x, y] = block;
        }

        #endregion

        private void Awake()
        {
            this.mapData = this.GetComponent<TileData>();

            // temp
            var sourceData = this.GetComponent<CustomTerrain>();
            sourceData.Initialize();

            this.ClearAllTiles();
            this.InstantiateTiles(sourceData);
        }

        private void ClearAllTiles()
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (var go in tiles)
            {
                if (Application.isEditor)
                {
                    GameObject.DestroyImmediate(go);
                }
                else
                {
                    GameObject.Destroy(go);
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
            var instance = Instantiate(
                tile, 
                new Vector3(x, y, 0.0f), 
                Quaternion.identity);

            // Setting order and parent
            instance.transform.SetParent(this.transform);
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
            for (var x = 0; x < TileData.Dimension; x++)
            {
                for (var y = 0; y < TileData.Dimension; y++)
                {
                    var tileType = sourceData.GetTileTypeAt(x, y);

                    var instance = this.InstantiateTileAt(tileType, x, y);

                    this.blocks[x, y] = instance.transform;
                }
            }
        }
    }
}