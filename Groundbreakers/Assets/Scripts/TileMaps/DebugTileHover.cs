namespace TileMaps
{
    using AI;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     Provide a temporary player interface to debug, interacting with tiles.
    /// </summary>
    [RequireComponent(typeof(TileController))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(TileStatus))]
    public class DebugTileHover : MonoBehaviour
    {
        private TileController controller;

        private SpriteRenderer rend;

        private TileStatus status;

        private Tilemap tilemap;

        [SerializeField]
        private GameObject tmpPrefab;

        [SerializeField]
        private GameObject blockadePrefab;

        /// <summary>
        ///     Indicating if this tile is currently hovered.
        /// </summary>
        private bool hovered;

        protected void OnDisable()
        {
            this.SetAlpha();
        }

        protected void OnEnable()
        {
            var tm = GameObject.Find("Tilemap");

            this.tilemap = tm.GetComponent<Tilemap>();
            this.rend = this.GetComponent<SpriteRenderer>();
            this.controller = this.tilemap.GetComponent<TileController>();
            this.status = this.GetComponent<TileStatus>();

            this.SetAlpha();
        }

        protected void OnMouseExit()
        {
            this.hovered = false;
        }

        protected void OnMouseOver()
        {
            this.hovered = this.status.CanHover();
        }

        protected void OnMouseUpAsButton()
        {
            if (!this.hovered)
            {
                return;
            }

            this.status.IsSelected = true;

            this.controller.SelectTile(this.gameObject);
        }

        protected void Update()
        {
            // Performance critical
            if (this.hovered || this.status.IsSelected)
            {
                this.SetAlpha(0.5f);

                if (Input.GetKeyDown("b"))
                {
                    var blockade = Instantiate(this.blockadePrefab, this.transform);
                }

                if (Input.GetKeyDown("1"))
                {
                    var pos = this.transform.position;
                    this.tilemap.ChangeTileAt(pos, Tiles.Grass);
                }

                if (Input.GetKeyDown("2"))
                {
                    var pos = this.transform.position;
                    this.tilemap.ChangeTileAt(pos, Tiles.Stone);
                }

                if (Input.GetKeyDown("3"))
                {
                    var pos = this.transform.position;
                    this.tilemap.ChangeTileAt(pos, Tiles.Water);
                }

            }
            else
            {
                this.SetAlpha();
            }
        }

        private void SetAlpha(float alpha = 1.0f)
        {
            var color = this.rend.color;
            this.rend.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}