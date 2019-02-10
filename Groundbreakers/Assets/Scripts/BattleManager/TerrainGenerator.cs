namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <summary>
    ///     The terrain generator class handles all biome/path generation algorithm. Note the class
    ///     only generate pure data(i.e. a 2D Array for map data, and List of Vector3 for paths).
    ///     The user, namely, the GameMap should instantiate the GameObjects using the data from
    ///     this class. This class is purely data oriented, no instances of Tile prefab.
    /// </summary>
    public class TerrainGenerator : MonoBehaviour
    {
        #region Inspector Properties

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

        private Tiles[,] data = new Tiles[Dimension, Dimension];

        private List<Vector3> pathA = new List<Vector3>();

        private List<Vector3> pathB = new List<Vector3>();

        #endregion

        #region Public Properties

        public enum Tiles
        {
            Path,
            Grass,
            Stone,
            Wall,
        }

        [Flags]
        private enum Direction
        {
            Down = 1,
            Left = 2,
            Right = 4,
        }

        public static int Dimension { get; } = 8;

        #endregion

        #region Public functions

        public void Initialize()
        {
            this.GenerateTerrain();
            this.GeneratePaths();
        }

        public Tiles GetTileTypeAt(int x, int y)
        {
            return this.data[x, y];
        }

        public List<Vector3> GetPathA()
        {
            return this.pathA;
        }

        public List<Vector3> GetPathB()
        {
            return this.pathB;
        }

        #endregion

        #region Terrain Generation Related

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

                    var a = Mathf.PerlinNoise(freq * nx, freq * ny);
                    var b = freq / 2 * Mathf.PerlinNoise(freq * 2 * nx, freq * 2 * ny);
                    var c = freq / 4 * Mathf.PerlinNoise((freq * 4) + nx, (freq * 4) + ny);

                    heightMap[i, j] = a + b + c;
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

        #endregion

        #region Path Generation Related

        /// <summary>
        ///     Generate two paths according to the design document. And store the path data into
        ///     the fields.
        /// </summary>
        private void GeneratePaths()
        {
            const int Length = 12;

            this.pathA = this.CreatePathSmart(Length, 0, 3);
            this.pathB = this.CreatePathSmart(Length, 4, 7);

            foreach (var pos in this.pathA)
            {
                this.data[(int)pos.x, (int)pos.y] = Tiles.Path;
            }

            foreach (var pos in this.pathB)
            {
                this.data[(int)pos.x, (int)pos.y] = Tiles.Path;
            }
        }


        /// <summary>
        ///     Get a new direction given the constrain.
        /// </summary>
        /// <param name="allowed">
        ///     The Bitmask that constrains your options
        /// </param>
        /// <returns>
        ///     The <see cref="Direction"/>.
        /// </returns>
        private Direction GetNewDirection(Direction allowed)
        {
            Direction newDir;
            do
            {
                var values = Enum.GetValues(typeof(Direction));
                newDir = (Direction)values.GetValue(Random.Range(0, values.Length));
            }
            while ((allowed & newDir) == 0);
            return newDir;
        }

        /// <summary>
        ///     Using very dummy approach (Random-Walk with constrains) to generate a path within
        ///     the margins.
        /// </summary>
        /// <param name="leftMargin">
        ///     The left margin in index [0,...7]
        /// </param>
        /// <param name="rightMargin">
        ///     The right margin in index [0,...7]
        /// </param>
        /// <returns>
        ///     The <see cref="List"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     should not happen.
        /// </exception>
        private List<Vector3> CreatePath(int leftMargin, int rightMargin)
        {
            List<Vector3> path = new List<Vector3>();
            
            var currentX = Random.Range(leftMargin, rightMargin + 1);
            var currentY = 7;

            path.Add(new Vector3(currentX, currentY));

            var currentAllowed = Direction.Down;
            var lastDirection = Direction.Down;
            while (currentY != 0)
            {
                var option = this.GetNewDirection(currentAllowed);

                int newX = currentX;
                int newY = currentY;

                switch (option)
                {
                    case Direction.Down:
                        if (lastDirection == Direction.Left)
                        {
                            currentAllowed = Direction.Left | Direction.Down;
                        }
                        else if (lastDirection == Direction.Right)
                        {
                            currentAllowed = Direction.Down | Direction.Right;
                        }
                        else
                        {
                            currentAllowed = Direction.Left | Direction.Down | Direction.Right;
                        }

                        newY--;
                        break;
                    case Direction.Left:
                        currentAllowed = Direction.Left | Direction.Down;
                        newX--;
                        break;
                    case Direction.Right:
                        currentAllowed = Direction.Down | Direction.Right;
                        newX++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                newX = Mathf.Clamp(newX, leftMargin, rightMargin);

                if (path.Contains(new Vector3(newX, newY)))
                {
                    continue;
                }

                lastDirection = option;
                currentX = newX;
                currentY = newY;
                path.Add(new Vector3(currentX, currentY));
            }

            return path;
        }

        /// <summary>
        ///     Using a very smart way to generate a path with Fixed length N.
        /// </summary>
        /// <param name="length">
        ///     The length N, which should be greater that otherwise Infinite loop.
        /// </param>
        /// <param name="leftMargin">
        ///     The left margin in index [0,...7]
        /// </param>
        /// <param name="rightMargin">
        ///     The right margin in index [0,...7]
        /// </param>
        /// <returns>
        ///     The <see cref="List"/>.
        /// </returns>
        private List<Vector3> CreatePathSmart(int length, int leftMargin, int rightMargin)
        {
            List<Vector3> path;
            do
            {
                path = this.CreatePath(leftMargin, rightMargin);
            }
            while (path.Count != length);

            return path;
        }

        #endregion
    }
}
