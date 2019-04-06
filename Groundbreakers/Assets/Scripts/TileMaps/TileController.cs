namespace TileMaps
{
    using System.Collections;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <summary>
    ///     This component provide APIs to modify any tiles: swap, destruct, or construct.
    /// </summary>
    public class TileController : MonoBehaviour
    {
        private Tilemap tilemap;

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

            this.StartCoroutine(this.MoveBlockTo(tileA, second));
            this.StartCoroutine(this.MoveBlockTo(tileB, first));
        }

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            this.tilemap = this.GetComponent<Tilemap>();
        }

        #endregion

        #region Internal Functions

        private IEnumerator MoveBlockTo(GameObject tile, Vector3 destination)
        {
            var duration = 0.35f;
            var newPos = tile.transform.position + new Vector3(0.0f, 1.0f);

            tile.transform.DOMove(destination, duration).SetEase(Ease.OutBack);

            yield return null;
        }

        #endregion
    }
}