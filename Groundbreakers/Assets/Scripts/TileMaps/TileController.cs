namespace TileMaps
{
    using System.Collections.Generic;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    ///     This component provide APIs to modify any tiles: swap, destruct, or construct.
    /// </summary>
    public class TileController : MonoBehaviour
    {
        /// <summary>
        ///     Contains a list of selected tiles. Should only have up to 2 elements.
        /// </summary>
        private readonly List<GameObject> selected = new List<GameObject>();

        private readonly List<GameObject> swappingTiles = new List<GameObject>();

        /// <summary>
        ///     Cached reference to the Tile map component.
        /// </summary>
        private Tilemap tilemap;

        private float previousTimeScale;

        public enum CommandState
        {
            Inactive,
            Swapping,
            Building,
            Deploying,
        }

        /// <summary>
        ///     Gets a value indicating whether if any tile is currently swapping.
        /// </summary>
        public static CommandState Active { get; private set; }

        #region For UI button only

        public void BeginInactive()
        {
            Active = CommandState.Inactive;
            this.previousTimeScale = 0.0f;
        }

        public void BeginBuild()
        {
            this.Begin(CommandState.Building);
        }

        public void BeginSwap()
        {
            this.Begin(CommandState.Swapping);
        }

        #endregion

        /// <summary>
        /// Start doing command
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        [Button]
        public void Begin(CommandState state)
        {
            if (Active == state)
            {
                Active = CommandState.Inactive;

                Time.timeScale = this.previousTimeScale;
                this.previousTimeScale = 0.0f;
                return;
            }

            Active = state;

            if (Mathf.Abs(this.previousTimeScale) < Mathf.Epsilon)
            {
                var t = Time.timeScale;
                Time.timeScale = 0.0f;
                this.previousTimeScale = t;
            }
        }

        /// <summary>
        ///     Clear the selected buffer.
        /// </summary>
        [Button]
        public void ClearSelected()
        {
            foreach (var go in this.selected)
            {
                go.GetComponent<TileStatus>().IsSelected = false;
            }

            this.selected.Clear();
        }

        public void SelectTile(Vector3 pos)
        {
            var block = this.tilemap.GetTileBlockAt(pos);

            Assert.IsNotNull(block);

            this.SelectTile(block);
        }

        public void SelectTile(GameObject tile)
        {
            if (this.selected.Contains(tile))
            {
                this.ClearSelected();
                return;
            }

            this.selected.Add(tile);

            if (this.selected.Count < 2)
            {
                return;
            }

            this.SwapSelectedTiles();
            // this.ClearSelected();
        }

        protected void OnEnable()
        {
            this.tilemap = this.GetComponent<Tilemap>();
            Active = CommandState.Inactive;
        }

        protected void Update()
        {
            // temp
            if (Input.GetMouseButtonDown(1))
            {
                this.ClearSelected();
            }

            if (Input.GetKeyDown("s"))
            {
                if (Active != CommandState.Swapping)
                {
                    this.Begin(CommandState.Inactive);
                }
                else
                {
                    this.Begin(CommandState.Swapping);
                }
            }
        }

        //private static void OnTilesChange(Vector3 first, Vector3 second)
        //{
        //    // TODO: Refactor this shit
        //    var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //    foreach (var enemy in enemies)
        //    {
        //        enemy.GetComponent<DynamicMovement>().OnTilesChange(first, second);
        //    }
        //}

        /// <summary>
        ///     Perform an swapping of tiles. Do animation, and swap references.
        /// </summary>
        /// <param name="first">
        ///     The first tile to swap.
        /// </param>
        /// <param name="second">
        ///     The second tile to swap.
        /// </param>
        private void SwapTiles(Vector3 first, Vector3 second)
        {
            var tileA = this.tilemap.GetTileBlockAt(first);
            var tileB = this.tilemap.GetTileBlockAt(second);

            this.swappingTiles.Clear();

            // Resetting the reference, temp
            this.tilemap.SetTileBlock(first, tileB.transform);
            this.tilemap.SetTileBlock(second, tileA.transform);

            this.MoveBlockTo(tileA, second);
            this.MoveBlockTo(tileB, first);
        }

        private void MoveBlockTo(GameObject tile, Vector3 destination)
        {
            // Update status
            var tileStatus = tile.GetComponent<TileStatus>();
            tileStatus.IsMoving = true;

            // Calculate travel path
            var origin = tile.transform.position;
            var liftHeight = new Vector3(0.0f, 1.0f, 1.0f);

            var path = new[] { origin + liftHeight, destination + liftHeight, destination };
            var durations = new[] { 0.2f, 0.6f, 0.4f };

            // Perform DOTween sequence
            var sequence = DOTween.Sequence();
            for (var i = 0; i < 2; i++)
            {
                sequence.Append(
                    tile.transform.DOLocalMove(path[i], durations[i])
                                  .SetUpdate(true));
            }

            sequence.Append(
                tile.transform.DOLocalMove(path[2], durations[2])
                              .SetEase(Ease.OutCubic)
                              .SetUpdate(true));

            sequence.SetUpdate(true);

            sequence.OnComplete(() => { this.OnSwapComplete(tile); });
        }

        private void OnSwapComplete(GameObject tile)
        {
            this.swappingTiles.Add(tile);

            if (this.swappingTiles.Count >= 2)
            {
                // Active = false;
                var first = this.swappingTiles[0];
                var second = this.swappingTiles[1];

                first.GetComponent<TileStatus>().IsMoving = false;
                second.GetComponent<TileStatus>().IsMoving = false;

                //OnTilesChange(first.transform.position, second.transform.position);
            }

            this.ClearSelected();
        }

        private void SwapSelectedTiles()
        {
            var first = this.selected[0];
            var second = this.selected[1];

            this.SwapTiles(first.transform.position, second.transform.position);
        }
    }
}