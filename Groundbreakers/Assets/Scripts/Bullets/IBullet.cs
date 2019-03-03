namespace Assets.Scripts
{
    using UnityEngine;

    public interface IBullet
    {
        /// <summary>
        /// The main entry method. Basically this gives an signal when the bullet is ready to be
        /// released. (Linear/Laser) 
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
        /// <param name="isMelee">
        /// The is Melee.
        /// </param>
        void HandleBulletHit(GameObject other, bool isMelee);
    }
}