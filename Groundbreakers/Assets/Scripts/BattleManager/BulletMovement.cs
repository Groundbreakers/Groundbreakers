namespace Assets.Scripts
{
    using UnityEngine;

    /// <summary>
    /// The most basic bullet movement pattern: move toward one direction in Linear Motion.
    /// </summary>
    public class BulletMovement : MonoBehaviour
    {
        #region Internal Fields

        /// <summary>
        /// Using this native structure to determine if the bullet should be killed.
        /// </summary>
        private static readonly Bounds ValidBounds = new Bounds(
            new Vector3(4.0f, 4.0f), new Vector3(8.0f, 8.0f));

        /// <summary>
        /// The linear direction of the movement of the bullet. 
        /// </summary>
        private Vector3 linearDirection;

        #endregion

        #region Public Functions    

        public void Launch(Vector3 initDirection)
        {
            this.linearDirection = initDirection.normalized;
        }

        #endregion

        #region Unity Callbacks

        private void FixedUpdate()
        {
            const float Speed = 0.1f;

            this.transform.Translate(this.linearDirection * Speed);

            // should update with speed factor, but eventually with more complicated equation
            if (!ValidBounds.Contains(this.transform.position))
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// The on collision enter 2 d.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            var go = other.gameObject;

            if (go.CompareTag("Enemy"))
            {
                this.HandleBulletHit(go);
            }
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Triggered when hit the target we wanted.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        private void HandleBulletHit(GameObject other)
        {
            GameObject.Destroy(this);
        }

        #endregion
    }
}