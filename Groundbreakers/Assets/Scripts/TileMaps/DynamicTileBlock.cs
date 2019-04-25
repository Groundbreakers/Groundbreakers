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
            this.animator.enabled = false;

            // A function that maps Enum -> GameObject
            switch (tileType)
            {
                case Tiles.Grass:
                    this.spriteRenderer.sprite = this.listSprites[0];
                    break;
                case Tiles.Stone:
                    this.spriteRenderer.sprite = this.listSprites[1];
                    break;
                case Tiles.Wall:
                    this.spriteRenderer.sprite = this.listSprites[2];
                    break;
                case Tiles.Water:
                    this.spriteRenderer.sprite = this.listSprites[3];
                    this.animator.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void OnEnable()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();
            this.status = this.GetComponent<TileStatus>();
        }

        /// <summary>
        ///     Change the Sprite of this tile base on its type when Start.
        /// </summary>
        protected void Start()
        {
            var tileType = this.status.GetTileType();

            this.ChangeTileType(tileType);
        }
    }
}