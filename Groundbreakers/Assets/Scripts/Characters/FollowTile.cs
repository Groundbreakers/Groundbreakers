namespace Characters
{
    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     Make the GameObject position copy of the specific tile.
    /// </summary>
    public class FollowTile : MonoBehaviour
    {
        [ShowInInspector]
        private Transform target;

        public void AffiliateTile(Transform tile)
        {
            this.target = tile;
        }

        private void Update()
        {
            if (this.target)
            {
                this.transform.position = this.target.position;
            }
        }
    }
}