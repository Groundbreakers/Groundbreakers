namespace Assets.Scripts
{
    using UnityEngine;

    /// <inheritdoc cref="IBullet" />
    /// <summary>
    ///     The most basic bullet movement pattern: move toward one direction in Linear Motion.
    /// </summary>
    [RequireComponent(typeof(OffScreenHandler))]
    public class BulletLinearMovement : MonoBehaviour, IBullet
    {
        private DamageHandler damageHandler;

        /// <summary>
        ///     The linear direction of the movement of the bullet.
        /// </summary>
        private Vector3 linearDirection;

        #region IBullet

        public void Launch(Transform target, DamageHandler handler)
        {
            var direction = target.position - this.transform.position;
            this.Launch(direction, handler);
        }

        public void Launch(Vector3 direction, DamageHandler handler)
        {
            this.linearDirection = direction.normalized;
            this.damageHandler = handler;
        }

        #endregion

        private void FixedUpdate()
        {
            const float Speed = 0.5f;

            this.transform.Translate(this.linearDirection * Speed);
        }

        /// <summary>
        ///     This method is triggered only if the collider is checked with "isTriggered"
        /// </summary>
        /// <param name="other">
        ///     The other.
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
    }
}