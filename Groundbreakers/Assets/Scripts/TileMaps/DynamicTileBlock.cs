namespace TileMaps
{
    using System;
    using System.Collections.Generic;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     The dynamic tile block.
    /// </summary>
    public class DynamicTileBlock : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private GameObject highGroundPrefab;

        [Required]
        [SerializeField]
        private GameObject grassPrefab;

        private SpriteRenderer spriteRenderer;

        private Animator animator;

        private TileStatus status;

        // Temp
        [SerializeField]
        private List<Sprite> listSprites = new List<Sprite>();

        /// <summary>
        ///     Change the tile type, and update the sprite or animator.
        /// </summary>
        /// <param name="tileType">
        ///     The tile type.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid Tile Type
        /// </exception>
        [Button]
        public void ChangeTileType(Tiles tileType)
        {
            // Clear all children
            foreach (Transform child in this.transform)
            {
                var blockade = child.GetComponent<Blockade>();
                if (blockade)
                {
                    blockade.CreateRubble();
                }

                Destroy(child.gameObject);
            }

            this.animator.enabled = false;

            // A function that maps Enum -> GameObject
            switch (tileType)
            {
                case Tiles.Grass:
                    this.spriteRenderer.sprite = this.listSprites[0];
                    Instantiate(this.grassPrefab, this.transform);
                    break;
                case Tiles.Stone:
                    this.spriteRenderer.sprite = this.listSprites[1];
                    break;
                case Tiles.HighGround:
                    this.spriteRenderer.sprite = this.listSprites[2];
                    Instantiate(this.highGroundPrefab, this.transform);
                    break;
                case Tiles.Water:
                    this.spriteRenderer.sprite = this.listSprites[3];
                    this.animator.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.status.UpdateTileType(tileType);
        }

        private void OnEnable()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();
            this.status = this.GetComponent<TileStatus>();
        }

        /// <summary>
        ///     Change the Sprite of this tile base on its type when Start.
        /// </summary>
        private void Start()
        {
            var tileType = this.status.GetTileType();

            this.ChangeTileType(tileType);
        }
    }
}