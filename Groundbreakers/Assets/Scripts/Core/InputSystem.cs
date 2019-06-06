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
        private static readonly int TurnOn = Shader.PropertyToID("_TurnOn");

        private Camera mainCamera;

        private List<GameObject> buffer = new List<GameObject>();

        private TileController tileController;

        [SerializeField]
        private GameObject blockadePrefab;

        [SerializeField]
        private LayerMask tileLayer;

        [SerializeField]
        private GameObject playerManager;

        private Hoverable lastHovered;

        /// <summary>
        ///     Set the tile to high light or not.
        /// </summary>
        /// <param name="tileObject">
        ///     The tile object to be determined. 
        /// </param>
        /// <param name="value">
        ///     The value. True = On, False = Off
        /// </param>
        private static void SetHighLight(GameObject tileObject, bool value)
        {
            var status = tileObject.GetComponent<TileStatus>();
            var rend = tileObject.GetComponent<SpriteRenderer>();

            // TMP Fast FIX
            if (status.GetTileType() == Tiles.HighGround)
            {
                var trans = tileObject.transform;

                if (trans.childCount > 0)
                {
                    var child = trans.GetChild(0);
                    child.GetComponent<SpriteRenderer>().material.SetFloat(TurnOn, value ? 1.0f : 0.0f);
                }
            }
            else
            {
                rend.material.SetFloat(TurnOn, value ? 1.0f : 0.0f);
            }
        }

        private void OnEnable()
        {
            Assert.IsNotNull(Camera.main);

            this.mainCamera = Camera.main;
            this.tileController = FindObjectOfType<TileController>();
        }

        private void Update()
        {
            // Reset selected
            if (this.lastHovered)
            {
                if (!this.lastHovered.GetComponent<TileStatus>().IsSelected)
                {
                    this.lastHovered.Unhover();
                    this.lastHovered = null;
                }
            }

            // Do nothing if busy
            if (TileController.Busy)
            {
                return;
            }

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
                this.lastHovered = hovered;
            }

            this.HandleLeftClick();
        }

        private void HandleLeftClick()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            if (!this.lastHovered)
            {
                return;
            }

            // var man = this.playerManager.GetComponent<PartyManager>();
            // man.DeployCurrentCharacterAt(this.currentHovered.transform);

            // SetHighLight(this.currentHovered, true);
            var tile = this.lastHovered.transform.gameObject;

            this.tileController.SelectTile(tile);
        }
    }
}