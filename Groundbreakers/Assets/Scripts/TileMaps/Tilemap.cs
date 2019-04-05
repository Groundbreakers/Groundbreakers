namespace TileMaps
{
    using System;

    using UnityEngine;

    [RequireComponent(typeof(CustomTerrain))]
    [RequireComponent(typeof(TileData))]
    public class Tilemap : MonoBehaviour
    {
        private const float CellSize = 1.0f;

        private Transform[,] blocks = new Transform[TileData.Dimension, TileData.Dimension];

        // below are all temp
        [SerializeField]
        private GameObject tileA;

        [SerializeField]
        private GameObject tileB;

        [SerializeField]
        private GameObject tileC;

        [SerializeField]
        private GameObject water;

        private void Awake()
        {
            var sourceData = this.GetComponent<CustomTerrain>();
            sourceData.Initialize();

            this.InstantiateTiles(sourceData);
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
            var instance = Instantiate(tile, new Vector3(x * CellSize, y * CellSize, 0.0f), Quaternion.identity);

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