namespace TileMaps
{
    using System.Collections;

    using AI;

    using CombatManager;

    using Sirenix.OdinInspector;

    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <inheritdoc cref="IEnumerable" />
    /// <summary>
    ///     Container for actual tile GameObjects. Provide API to construct tiles, modify tiles.
    /// </summary>
    [RequireComponent(typeof(ITerrainData))]
    [RequireComponent(typeof(NavigationMap))]
    public class Tilemap : MonoBehaviour, IEnumerable
    {
        private Transform[,] blocks = new Transform[Dimension, Dimension];

        private TileStatus[,] cachedStatus = new TileStatus[Dimension, Dimension];

        /// <summary>
        ///     The source of the map data. Pure data. Should setup the blocks according to this.
        /// </summary>
        private ITerrainData mapData;

        [SerializeField]
        private GameObject tilePrefab;

        // Temp
        [SerializeField]
        private GameObject mushroomPrefab;

        [SerializeField]
        private GameObject plantPrefab;

        [SerializeField]
        private GameObject highGroundPrefab;

        /// <summary>
        ///     Gets the squared map's dimension
        /// </summary>
        public static int Dimension { get; } = 8;

        public static int CountNumHighGrounds(ITerrainData data)
        {
            var num = 0;

            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var type = data.GetTileTypeAt(i, j);

                    if (type == Tiles.HighGround)
                    {
                        num++;
                    }
                }
            }

            return num;
        }

        /// <summary>
        ///     The get blockade at.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="GameObject" />. Might return null if no blockade. 
        /// </returns>
        public GameObject GetBlockadeAt(Vector3 position)
        {
            var parent = this.GetTileBlockAt(position);

            var blockade = parent.GetComponentInChildren<Blockade>();

            return blockade ? blockade.gameObject : null;
        }

        public GameObject GetTileBlockAt(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            return this.GetTileBlockAt(x, y);
        }

        public GameObject GetTileBlockAt(int x, int y)
        {
            return this.blocks[x, y].gameObject;
        }

        public TileStatus GetTileStatusAt(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            return this.GetTileStatusAt(x, y);
        }

        /// <summary>
        ///     Get the cached tile status at position x, y.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <returns>
        ///     The <see cref="TileStatus" />.
        /// </returns>
        public TileStatus GetTileStatusAt(int x, int y)
        {
            return this.cachedStatus[x, y];
        }

        public void SetTileBlock(Vector3 position, Transform block)
        {
            var x = (int)position.x;
            var y = (int)position.y;
            var index = (x * Dimension) + y;

            this.blocks[x, y] = block;
            this.cachedStatus[x, y] = block.GetComponent<TileStatus>();

            block.SetSiblingIndex(index);
        }

        public void ChangeTileAt(Vector3 position, Tiles type)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            var dynamicTile = this.blocks[x, y].GetComponent<DynamicTileBlock>();

            dynamicTile.ChangeTileType(type);
        }

        /// <summary>
        ///     The setup map.
        /// </summary>
        [Button]
        public void SetupMap()
        {
            this.mapData = this.GetComponent<ITerrainData>();

            do
            {
                this.mapData.Initialize();
            }
            while (CountNumHighGrounds(this.mapData) < 5);

            ClearAllTiles();
            this.InstantiateTiles(this.mapData);
            this.InstantiateEnvironments();
        }

        public void OnTileOccupied(Vector3 pos, bool status = true)
        {
            var block = this.GetTileStatusAt(pos);

            block.IsOccupied = status;
        }

        /// <summary>
        ///     Usage: for (var block of this.tileMap) { ... };
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            for (var x = 0; x < Dimension; x++)
            {
                for (var y = 0; y < Dimension; y++)
                {
                    yield return this.GetTileBlockAt(x, y);
                }
            }
        }

        protected void OnEnable()
        {
            this.mapData = this.GetComponent<ITerrainData>();
        }

        /// <summary>
        ///     Immediately destroy all tile GameObjects in the scene.
        /// </summary>
        private static void ClearAllTiles()
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (var go in tiles)
            {
                if (Application.isEditor)
                {
                    GameObject.DestroyImmediate(go);
                }
                else
                {
                    GameObject.Destroy(go);
                }
            }
        }

        /// <summary>
        ///     Instantiate all tiles. Must call generator.Initialize before using this function.
        ///     This function will destroy any existing tileBlock GameObjects.
        /// </summary>
        /// <param name="sourceData">
        ///     The source Data.
        /// </param>
        private void InstantiateTiles(ITerrainData sourceData)
        {
            // Re instantiate all tiles
            for (var x = 0; x < Dimension; x++)
            {
                for (var y = 0; y < Dimension; y++)
                {
                    var tileType = sourceData.GetTileTypeAt(x, y);

                    var instance = Instantiate(this.tilePrefab, new Vector3(x, y), Quaternion.identity);
                    instance.transform.SetParent(this.transform);

                    var status = instance.GetComponent<TileStatus>();
                    status.UpdateTileType(tileType);

                    // Store the references in 2D array
                    this.blocks[x, y] = instance.transform;
                    this.cachedStatus[x, y] = status;
                }
            }
        }

        /// <summary>
        ///     Instantiating fast and dirty but works fine environmental objects.
        /// </summary>
        private void InstantiateEnvironments()
        {
            this.SpawnMushrooms();
            this.SpawnGrassAndHighGround();
        }

        private void SpawnMushrooms()
        {
            foreach (var pos in this.mapData.GetMushroomLocations())
            {
                var instance = Instantiate(
                    this.mushroomPrefab,
                    this.GetTileBlockAt(pos).transform);
            }
        }

        private void SpawnGrassAndHighGround()
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                   var status = this.GetTileStatusAt(i, j);

                   if (status.GetTileType() == Tiles.Grass)
                   {
                       var instance = Instantiate(
                           this.plantPrefab, 
                           this.GetTileBlockAt(i, j).transform);
                   }

                   if (status.GetTileType() == Tiles.HighGround)
                   {
                       var instance = Instantiate(
                           this.highGroundPrefab,
                           this.GetTileBlockAt(i, j).transform);
                   }
                }
            }
        }
    }
}