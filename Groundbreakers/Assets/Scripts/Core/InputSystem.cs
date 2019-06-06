namespace Core
{
    using System;
    using System.Collections.Generic;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class InputSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject blockadePrefab;

        [SerializeField]
        private LayerMask tileLayer;

        [SerializeField]
        private GameObject playerManager;

        private Camera mainCamera;

        private TileController tileController;

        private Hoverable currentHovered;

        private void OnEnable()
        {
            Assert.IsNotNull(Camera.main);

            this.mainCamera = Camera.main;
            this.tileController = GameObject.FindObjectOfType<TileController>();
        }

        private void Update()
        {
            this.ResetHovered();

            if (TileController.Busy)
            {
                return;
            }

            // Only update with hover if in Swapping/Building/Deploy mode.
            if (TileController.Active != TileController.CommandState.Inactive)
            {
                this.UpdateWithHover();
            }

            this.HandleLeftClick();
        }

        private void ResetHovered()
        {
            // Reset selected
            if (!this.currentHovered)
            {
                return;
            }

            if (this.currentHovered.GetComponent<TileStatus>().IsSelected)
            {
                return;
            }

            this.currentHovered.Unhover();
            this.currentHovered = null;
        }

        private void UpdateWithHover()
        {
            // Perform Ray cast
            var hit = Physics2D.Raycast(
                this.mainCamera.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero,
                100,
                this.tileLayer);

            if (hit)
            {
                var target = hit.collider.gameObject;

                // Check can hover
                var status = target.GetComponent<TileStatus>();
                var hovered = target.GetComponent<Hoverable>();

                Assert.IsNotNull(status);

                if (!status.CanHover())
                {
                    return;
                }

                hovered.Hover();
                this.currentHovered = hovered;
            }
        }

        private void HandleLeftClick()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            if (!this.currentHovered)
            {
                return;
            }

            switch (TileController.Active)
            {
                case TileController.CommandState.Inactive:
                    break;
                case TileController.CommandState.Swapping:
                    this.HandleSwap();
                    break;
                case TileController.CommandState.Building:
                    this.HandleBuild();
                    break;
                case TileController.CommandState.Deploying:
                    this.HandleDeploying();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Commands

        private void HandleSwap()
        {
            var tile = this.currentHovered.transform.gameObject;

            this.tileController.SelectTile(tile);
        }

        private void HandleBuild()
        {
            var tile = this.currentHovered.transform.gameObject;
            var status = tile.GetComponent<TileStatus>();


            if (status.CanPass())
            {
                var blockade = Instantiate(
                    this.blockadePrefab,
                    tile.transform);
            }
            else
            {
                // Play bad SE
            }
        }

        private void HandleDeploying()
        {
            // var man = this.playerManager.GetComponent<PartyManager>();
            // man.DeployCurrentCharacterAt(this.currentHovered.transform);
        }

        #endregion
    }
}