namespace TileMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <inheritdoc />
    /// <summary>
    ///     Container for actual tile GameObjects. Provide API to construct tiles, modify tiles.
    /// </summary>
    [RequireComponent(typeof(CustomTerrain))]
    [RequireComponent(typeof(NavigationMap))]
    public class Tilemap : MonoBehaviour, IEnumerable
    {
        private Transform[,] blocks = new Transform[Dimension, Dimension];

        private TileStatus[,] cachedStatus = new TileStatus[Dimension, Dimension];

        private NavigationMap navigationMap;

        private ITerrainData mapData;

        [SerializeField]
        private GameObject tilePrefab;

        // Temp
        [SerializeField]
        private GameObject mushroom;

        private List<GameObject> mushrooms = new List<GameObject>();

        /// <summary>
        ///     Gets the squared map's dimension
        /// </summary>
        public static int Dimension { get; } = 8;

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

        /// <summary>
        ///     The setup map.
        /// </summary>
        [Button]
        public void SetupMap()
        {
            this.mapData.Initialize();

            this.ClearAllTiles();
            this.InstantiateTiles(this.mapData);
            this.InstantiateEnvironments();
        }

        public void OnTileChanges()
        {
        }

        public void OnTileOccupied(Vector3 pos, bool status = true)
        {
            var block = this.GetTileStatusAt(pos);

            block.IsOccupied = status;
        }

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
            this.mapData = this.GetComponent<CustomTerrain>();
            this.navigationMap = this.GetComponent<NavigationMap>();
        }

        private void ClearAllTiles()
        {
            var tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (var go in tiles)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(go);
                }
                else
                {
                    Destroy(go);
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
            var num = 4;
            this.mushrooms.Clear();

            // We use naive approach here
            for (var i = 0; i < num; i++)
            {
                bool duplicate;
                Transform block;
                do
                {
                    block = this.PickNonOcuppiedBlock(this.mapData);
                    duplicate = this.mushrooms.Any(go => go.transform.position.Equals(block.position));
                }
                while (duplicate);

                // Debug.Log("Block Location: " + block.position.x + " " + block.position.y);
                var mush = Instantiate(this.mushroom, block);
                mush.transform.localPosition = Vector3.zero;
                this.mushrooms.Add(mush);

                // Manually set these tile to undeployable
                block.GetComponent<TileStatus>().SetCanDeploy(false);
            }
        }

        private Transform PickNonOcuppiedBlock(ITerrainData sourceData)
        {
            int x;
            int y;
            do
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
            }
            while (sourceData.GetTileTypeAt(x, y) == Tiles.Water);

            return this.blocks[x, y];
        }
    }
}