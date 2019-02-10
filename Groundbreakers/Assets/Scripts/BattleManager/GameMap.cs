namespace Assets.Scripts
{
    using System;
    using System.Collections;

    using UnityEngine;

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

        /// <summary>
        /// The total enter duration for the map. Recommend set the value to (MaxDelay + Duration)
        /// of the TilePrefab's Enter Behaviour setting. While the tiles are entering, the player
        /// can not deploy characters on to the tiles.
        /// <see cref="EnterBehavior"/>
        /// </summary>
        [SerializeField]
        [Range(3.0f, 10.0f)]
        private float totalEnterDuration = 4.0f;

        [SerializeField]
        private GameObject tileA;

        [SerializeField]
        private GameObject tileB;

        [SerializeField]
        private GameObject tileC;

        [SerializeField]
        private GameObject tilePath;

        #endregion

        #region Internal Variables

        private Transform[,] tileBlocks = new Transform[TG.Dimension, TG.Dimension];

        private MobSpawner mobSpawner;

        /// <summary>
        /// The Saving a reference to the tile generator component.
        /// </summary>
        private TG generator;

        #endregion

        #region IBattlePhaseHandler

        public void OnBattleBegin()
        {
            // Create a new level
            this.SetupNewLevel();

            this.StartCoroutine(this.StartEntering());
        }

        public void OnBattleEnd()
        {
            var children = this.GetComponentsInChildren<EnterBehavior>();

            foreach (var behavior in children)
            {
                behavior.StartExiting();
            }
        }

        #endregion

        #region Public Functions

        public void SetupNewLevel()
        {
            // Generate new map data
            this.generator.Initialize();

            // Instantiate tiles from data
            this.InstantiateTiles();
            this.SetupMobSpawner();
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            BattleManager.StartListening("start", this.OnBattleBegin);
            BattleManager.StartListening("end", this.OnBattleEnd);

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

        private IEnumerator StartEntering()
        {
            // Tell each tiles to start tween in.
            var children = this.GetComponentsInChildren<EnterBehavior>();
            foreach (var behavior in children)
            {
                behavior.StartEntering();
            }

            yield return new WaitForSeconds(this.totalEnterDuration);

            Debug.Log("all block ready");

            BattleManager.TriggerEvent("block ready");
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
            // Clear the 2D array
            foreach (Transform tileBlock in this.transform)
            {
                GameObject.Destroy(tileBlock.gameObject);
            }

            // Re instantiate all tiles
            for (var x = 0; x < TG.Dimension; x++)
            {
                for (var y = 0; y < TG.Dimension; y++)
                {
                    Tiles tileType = this.generator.GetTileTypeAt(x, y);

                    var instance = this.InstantiateTileAt(tileType, x, y);

                    // TODO: Refactor here
                    if (tileType == Tiles.Path)
                    {
                        instance.GetComponent<SelectNode>().SetCanDeploy(false);
                    }

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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Finally Instantiate it.
            var instance = Instantiate(
                tile,
                new Vector3(x * CellSize, y * CellSize, 0.0f),
                Quaternion.identity);

            // Setting order and parent
            instance.GetComponent<TileBlock>().SetSortingOrder(TG.Dimension - y);
            instance.transform.SetParent(this.transform);
            return instance;
        }

        #endregion
    }
}
