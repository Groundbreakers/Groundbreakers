namespace Assets.Scripts
{
    using UnityEngine;

    public interface IBullet
    {
        /// <summary>
        /// The main entry point. 
        /// </summary>
        /// <param name="direction">
        /// The direction shooting towards.
        /// </param>
        void Launch(Vector3 direction);

        /// <summary>
        /// Triggered when hit the target we wanted.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        void HandleBulletHit(GameObject other);
    }
}