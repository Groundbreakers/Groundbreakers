namespace BattleManager
{
    using UnityEngine;

    /// <summary>
    /// The most basic bullet movement pattern: move toward one direction in Linear Motion.
    /// </summary>
    public class BulletMovement : MonoBehaviour
    {
        #region Internal Fields

        private Vector3 linearDirection;

        #endregion

        #region Public Functions    

        public void Launch(Vector3 initDirection)
        {
            this.linearDirection = initDirection;
        }

        #endregion

        #region Unity Callbacks

        private void FixedUpdate()
        {
            this.transform.Translate(this.linearDirection);
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