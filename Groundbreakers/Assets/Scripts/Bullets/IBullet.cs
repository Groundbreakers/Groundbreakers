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
        /// <param name="handler">
        /// The Damage handler component that you wish to use in this bullet.
        /// </param>
        void Launch(Vector3 direction, DamageHandler handler);
    }
}