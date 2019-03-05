namespace Assets.Scripts
{
    using UnityEngine;

    /// <inheritdoc cref="IBullet"/>
    /// <summary>
    /// The most basic bullet movement pattern: move toward one direction in Linear Motion.
    /// </summary>
    public class BulletMovement : MonoBehaviour, IBullet
    {
        #region Internal Fields

        /// <summary>
        /// Using this native structure to determine if the bullet should be killed.
        /// </summary>
        private static readonly Bounds ValidBounds = new Bounds(
            new Vector3(4.0f, 4.0f), new Vector3(10.0f, 10.0f));

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


        private static bool CheckOutOfBounds(Vector3 position)
        {
            var xy = new Vector3(position.x, position.y, 0.0f);

            return !ValidBounds.Contains(xy);
        }

        #endregion

        #region Unity Callbacks

        private void FixedUpdate()
        {
            const float Speed = 0.5f;

            this.transform.Translate(this.linearDirection * Speed);

            if (CheckOutOfBounds(this.transform.position))
            {
                GameObject.Destroy(this.gameObject);
            }
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