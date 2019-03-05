namespace Assets.Scripts
{
    using UnityEngine;

    public interface IBullet
    {
        /// <summary>
        ///     The main entry method. Basically this gives an signal when the bullet is ready to be
        ///     released. (Linear/Laser)
        /// </summary>
        /// <param name="target">
        ///     The target Transform(typically )
        /// </param>
        /// <param name="handler">
        ///     The Damage handler component that you wish to use in this bullet.
        /// </param>
        void Launch(Transform target, DamageHandler handler);

        /// <summary>
        ///     Same as above. However shoot towards a linear direction.
        /// </summary>
        /// <param name="direction">
        ///     The direction.
        /// </param>
        /// <param name="handler">
        ///     The handler.
        /// </param>
        void Launch(Vector3 direction, DamageHandler handler);
    }
}