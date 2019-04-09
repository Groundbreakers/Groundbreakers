﻿namespace TileMaps
{
    using System;
    using System.Collections.Generic;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;

    using Random = UnityEngine.Random;

    /// <summary>
    ///     This component provide APIs to modify any tiles: swap, destruct, or construct.
    /// </summary>
    public class TileController : MonoBehaviour
    {
        private Tilemap tilemap;

        private List<GameObject> selected = new List<GameObject>();

        #region Public APIs

        [Button]
        public void DebugSwapTiles()
        {
            int x1, y1, x2, y2;

            do
            {
                x1 = Random.Range(0, TileData.Dimension);
                y1 = Random.Range(0, TileData.Dimension);
                x2 = Random.Range(0, TileData.Dimension);
                y2 = Random.Range(0, TileData.Dimension);
            }
            while (x1 == x2 && y1 == y2);

            this.SwapTiles(new Vector3(x1, y1), new Vector3(x2, y2));
        }

        public void SwapTiles(Vector3 first, Vector3 second)
        {
            var tileA = this.tilemap.GetTileBlockAt(first);
            var tileB = this.tilemap.GetTileBlockAt(second);

            // Resetting the reference, temp
            this.tilemap.SetTileBlock(first, tileB.transform);
            this.tilemap.SetTileBlock(second, tileA.transform);

            this.MoveBlockTo(tileA, second);
            this.MoveBlockTo(tileB, first);
        }

        public void SelectTile(GameObject tile)
        {
            this.selected.Add(tile);

            if (this.selected.Count >= 2)
            {
                this.SwapSelectedTiles();
                this.ClearSelected();
            }
        }

        [Button]
        public void ClearSelected()
        {
            foreach (var go in this.selected)
            {
                go.GetComponent<TileStatus>().IsSelected = false;
            }

            this.selected.Clear();
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.tilemap = this.GetComponent<Tilemap>();
        }

        private void Update()
        {
            // temp
            if (Input.GetMouseButtonDown(1))
            {
                this.ClearSelected();
            }
        }

        #endregion

        #region Internal Functions

        private void SwapSelectedTiles()
        {
            var first = this.selected[0];
            var second = this.selected[1];

            this.SwapTiles(first.transform.position, second.transform.position);
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
            var durations = new[] { 0.3f, 2.0f, 0.3f };

            // Perform DOTween sequence
            var sequence = DOTween.Sequence();
            for (var i = 0; i < 3; i++)
            {
                sequence.Append(tile.transform.DOLocalMove(path[i], durations[i]).SetEase(Ease.OutBack));
            }

            sequence.OnComplete(() => tileStatus.IsMoving = false);
        }

        #endregion
    }
}