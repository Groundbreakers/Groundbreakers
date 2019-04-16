namespace TileMaps
{
    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     The passive data component that contains some status of this current tile GameObject.
    /// </summary>
    public class TileStatus : MonoBehaviour
    {
        [ShowInInspector]
        private bool canDeploy;

        private Tiles type;

        [field: ShowInInspector]
        [field: ReadOnly]
        public bool IsMoving { private get; set; }

        [field: ShowInInspector]
        [field: ReadOnly]
        public bool IsSelected { get; set; }

        public bool CanHover()
        {
            return this.canDeploy && !this.IsMoving;
        }

        public bool CanPass()
        {
            return this.canDeploy && !this.IsMoving;
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

            // temp
            this.canDeploy = tileType != Tiles.Water;
        }
    }
}