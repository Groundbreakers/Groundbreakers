namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    using Random = UnityEngine.Random;

    public class GameMap : MonoBehaviour
    {
        #region Internal Constants

        private const uint Dimension = 8;

        private const float CellSize = 1.0f;

        #endregion

        #region Inspector Properties

        [SerializeField]
        private GameObject tileA = null;

        [SerializeField]
        private GameObject tileB = null;

        [SerializeField]
        private GameObject tileC = null;

        [SerializeField]
        private GameObject tilePath = null;

        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float frequency = 5.0f;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float grassMax = 0.65f;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float sandMax = 0.45f;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float stoneMax = 1.0f;

        #endregion

        #region Internal Variables

        /// <summary>
        /// The parent transform that hold all sub sprite for tiles.
        /// </summary>
        private Transform tilesHolder;

        private Transform mapHolder;

        private Tiles[,] data;

        private Transform[,] tileBlocks;

        #endregion

        #region Public Properties

        public enum Tiles
        {
            Path,
            Grass,
            Stone,
            Wall,
        }

        #endregion

        #region Unity Callbacks

        public void Start()
        {
            this.mapHolder = new GameObject("Map").transform;
            this.data = new Tiles[Dimension, Dimension];
            this.tileBlocks = new Transform[Dimension, Dimension];


            this.tilesHolder = new GameObject("tilesHolder").transform;
            this.tilesHolder.SetParent(this.mapHolder);

            this.GenerateTerrain();
            this.CreatePath(0, 3);
            this.CreatePath(4, 7);
            this.CreateGameBoard();

            // Locating the mother board
            float newX = -Dimension * CellSize / 2.0f;
            float newY = -Dimension * CellSize / 2.0f;

            this.mapHolder.SetPositionAndRotation(new Vector3(newX, newY, 0.0f), Quaternion.identity);
        }

        #endregion

        #region Public functions

        public Transform GetTileAt(int x, int y)
        {
            return this.tileBlocks[x, y];
        }

        #endregion

        #region Internal functions

        /// <summary>
        /// The generate terrain, and store the information into the data field.
        /// After calling this function, the 'data' field is filled with biome data.
        /// </summary>
        private void GenerateTerrain()
        {
            float freq = this.frequency;
            float[,] heightMap = new float[Dimension, Dimension];
            float[,] moisture = new float[Dimension, Dimension];

            // Generating Noise
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var nx = (i / (float)Dimension) - 0.5f;
                    var ny = (j / (float)Dimension) - 0.5f;

                    heightMap[i, j] = Mathf.PerlinNoise(freq * nx, freq * ny)
                                      + (freq / 2 * Mathf.PerlinNoise(freq * 2 * nx, freq * 2 * ny))
                                      + (freq / 4 * Mathf.PerlinNoise((freq * 4) + nx, (freq * 4) + ny));
                    heightMap[i, j] = (heightMap[i, j] + 1) / 2;
                }
            }

            // Generate Tile map
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var e = heightMap[i, j];
                    var m = moisture[i, j];

                    this.data[i, j] = this.GetBiomeType(e, m);
                }
            }
        }

        /// <summary>
        /// Returns the type of terrain that corresponds to the given parameters.
        /// </summary>
        /// <param name="e">
        /// The 2D array data that represents the elevation.
        /// </param>
        /// <param name="m">
        /// The 2D array data that represents the moisture.
        /// </param>
        /// <returns>
        /// The <see cref="Tiles"/> that corresponds to a specific tile type.
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

        private void CreateGameBoard()
        {
            // Spawning each individual tiles
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var instance = this.InstantiateTileAt(this.data[i, j], i, j);

                    this.tileBlocks[i, j] = instance.transform;
                    Debug.Log("a");
                }
            }
        }

        /// <summary>
        /// Given the tile type, instantiate a GameObject from corresponding prefab at the location (x,y).
        /// </summary>
        /// <param name="tileType">
        /// The tile type <see cref="Tiles"/>
        /// </param>
        /// <param name="x">
        /// The X coordinate of the desired grid to place the tile.
        /// </param>
        /// <param name="y">
        /// The Y coordinate of the desired grid to place the tile.
        /// </param>
        /// <returns>
        /// The <see cref="GameObject"/>.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Should not happen.
        /// </exception>
        private GameObject InstantiateTileAt(Tiles tileType, int x, int y)
        {
            GameObject tile;

            // Determine Enum -> GameObject
            switch (tileType)
            {
                case Tiles.Path:
                    tile = this.tilePath;
                    break;
                case Tiles.Grass:
                    tile = this.tileA;
                    break;
                case Tiles.Stone:
                    tile = this.tileB;
                    break;
                case Tiles.Wall:
                    tile = this.tileC;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Finally Instantiate it.
            var instance = Instantiate(tile, new Vector3(x * CellSize, y * CellSize, 0f), Quaternion.identity);

            instance.GetComponent<SpriteRenderer>().sortingOrder = (int)Dimension - y;
            instance.transform.SetParent(this.tilesHolder);
            return instance;
        }

        private void CreatePath(int leftMargin, int rightMargin)
        {
            var startIndex = Random.Range(leftMargin, rightMargin);

            List<Vector2> path = new List<Vector2>();

            // Naive path algorithm
            var depth = 0;
            var current = startIndex;
            while (depth < Dimension)
            {
                int newX;
                newX = Random.Range(current - 1, current + 2);
                newX = Mathf.Clamp(newX, leftMargin, rightMargin);

                current = newX;

                this.data[current, depth] = Tiles.Path;
                depth++;
            }
        }

        #endregion
    }
}
