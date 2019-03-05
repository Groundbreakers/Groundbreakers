namespace Assets.Scripts
{
    using UnityEngine;

    /// <inheritdoc cref="IBullet"/>
    /// <summary>
    /// The most basic bullet movement pattern: move toward one direction in Linear Motion.
    /// </summary>
    [RequireComponent(typeof(OffScreenHandler))]
    public class BulletMovement : MonoBehaviour, IBullet
    {
        #region Internal Fields

        /// <summary>
        /// The linear direction of the movement of the bullet. 
        /// </summary>
        private Vector3 linearDirection;

        private DamageHandler damageHandler;

        #endregion

        #region IBullet

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
        public void Launch(Vector3 direction, DamageHandler handler)
        {
            this.linearDirection = direction.normalized;
            this.damageHandler = handler;
        }

        #endregion

        #region Internal Static Helpers

        #endregion

        #region Unity Callbacks

        private void FixedUpdate()
        {
            const float Speed = 0.5f;

            this.transform.Translate(this.linearDirection * Speed);
        }

        /// <summary>
        /// This method is triggered only if the collider is checked with "isTriggered"
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;

            if (go.CompareTag("Enemy"))
            {
                this.damageHandler.DeliverDamageTo(go);
                GameObject.Destroy(this.gameObject);
            }
        }

        #endregion

    }
}