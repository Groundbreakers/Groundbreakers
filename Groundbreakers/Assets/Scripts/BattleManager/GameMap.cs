namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using UnityEngine;

    using Debug = UnityEngine.Debug;
    using Random = UnityEngine.Random;

    public class GameMap : MonoBehaviour
    {
        #region Internal Constants

        public const uint Dimension = 8;

        private const float CellSize = 1.0f;

        #endregion

        #region Inspector Properties

        //[SerializeField]
        //private GameObject[] tiles;

        [SerializeField]
        private GameObject tileA = null;

        [SerializeField]
        private GameObject tileB = null;

        [SerializeField]
        private GameObject tileC = null;

        [SerializeField]
        private GameObject tilePath = null;

        //[SerializeField]
        //private GameObject spawner = null;

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

        private WaveSpawner waveSpawner;

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
        public enum Direction
        {
            Down = 1,
            Left = 2,
            Right = 4,
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

            this.waveSpawner = this.GetComponent<WaveSpawner>();

            this.GenerateTerrain();

            this.InitializeWaveSpawner();
                

            this.CreateGameBoard();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// The get tile at.
        /// </summary>
        /// <param name="pos">
        /// The pos.
        /// </param>
        /// <returns>
        /// The <see cref="Transform"/>.
        /// </returns>
        public Transform GetTileAt(Vector3 pos)
        {
            return this.tileBlocks[(int)pos.x, (int)pos.y];
        }

        #endregion

        #region Internal functions

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

        private void CreateGameBoard()
        {
            // Spawning each individual tiles
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    Tiles tileType = this.data[i, j];
                    var instance = this.InstantiateTileAt(tileType, i, j);

                    this.tileBlocks[i, j] = instance.transform;

                    // TODO: Refactor this part
                    if (tileType == Tiles.Path)
                    {
                        // Basically disable the component that allows the player to select.
                        instance.GetComponent<SelectNode>().SetCanDeploy(false);
                    }
                }
            }
        }

        /// <summary>
        /// Given the tile type, instantiate a GameObject from corresponding prefab at the
        /// location (x,y).
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

            // A function that maps Enum -> GameObject.
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
            var instance = Instantiate(
                tile, 
                new Vector3(x * CellSize, y * CellSize, 0.0f), 
                Quaternion.identity);

            instance.GetComponent<TileBlock>().SetSortingOrder((int)Dimension - y);
            instance.transform.SetParent(this.tilesHolder);
            return instance;
        }

        #endregion

        #region Path Generation Related

        /// <summary>
        /// Get a new direction given the constrain.
        /// </summary>
        /// <param name="allowed">
        /// The Bitmask that constrains your options
        /// </param>
        /// <returns>
        /// The <see cref="Direction"/>.
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
        /// Using very dummy approach (Random Walk with constrains) to generate a path within the
        /// margins.
        /// </summary>
        /// <param name="leftMargin">
        /// The left margin in index [0,...7]
        /// </param>
        /// <param name="rightMargin">
        /// The right margin in index [0,...7]
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// should not happen.
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

            // path.Reverse();
            return path;
        }

        /// <summary>
        /// Using a very smart way to generate a path with Fixed length N.
        /// </summary>
        /// <param name="length">
        /// The length N, which should be greater that otherwise Infinite loop.
        /// </param>
        /// <param name="leftMargin">
        /// The left margin in index [0,...7]
        /// </param>
        /// <param name="rightMargin">
        /// The right margin in index [0,...7]
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<Vector3> CreatePathSmart(int length, int leftMargin, int rightMargin)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            List<Vector3> path;
            do
            {
                path = this.CreatePath(leftMargin, rightMargin);
            }
            while (path.Count != length);
            sw.Stop();

            Debug.Log("Elapsed " + sw.Elapsed.Milliseconds);

            return path;
        }

        private void InitializeWaveSpawner()
        {
            var n = 12;

            // Creating the first path
            var path = this.CreatePathSmart(n, 0, 3);
            foreach (var pos in path)
            {
                this.data[(int)pos.x, (int)pos.y] = Tiles.Path;
            }

            foreach (var node in path)
            {
                this.waveSpawner.AddPoint(node, 1);
            }

            var newX = path[0].x;
            var newY = path[0].y;
           
            this.waveSpawner.spawnPoint1 = new Vector3(newX, newY);

            // Creating the second path
            path = this.CreatePathSmart(n, 4, 7);
            foreach (var pos in path)
            {
                this.data[(int)pos.x, (int)pos.y] = Tiles.Path;
            }

            foreach (var node in path)
            {
                this.waveSpawner.AddPoint(node, 2);
            }

            newX = path[0].x;
            newY = path[0].y;
            this.waveSpawner.spawnPoint2 = new Vector3(newX, newY);
        }

        #endregion

        #endregion
    }

    #region Editor Stuff

    [Serializable]
    public class TilePrefab
    {
        public GameObject prefab;

        public int amount;
    }

    #endregion
}
