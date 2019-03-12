namespace Assets.Scripts
{
    using System;

    using Sirenix.OdinInspector;

    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <inheritdoc cref="ITerrainData" />
    /// <summary>
    /// </summary>
    public class CustomTerrain : MonoBehaviour, ITerrainData
    {
        [SerializeField]
        private GameObject bossPrefab;

        [SerializeField]
        private Tiles[,] data = new Tiles[8, 8];

        private static int Dimension { get; } = 8;

        /// <inheritdoc />
        /// <summary>
        ///     The get tile type at.
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

        /// <summary>
        ///     This function should be called by GameMap. A new sets of data is generated each time
        ///     you call this Initialize Function.
        /// </summary>
        public void Initialize()
        {
            // This is temp
            this.data = new[,]
                            {
                                { Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Water, Tiles.Water, Tiles.Grass, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, },
                                { Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, Tiles.Grass, },
                            };

            this.GenerateEnviroment();
            this.GenerateBoss();
        }

        private void GenerateBoss()
        {
            Instantiate(this.bossPrefab, new Vector3(3.0f, 3.0f), Quaternion.identity);
        }

        private void GenerateEnviroment()
        {
            var num = Random.Range(1, 3);
            for (var i = 0; i < num; i++)
            {
                int x;
                int y;
                do
                {
                    x = Random.Range(0, Dimension);
                    y = Random.Range(0, Dimension);
                }
                while (this.GetTileTypeAt(x, y) == Tiles.Path);

                this.data[x, y] = Tiles.Water;
            }
        }
    }
}