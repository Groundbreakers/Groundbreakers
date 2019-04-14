﻿namespace TileMaps
{
    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Provide a temporary player interface to debug, interacting with tiles.
    /// </summary>
    [RequireComponent(typeof(TileController))]
    public class DebugTileHover : MonoBehaviour
    {
        private SpriteRenderer rend;

        private TileController controller;

        private TileStatus status;

        private bool hovered;

        private void OnEnable()
        {
            var tilemap = GameObject.Find("Tilemap");
            
            this.rend = this.GetComponent<SpriteRenderer>();
            this.controller = tilemap.GetComponent<TileController>();
            this.status = this.GetComponent<TileStatus>();

            this.SetAlpha();
        }

        private void OnMouseOver()
        {
            this.hovered = true;
        }

        private void OnMouseExit()
        {
            this.hovered = false;
        }

        private void OnMouseUpAsButton()
        {
            this.status.IsSelected = true;

            this.controller.SelectTile(this.gameObject);
        }

        private void Update()
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

        private void OnDisable()
        {
            this.SetAlpha();
        }

        private void SetAlpha(float alpha = 1.0f)
        {
            var color = this.rend.color;
            this.rend.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}