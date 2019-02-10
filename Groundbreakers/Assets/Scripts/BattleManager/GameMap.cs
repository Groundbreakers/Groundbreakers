namespace Assets.Scripts
{
    using UnityEngine;

    using TG = TerrainGenerator;
    using Tiles = TerrainGenerator.Tiles;

    /// <summary>
    /// This function should be attached to the GameMap Object in the MainScene. This class handles
    /// All tile block related features. For example, at each level, it construct a new tile map
    /// using the data obtained from the TerrainGenerator. 
    /// </summary>
    [RequireComponent(typeof(TG))]
    public class GameMap : MonoBehaviour
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

        #endregion

        #region Internal Variables

        private Transform[,] tileBlocks = new Transform[TG.Dimension, TG.Dimension];

        private WaveSpawner waveSpawner;

        /// <summary>
        /// The Saving a reference to the tile generator component.
        /// </summary>
        private TG generator;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.generator = this.GetComponent<TG>();
        }

        private void Start()
        {
            this.waveSpawner = this.GetComponent<WaveSpawner>();

            // this.GenerateTerrain();
            // this.CreateGameBoard();
        }

        #endregion

        #region Internal Functions

        //private void CreateGameBoard()
        //{
        //    // Spawning each individual tiles
        //    for (var x = 0; x < TG.Dimension; x++)
        //    {
        //        for (var y = 0; y < TG.Dimension; y++)
        //        {
        //            Tiles tileType = this.data[x, y];
        //            var instance = this.InstantiateTileAt(tileType, x, y);

        //            this.tileBlocks[x, y] = instance.transform;

        //            // TODO: Refactor this part
        //            if (tileType == Tiles.Path)
        //            {
        //                // Basically disable the component that allows the player to select.
        //                instance.GetComponent<SelectNode>().SetCanDeploy(false);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Given the tile type, instantiate a GameObject from corresponding prefab at the
        ///// location (x,y).
        ///// </summary>
        ///// <param name="tileType">
        ///// The tile type <see cref="Tiles"/>
        ///// </param>
        ///// <param name="x">
        ///// The X coordinate of the desired grid to place the tile.
        ///// </param>
        ///// <param name="y">
        ///// The Y coordinate of the desired grid to place the tile.
        ///// </param>
        ///// <returns>
        ///// The <see cref="GameObject"/>.
        ///// </returns>
        ///// <exception cref="System.ArgumentOutOfRangeException">
        ///// Should not happen.
        ///// </exception>
        //private GameObject InstantiateTileAt(Tiles tileType, int x, int y)
        //{
        //    GameObject tile;

        //    // A function that maps Enum -> GameObject.
        //    switch (tileType)
        //    {
        //        case Tiles.Path:
        //            tile = this.tilePath;
        //            break;
        //        case Tiles.Grass:
        //            tile = this.tileA;
        //            break;
        //        case Tiles.Stone:
        //            tile = this.tileB;
        //            break;
        //        case Tiles.Wall:
        //            tile = this.tileC;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //    // Finally Instantiate it.
        //    var instance = Instantiate(
        //        tile, 
        //        new Vector3(x * CellSize, y * CellSize, 0.0f), 
        //        Quaternion.identity);

        //    instance.GetComponent<TileBlock>().SetSortingOrder(TG.Dimension - y);
        //    instance.transform.SetParent(this.tilesHolder);
        //    return instance;
        //}

        #endregion
    }
}
