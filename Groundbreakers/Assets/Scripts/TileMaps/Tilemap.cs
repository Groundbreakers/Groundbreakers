namespace TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(CustomTerrain))]
    public class Tilemap : MonoBehaviour
    {
        // below are all temp
        [SerializeField]
        private GameObject tileA;

        [SerializeField]
        private GameObject tileB;

        [SerializeField]
        private GameObject tileC;

        [SerializeField]
        private GameObject water;

        private Transform[,] blocks = new Transform[TileData.Dimension, TileData.Dimension];

        private ITerrainData mapData;

        #region Public API

        /// <summary>
        /// The setup map.
        /// </summary>
        [Button]
        public void SetupMap()
        {
            this.mapData = this.GetComponent<CustomTerrain>();
            this.mapData.Initialize();

            this.ClearAllTiles();
            this.InstantiateTiles(this.mapData);
        }

        private struct TempNode
        {
            public Vector3 Pos;
            public float F;
            public float G;

            public TempNode(Vector3 pos, float f = 0.0f, float g = 0.0f)
            {
                this.Pos = pos;
                this.F = f;
                this.G = g;
            } 
        }

        /// <summary>
        /// TODO: Improve Performance and clearness
        /// </summary>
        [Button]
        [InfoBox("Using A* to find the shortest path from Spawn A to Defend A")]
        public void DebugCalculatePath()
        {
            var start = GameObject.Find("Spawn Point A").transform.position;
            var end = GameObject.Find("Defend Point A").transform.position;

            var open = new List<TempNode> { new TempNode(start) };
            var closed = new List<TempNode>();
            var g = 0;

            TempNode current;
            while (open.Count > 0)
            {
                var lowest = open.Min(node => node.F);
                current = open.First(l => Math.Abs(l.F - lowest) < float.Epsilon);

                closed.Add(current);
                open.Remove(current);

                // check if is destination
                if (current.Pos == end)
                {
                    break;
                }

                var x = current.Pos.x;
                var y = current.Pos.y;

                var proposedLocations = new List<Vector3>
                                            {
                                                new Vector3(x + 1, y),
                                                new Vector3(x - 1, y),
                                                new Vector3(x, y + 1),
                                                new Vector3(x, y - 1)
                                            };

                // performance critical
                var adjacentTiles = proposedLocations.Where(
                    pos =>
                        {
                            var block = this.GetTileBlockAt(pos);
                            return block && block.GetComponent<TileStatus>().CanPass();
                        }).ToList();

                foreach (var tile in adjacentTiles)
                {
                    var a = closed.FirstOrDefault(node => node.Pos == tile);

                    var b = open.FirstOrDefault(node => node.Pos == tile);

                }
            }

            this.GetTileBlockAt(start);
        }

        /// <summary>
        /// Check if the map is passable at position grid.
        /// </summary>
        /// <param name="grid">
        /// The grid vector, the range should be within the bound of the map.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>. True if the position can pass.
        /// </returns>
        public bool IsMapPassable(Vector3 grid)
        {
            var type = this.mapData.GetTileTypeAt(grid.x, grid.y);

            // temp, currently only water/high ground is not passable.
            if (type == Tiles.Water)
            {
                return false;
            }

            return true;
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

        public void SetTileBlock(Vector3 position, Transform block)
        {
            var x = (int)position.x;
            var y = (int)position.y;

            this.blocks[x, y] = block;
        }

        #endregion

        private void ClearAllTiles()
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
        ///     Given the tile type, instantiate a GameObject from corresponding prefab at the
        ///     location (x,y).
        /// </summary>
        /// <param name="tileType">
        ///     The tile type <see cref="Tiles" />
        /// </param>
        /// <param name="x">
        ///     The X coordinate of the desired grid to place the tile.
        /// </param>
        /// <param name="y">
        ///     The Y coordinate of the desired grid to place the tile.
        /// </param>
        /// <returns>
        ///     The <see cref="GameObject" />.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Should not happen.
        /// </exception>
        private GameObject InstantiateTileAt(Tiles tileType, int x, int y)
        {
            GameObject tile;

            // A function that maps Enum -> GameObject.
            switch (tileType)
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
                case Tiles.Water:
                    tile = this.water;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Finally Instantiate it.
            var instance = Instantiate(
                tile, 
                new Vector3(x, y, 0.0f), 
                Quaternion.identity);

            // Setting order and parent
            instance.transform.SetParent(this.transform);

            instance.GetComponent<TileStatus>().UpdateTileType(tileType);

            return instance;
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
            for (var x = 0; x < TileData.Dimension; x++)
            {
                for (var y = 0; y < TileData.Dimension; y++)
                {
                    var tileType = sourceData.GetTileTypeAt(x, y);

                    var instance = this.InstantiateTileAt(tileType, x, y);

                    this.blocks[x, y] = instance.transform;
                }
            }
        }
    }
}