namespace Core
{
    using System;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class InputSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject blockadePrefab;

        [SerializeField]
        private LayerMask tileLayer;

        private PartyManager party;

        private Camera mainCamera;

        private TileController tileController;

        private DynamicTerrainController terrainController;

        private Hoverable currentHovered;

        private void OnEnable()
        {
            Assert.IsNotNull(Camera.main);

            this.mainCamera = Camera.main;
            this.tileController = GameObject.FindObjectOfType<TileController>();
            this.terrainController = GameObject.FindObjectOfType<DynamicTerrainController>();
            this.party = GameObject.FindObjectOfType<PartyManager>();
        }

        private void Update()
        {
            this.ResetHovered();

            if (TileController.Busy)
            {
                return;
            }

            // Only update with hover if in Swapping/Building/Deploy mode.
            if (TileController.Active != TileController.CommandState.Inactive &&
                !this.terrainController.Busy)
            {
                this.UpdateWithHover();
            }

            this.HandleLeftClick();
            this.HandleRightClick();
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
                var hovered = target.GetComponent<Hoverable>();


                if (!this.AdditionalHoverCheck(target))
                {
                    return;
                }

                hovered.Hover();
                this.currentHovered = hovered;
            }
        }

        private bool AdditionalHoverCheck(GameObject tile)
        {
            var status = tile.GetComponent<TileStatus>();

            Assert.IsNotNull(status);

            var canHover = status.CanHover();
            switch (TileController.Active)
            {
                case TileController.CommandState.Inactive:
                    break;
                case TileController.CommandState.Swapping:
                    break;
                case TileController.CommandState.Building:
                    break;
                case TileController.CommandState.Deploying:

                    canHover = this.party.CanDeployAt(tile.transform);

                    break;
                case TileController.CommandState.Boooming:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return canHover;
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
                case TileController.CommandState.Boooming:
                    this.HandleBooom();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleRightClick()
        {
            if (!Input.GetMouseButtonDown(1))
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

                    if (this.tileController.HasSelected())
                    {
                        this.tileController.ClearSelected();
                    }
                    else
                    {
                        this.tileController.BeginInactive();
                    }

                    GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("TileError");

                    break;

                case TileController.CommandState.Boooming:
                case TileController.CommandState.Building:

                    this.tileController.ClearSelected();
                    this.tileController.BeginInactive();

                    GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("TileError");

                    break;
                case TileController.CommandState.Deploying:

                    this.party.DeselectCharacter();

                    this.tileController.BeginInactive();

                    GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("TileError");

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

            GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("TileDeploy");
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

                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("TileDeploy");
                this.terrainController.IncrementRiskLevel(0.15f);
            }
            else
            {
                // Play bad SE
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("TileError");
            }
        }

        private void HandleDeploying()
        {
            this.party.DeployCurrentCharacterAt(this.currentHovered.transform);

            GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("CharacterTransform");

            // Should unhover ground breaker buttons
        }

        private void HandleBooom()
        {
            var tile = this.currentHovered.transform.gameObject;

            this.tileController.BooomTile(tile);
        }

        #endregion
    }
}