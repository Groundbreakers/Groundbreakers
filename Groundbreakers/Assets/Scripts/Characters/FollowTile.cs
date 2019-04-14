namespace Characters
{
    using Sirenix.OdinInspector;

    using UnityEngine;

    public class FollowTile : MonoBehaviour
    {
        [ShowInInspector]
        private Transform target;

        public void AffiliateTile(Transform tile)
        {
            this.target = tile;
        }

        private void FixedUpdate()
        {
            if (this.target)
            {
                this.transform.position = this.target.position;
            }
        }
    }
}