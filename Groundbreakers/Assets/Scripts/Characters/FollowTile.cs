namespace Characters
{
    using System;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     Make the GameObject position copy of the specific tile.
    /// </summary>
    public class FollowTile : MonoBehaviour
    {
        [ShowInInspector]
        private Transform target;

        private SpriteRenderer rend;

        private TileStatus status;

        public void AffiliateTile(Transform tile)
        {
            this.target = tile;
            this.status = tile.GetComponent<TileStatus>();
        }

        private void OnEnable()
        {
            this.rend = this.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (this.target)
            {
                this.transform.position = this.target.position;

                this.rend.sortingLayerName = this.status.IsMoving ? "HUD" : "Mobs";
            }
        }
    }
}