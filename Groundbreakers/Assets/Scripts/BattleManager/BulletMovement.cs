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
            this.transform.Translate(this.linearDirection);

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
                this.HandleBulletHit();
            }
        }

        #endregion

        #region Internal Functions

        private void HandleBulletHit()
        {
            GameObject.Destroy(this);
        }

        #endregion
    }
}