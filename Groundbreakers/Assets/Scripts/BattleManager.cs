namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    using Random = UnityEngine.Random;

    public class BattleManager : MonoBehaviour, IBattlePhaseHandler
    {
        #region Inspector Variables

        [SerializeField]
        private GameObject tileA = null;

        [SerializeField]
        private GameObject tileB = null;

        [SerializeField]
        private GameObject tileC = null;

        [SerializeField]
        private GameObject tilePath = null;

        [SerializeField]
        private GameObject spawner = null;

        [SerializeField]
        private GameObject wayPoint = null;

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

        private const uint Dimension = 8;

        private const float CellSize = 1.0f;

        /// <summary>
        /// The parent transform that hold all sub sprite for tiles.
        /// </summary>
        private Transform tilesHolder;

        private Transform mapHolder;

        private Tiles[,] data;

        #endregion

        #region Public Properties

        public enum Tiles
        {
            Grass,
            Stone,
            Wall,
        }

        public enum Stages
        {
            Null,
            Entering,
            Combating,
            Exiting
        }

        public static Stages GameState { get; private set; } = Stages.Null;

        #endregion

        #region Unity Callbacks

        public void Start()
        {
            this.mapHolder = new GameObject("Map").transform;


            this.GenerateTerrain();
            this.CreateGameBoard();
            this.CreatePath(0, 3);

            // Locating the mother board
            float newX = -Dimension * CellSize / 2.0f;
            float newY = -Dimension * CellSize / 2.0f;

            this.mapHolder.SetPositionAndRotation(
                new Vector3(newX, newY, 0f),
                Quaternion.identity);
        }

        public void Update()
        {
            // Debug only
            if (Input.GetKeyDown("space"))
            {
                switch (GameState)
                {
                    case Stages.Null:
                        this.OnTilesEntering();
                        GameState = Stages.Entering;
                        break;
                    case Stages.Entering:
                        GameState = Stages.Combating;
                        break;
                    case Stages.Combating:
                        GameState = Stages.Exiting;
                        this.OnTilesExiting();
                        break;
                    case Stages.Exiting:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Debug.Log(GameState);
            }
        }

        #endregion

        #region IBattlePhaseHandler

        public void OnTilesEntering()
        {
            foreach (Transform child in this.tilesHolder)
            {
                var tb = child.GetComponent<TileBlock>();
                tb.OnTilesEntering();
            }
        }

        public void OnBattling()
        {
            throw new System.NotImplementedException();
        }

        public void OnTilesExiting()
        {
            foreach (Transform child in this.tilesHolder)
            {
                var tb = child.GetComponent<TileBlock>();
                tb.OnTilesExiting();
            }
        }

        #endregion

        #region Internal functions

        private void GenerateTerrain()
        {
            float freq = this.frequency;
            float[,] heightMap = new float[Dimension, Dimension];
            float[,] moisture = new float[Dimension, Dimension];
            this.data = new Tiles[Dimension, Dimension];

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

            //// Generate Moisture
            //for (var i = 0; i < Dimension; i++)
            //{
            //    for (var j = 0; j < Dimension; j++)
            //    {
            //        var nx = (i / (float)Dimension) - 0.5f;
            //        var ny = (j / (float)Dimension) - 0.5f;

            //        moisture[i, j] = Mathf.PerlinNoise(freq * nx, freq * ny)
            //                          + (freq / 2 * Mathf.PerlinNoise(freq * 2 * nx, freq * 2 * ny))
            //                          + (freq / 4 * Mathf.PerlinNoise((freq * 4) + nx, (freq * 4) + ny));
            //        moisture[i, j] = (moisture[i, j] + 1) / 2;
            //    }
            //}

            // Generate Tilemap
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var e = heightMap[i, j];
                    var m = moisture[i, j];

                    this.data[i, j] = this.GetBiomeType(e, m);

                    Debug.Log(this.data[i, j] + "    " + e);

                }
            }
        }

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
            this.tilesHolder = new GameObject("tilesHolder").transform;

            // Spawning each individual tiles
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    GameObject tile;

                    switch (this.data[i, j])
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
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    var instance = Instantiate(
                        tile, 
                        new Vector3(i * CellSize, j * CellSize, 0f), 
                        Quaternion.identity);

                    instance.GetComponent<SpriteRenderer>().sortingOrder = (int)Dimension - j;
                    instance.transform.SetParent(this.tilesHolder);
                }
            }

            //// Locating the mother board
            //float newX = -Dimension * CellSize / 2.0f; 
            //float newY = -Dimension * CellSize / 2.0f;

            //this.mapHolder.SetPositionAndRotation(
            //    new Vector3(newX, newY, 0f),
            //    Quaternion.identity);
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
                var newX = Random.Range(current - 1, current + 2);
                newX = Mathf.Clamp(newX, leftMargin, rightMargin);

                if (newX != current)
                {
                    path.Add(new Vector2(newX, depth));
                    path.Add(new Vector2(current, depth));
                }

                current = newX;
                depth++;
            }

            foreach (var vec in path)
            {
                // Placing a way point at here
                var instance = Instantiate(
                    this.wayPoint,
                    new Vector3(vec.x * CellSize, vec.y * CellSize, 0f),
                    Quaternion.identity);

                // Also Change the tile to path tile

                instance.transform.SetParent(this.mapHolder);
            }

            // Locating the mother board
            this.tilesHolder.SetParent(this.mapHolder);
        }

        #endregion
    }
}