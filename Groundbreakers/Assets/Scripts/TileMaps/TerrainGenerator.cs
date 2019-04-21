namespace TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <inheritdoc cref="ITerrainData" />
    /// <summary>
    ///     The terrain generator class handles all biome/path generation algorithm. Note the class
    ///     only generate pure data(i.e. a 2D Array for map data, and List of Vector3 for paths).
    ///     The user, namely, the GameMap should instantiate the GameObjects using the data from
    ///     this class. This class is purely data oriented, no instances of Tile prefab.
    /// </summary>
    [SuppressMessage("ReSharper", "StyleCop.SA1407", Justification = "Ok here")]
    public class TerrainGenerator : MonoBehaviour, ITerrainData
    {
        /// <summary>
        ///     Pure map data stored as "Tiles" in native 2D array.
        /// </summary>
        private readonly Tiles[,] data = new Tiles[Tilemap.Dimension, Tilemap.Dimension];

        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float frequency = 5.0f;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float grassMax = 0.65f;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float sandMax = 0.45f;

        public IEnumerable<Vector3> GetSpawnLocations()
        {
            throw new NotImplementedException();
        }

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
            if (x < 0 || x >= Tilemap.Dimension || y < 0 || y >= Tilemap.Dimension)
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
            this.GenerateTerrain();
            this.GenerateEnvironment();
        }

        private void GenerateEnvironment()
        {
            var num = Random.Range(1, 3);
            for (var i = 0; i < num; i++)
            {
                int x;
                int y;
                do
                {
                    x = Random.Range(0, Tilemap.Dimension);
                    y = Random.Range(0, Tilemap.Dimension);
                }
                while (this.GetTileTypeAt(x, y) == Tiles.Path);

                this.data[x, y] = Tiles.Water;
            }
        }

        /// <summary>
        ///     The generate terrain, and store the information into the data field.
        ///     After calling this function, the 'data' field is filled with biome data.
        /// </summary>
        private void GenerateTerrain()
        {
            var n = Tilemap.Dimension;

            var freq = this.frequency;
            var heightMap = new float[n, n];
            var moisture = new float[n, n];

            // Generating Noise
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    var nx = i / (float)n - 0.5f;
                    var ny = j / (float)n - 0.5f;

                    var a = Mathf.PerlinNoise(freq * nx, freq * ny);
                    var b = freq / 2 * Mathf.PerlinNoise(freq * 2 * nx, freq * 2 * ny);
                    var c = freq / 4 * Mathf.PerlinNoise(freq * 4 + nx, freq * 4 + ny);

                    heightMap[i, j] = a + b + c;
                    heightMap[i, j] = (heightMap[i, j] + 1) / 2;
                }
            }

            // Generate Tile map
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    var e = heightMap[i, j];
                    var m = moisture[i, j];

                    this.data[i, j] = this.GetBiomeType(e, m);
                }
            }
        }

        /// <summary>
        ///     Returns the type of terrain that corresponds to the given parameters.
        /// </summary>
        /// <param name="e">
        ///     The 2D array data that represents the elevation.
        /// </param>
        /// <param name="m">
        ///     The 2D array data that represents the moisture.
        /// </param>
        /// <returns>
        ///     The <see cref="Tiles" /> that corresponds to a specific tile type.
        /// </returns>
        private Tiles GetBiomeType(float e, float m)
        {
            e -= 1;

            if (e < this.sandMax)
            {
                return Tiles.Grass;
            }

            if (e < this.grassMax)
            {
                return Tiles.Wall;
            }

            return Tiles.Stone;
        }
    }
}