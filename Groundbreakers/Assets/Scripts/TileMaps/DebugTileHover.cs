namespace TileMaps
{
    using System;
    using System.Security.Cryptography.X509Certificates;

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
        private static readonly int TurnOn = Shader.PropertyToID("_TurnOn");

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

        public void SetAlpha(float alpha = 1.0f)
        {
            // TMP Fast FIX
            if (this.status.GetTileType() == Tiles.HighGround)
            {
                if (this.transform.childCount > 0)
                {
                    var child = this.transform.GetChild(0);
                    child.GetComponent<SpriteRenderer>().material.SetFloat(
                        TurnOn,
                        Math.Abs(alpha - 1.0f) < Mathf.Epsilon ? 0.0f : 1.0f);
                }
            }
            else
            {
                this.rend.material.SetFloat(
                    TurnOn, 
                    Math.Abs(alpha - 1.0f) < Mathf.Epsilon ? 0.0f : 1.0f);
            }
        }

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
            var active = TileController.Active != TileController.CommandState.Inactive;

            this.hovered = active && this.status.CanHover();
        }

        protected void OnMouseUpAsButton()
        {
            if (!this.hovered || TileController.Busy)
            {
                return;
            }

            switch (TileController.Active)
            {
                case TileController.CommandState.Inactive:
                    break;
                case TileController.CommandState.Swapping:

                    this.controller.SelectTile(this.gameObject);

                    break;
                case TileController.CommandState.Building:
                    if (this.status.CanPass())
                    {
                        var blockade = Instantiate(this.blockadePrefab, this.transform);
                    }
                    else
                    {
                        // Play bad SE
                    }

                    break;
                case TileController.CommandState.Deploying:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Update()
        {
            // Performance critical
            if (this.hovered || this.status.IsSelected)
            {
                this.SetAlpha(0.5f);
            }
            else
            {
                this.SetAlpha();
            }
        }
    }
}