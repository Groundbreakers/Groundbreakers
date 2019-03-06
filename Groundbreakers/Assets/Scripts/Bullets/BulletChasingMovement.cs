namespace Assets.Scripts
{
    using System;

    using UnityEngine;

    /// <inheritdoc cref="IBullet" />
    /// <summary>
    ///     This components makes the bullet chase the target. If the target dies before we
    ///     hit them. Then move linearly. 
    /// </summary>
    [RequireComponent(typeof(OffScreenHandler))]
    public class BulletChasingMovement : MonoBehaviour, IBullet
    {
        private DamageHandler damageHandler;

        /// <summary>
        /// The target Transform
        /// </summary>
        private Transform target;

        private Vector3 lastDirection = Vector3.zero;

        #region IBullet

        public void Launch(Transform target, DamageHandler handler)
        {
            this.target = target;

            this.damageHandler = handler;
        }

        public void Launch(Vector3 direction, DamageHandler handler)
        {
            this.lastDirection = direction;
        }

        #endregion

        private void FixedUpdate()
        {
            const float Speed = 0.1f;

            if (this.target)
            {
                var targetPosition = this.target.position;

                this.transform.LookAt(targetPosition);
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position,
                    targetPosition,
                    Speed);

                this.lastDirection = this.target.position - this.transform.position;
            }
            else
            {
                this.transform.Translate(this.lastDirection * Speed);
            }
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