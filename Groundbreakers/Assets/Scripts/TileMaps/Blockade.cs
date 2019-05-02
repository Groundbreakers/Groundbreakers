namespace TileMaps
{
    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     This Component can be attached to GameObjects. When this GO is created, instantly
    ///     set the grid where this GO stands to Not passable. And restore its original state when
    ///     this destroy. 
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Blockade : MonoBehaviour
    {
        private SpriteRenderer sprite;

        private Tilemap gameMap;

        private bool previousCanPass;

        private void OnEnable()
        {
            // Inefficient but acceptable here.
            this.gameMap = GameObject.FindObjectOfType<Tilemap>();

            this.sprite = this.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            var parent = this.transform.parent;

            if (parent && parent.CompareTag("Tile"))
            {
                var status = parent.GetComponent<TileStatus>();

                this.previousCanPass = status.CanPass();
                status.SetCanPass(false);

                this.gameMap.OnTileChanges(parent.position);
            }

            //var position = this.transform.position;
            //var status = this.gameMap.GetTileStatusAt(position);

            //this.previousCanPass = status.CanPass();
            //status.SetCanPass(false);

            //// Notify the map to update
            //this.gameMap.OnTileChanges(position);
        }

        private void OnDestroy()
        {
            var position = this.transform.position;

            var status = this.gameMap.GetTileStatusAt(position);

            status.SetCanPass(this.previousCanPass);

            // Notify the map to update
            this.gameMap.OnTileChanges(position);
        }
    }
}