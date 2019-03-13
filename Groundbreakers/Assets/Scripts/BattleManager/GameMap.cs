namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using Random = UnityEngine.Random;
    using TG = TerrainGenerator;
    using Tiles = TerrainGenerator.Tiles;

    /// <summary>
    /// This function should be attached to the GameMap Object in the MainScene. This class handles
    /// All tile block related features. For example, at each level, it construct a new tile map
    /// using the data obtained from the TerrainGenerator. 
    /// </summary>
    [RequireComponent(typeof(TG))]
    public class GameMap : MonoBehaviour, IBattlePhaseHandler
    {
        #region Internal Constants

        // We assume each tile block takes 1 unit in the space.
        private const float CellSize = 1.0f;

        #endregion

        #region Inspector Properties

        [SerializeField]
        private GameObject tileA;

        [SerializeField]
        private GameObject tileB;

        [SerializeField]
        private GameObject tileC;

        [SerializeField]
        private GameObject tilePath;

        [SerializeField]
        private GameObject mushroom;

        [SerializeField]
        private GameObject plants;

        [SerializeField]
        private GameObject water;

        #endregion

        #region Internal Variables

        private Transform[,] tileBlocks = new Transform[TG.Dimension, TG.Dimension];

        private MobSpawner mobSpawner;

        /// <summary>
        /// The Saving a reference to the tile generator component.
        /// </summary>
        private TG generator;

        private List<GameObject> mushrooms = new List<GameObject>();

        #endregion

        #region IBattlePhaseHandler

        public void OnBattleBegin()
        {
            // Create a new level
            this.SetupNewLevel();

            var children = this.GetComponentsInChildren<EnterBehavior>();
            foreach (var behavior in children)
            {
                behavior.StartEntering();
            }
        }

        public void OnBattleEnd()
        {
            var children = this.GetComponentsInChildren<EnterBehavior>();

            foreach (var behavior in children)
            {
                behavior.StartExiting();
            }
        }

        public void OnBattleVictory()
        {
            // Clear the 2D array
            foreach (Transform tileBlock in this.transform)
            {
                GameObject.Destroy(tileBlock.gameObject);
            }
        }

        #endregion

        #region Public Functions

        public Transform GetTileAt(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            return this.GetTileAt(x, y);
        }

        public Transform GetTileAt(int x, int y)
        {
            Debug.Log(x + " " + y);
            return this.tileBlocks[x, y];
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            BattleManager.StartListening("start", this.OnBattleBegin);
            BattleManager.StartListening("end", this.OnBattleEnd);
            BattleManager.StartListening("victory", this.OnBattleVictory);

            this.mobSpawner = BattleManager.Instance.GetComponent<MobSpawner>();
            this.generator = this.GetComponent<TG>();
        }

        private void OnDisable()
        {
            BattleManager.StopListening("start", this.OnBattleBegin);
            BattleManager.StopListening("end", this.OnBattleEnd);
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Procedural generate a new map
        /// </summary>
        private void SetupNewLevel()
        {
            // Generate new map data
            this.generator.Initialize();

            // Instantiate tiles from data
            this.InstantiateTiles();
            this.SetupPathSprite();
            this.SetupMobSpawner();
            this.InstantiateEnvironments();
        }

        /// <summary>
        /// We need to manually determine what each type of the path is.
        /// Note this solution is very ugly but works :)
        /// </summary>
        private void SetupPathSprite()
        {
            this.RelaxPath(this.generator.GetPathA());
            this.RelaxPath(this.generator.GetPathB());
        }

        /// <summary>
        /// Private helper function to reset sprites of path.
        /// I understand this is an inefficient algorithm. However, due to the scale of number of
        /// paths is small. In this case it does not matter at all.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        private void RelaxPath(List<Vector3> path)
        {
            foreach (var pos in path)
            {
                var block =
                    this.tileBlocks[(int)pos.x, (int)pos.y].GetComponentInChildren<PathAtlas>();

                var left = !path.Contains(new Vector3(pos.x - 1, pos.y));
                var right = !path.Contains(new Vector3(pos.x + 1, pos.y));
                var up = !path.Contains(new Vector3(pos.x, pos.y - 1));
                var down = !path.Contains(new Vector3(pos.x, pos.y + 1));

                if (left && right)
                {
                    block.SetDirection("road_2");
                }
                else if (up && down)
                {
                    block.SetDirection();
                }
                else if (up && left)
                {
                    block.SetDirection("road_4");
                }
                else if (up && right)
                {
                    block.SetDirection("road_3");
                }
                else if (left && down)
                {
                    block.SetDirection("road_5");
                }
                else if (right && down)
                {
                    block.SetDirection("road_6");
                }
            }
        }

        /// <summary>
        /// Heritage from Austin. Adding vector3 points to the spawn. 
        /// </summary>
        private void SetupMobSpawner()
        {
            this.mobSpawner.ClearPoints();

            var path = this.generator.GetPathA();
            foreach (var node in path)
            {
                this.mobSpawner.AddPoint(node, 1);
            }

            path = this.generator.GetPathB();
            foreach (var node in path)
            {
                this.mobSpawner.AddPoint(node, 2);
            }
        }

        private void SetupFromChildren()
        {
            var index = 0;
            foreach (Transform child in this.transform)
            {
                if (child.CompareTag("Tile"))
                {
                    var x = index % TG.Dimension;
                    var y = index / TG.Dimension;

                    this.tileBlocks[x, y] = child;

                    index++;
                }
            }
        }

        /// <summary>
        /// Instantiate all tiles. Must call generator.Initialize before using this function.
        /// This function will destroy any existing tileBlock GameObjects. 
        /// </summary>
        private void InstantiateTiles()
        {
            // Re instantiate all tiles
            for (var x = 0; x < TG.Dimension; x++)
            {
                for (var y = 0; y < TG.Dimension; y++)
                {
                    Tiles tileType = this.generator.GetTileTypeAt(x, y);

                    var instance = this.InstantiateTileAt(tileType, x, y);

                    this.tileBlocks[x, y] = instance.transform;
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
                case Tiles.Water:
                    tile = this.water;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Finally Instantiate it.
            var instance = Instantiate(
                tile,
                new Vector3(x * CellSize, y * CellSize, 0.0f),
                Quaternion.identity);

            // Setting order and parent
            instance.transform.SetParent(this.transform);
            return instance;
        }

        /// <summary>
        /// Instantiating fast and dirty but works fine environmental objects. 
        /// </summary>
        private void InstantiateEnvironments()
        {
            //int num = Random.Range(2, 5);
            int num = 28;
            this.mushrooms.Clear();

            // We use naive approach here
            for (int i = 0; i < num; i++)
            {
                bool duplicate;
                Transform block;
                do
                {
                    block = this.PickNonOcuppiedBlock();
                    duplicate = this.mushrooms.Any(
                        go => go.transform.position.Equals(block.position));
                }
                while (duplicate);

                Debug.Log("Block Location: " + block.position.x + " " + block.position.y);
                var mush = Instantiate(this.mushroom, block);
                mush.transform.localPosition = Vector3.zero;
                this.mushrooms.Add(mush);

                // Manually set these tile to undeployable
                // block.GetComponent<SelectNode>().SetCanDeploy(false);
            }
        }

        private Transform PickNonOcuppiedBlock()
        {
            int x;
            int y;
            do
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
            }
            while (this.generator.GetTileTypeAt(x, y) == Tiles.Path ||
                    this.generator.GetTileTypeAt(x, y) == Tiles.Water);

            return this.tileBlocks[x, y];
        }

        #endregion
    }
}
