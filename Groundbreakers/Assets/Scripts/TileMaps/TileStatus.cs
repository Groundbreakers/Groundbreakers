namespace TileMaps
{
    using System;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     The passive data component that contains some status of this current Tile GameObject.
    /// </summary>
    public class TileStatus : MonoBehaviour
    {
        /// <summary>
        ///     Indicating if this tile can pass through.
        /// </summary>
        [SerializeField]
        private bool canPass;

        /// <summary>
        ///     Indicating if the player can select or swap this tiles. Currently, only Water tile
        ///     is not selectable.
        /// </summary>
        [SerializeField]
        private bool canSwap;

        [SerializeField]
        private Tiles type;

        [field: ShowInInspector]
        [field: ReadOnly]
        public bool IsMoving { private get; set; }

        [field: ShowInInspector]
        [field: ReadOnly]
        public bool IsSelected { get; set; }

        public bool IsOccupied { get; set; }

        /// <summary>
        ///     Gets or sets the weight; Should be used only by the navMesh when doing A*.
        /// </summary>
        public float Weight { get; set; }

        public Tiles GetTileType()
        {
            return this.type;
        }

        /// <summary>
        ///     Indicating if this tile is selectable. Used by the DebugHover.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool CanHover()
        {
            return this.canSwap && !this.IsMoving && !this.IsOccupied;
        }

        /// <summary>
        ///     Used by the Search Algorithm and character deployment component, to check if this
        ///     tile can pass.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool CanPass()
        {
            return this.canPass && !this.IsMoving;
        }

        public void SetCanPass(bool value)
        {
            this.canPass = value;
        }

        /// <summary>
        ///     This function is called when instantiate or on tile type has been changed. Should
        ///     update the status such as if can deploy.
        /// </summary>
        /// <param name="tileType">
        ///     The type.
        /// </param>
        public void UpdateTileType(Tiles tileType)
        {
            this.type = tileType;

            // TMP
            switch (tileType)
            {
                case Tiles.Grass:
                case Tiles.Stone:
                    this.canPass = true;
                    this.canSwap = true;
                    break;
                case Tiles.Wall:
                    this.canPass = false;
                    this.canSwap = true;
                    break;
                case Tiles.Water:
                    this.canPass = false;
                    this.canSwap = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
            }

        }
    }
}