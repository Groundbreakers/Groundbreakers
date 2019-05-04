namespace TileMaps
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <inheritdoc cref="ITerrainData" />
    /// <summary>
    ///     Load a hard-coded map. 
    /// </summary>
    public class CustomTerrain : MonoBehaviour, ITerrainData
    {
        /// <summary>
        ///     The 2D array that stores the important map data.
        /// </summary>
        private Tiles[,] data;

        /// <summary>
        ///     Gets the size of this map.
        /// </summary>
        private static int Dimension { get; } = 8;

        #region ITerrainData

        /// <inheritdoc />
        /// <summary>
        ///     The get tile type at.
        /// </summary>
        /// <param name="x">
        ///     The x coordinate of the data. Range: [0, 7]
        /// </param>
        /// <param name="y">
        ///     The y coordinate of the data. Range: [0, 7]
        /// </param>
        /// <returns>
        ///     The <see cref="T:Assets.Scripts.Tiles" />. The enumeration that represent the tile's type.
        /// </returns>
        public Tiles GetTileTypeAt(float x, float y)
        {
            if (x < 0 || x >= Dimension || y < 0 || y >= Dimension)
            {
                return Tiles.OutOfBound;
            }

            return this.data[(int)x, (int)y];
        }

        /// <inheritdoc />
        /// <summary>
        ///     This function should be called by GameMap. A new sets of data is generated each time
        ///     you call this Initialize Function.
        /// </summary>
        public void Initialize()
        {
            // This is temp
            this.data = new[,]
                            {
                                {
                                    Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.HighGround, Tiles.Grass, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.HighGround, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.HighGround, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.HighGround, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.HighGround, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass
                                },
                                {
                                    Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass
                                }
                            };
        }

        public List<Vector3> GetMushroomLocations()
        {
            return new List<Vector3>();
        }

        #endregion
    }
}