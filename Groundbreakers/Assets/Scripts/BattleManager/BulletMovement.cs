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
            new Vector3(4.0f, 4.0f), new Vector3(8.0f, 8.0f));

        /// <summary>
        /// The linear direction of the movement of the bullet. 
        /// </summary>
        private Vector3 linearDirection;

        #endregion

        #region IBullet

        public void Launch(Vector3 initDirection)
        {
            this.linearDirection = initDirection.normalized;
        }

        public void HandleBulletHit(GameObject other)
        {
            Debug.Log("hit");
            GameObject.Destroy(this);
        }

        #endregion

        #region Unity Callbacks

        private void FixedUpdate()
        {
            const float Speed = 0.1f;

            var pos = this.transform.position;

            this.transform.Translate(this.linearDirection * Speed);

            // should update with speed factor, but eventually with more complicated equation
            if (pos.x < -1 || pos.x > 10 || pos.y < -1 || pos.y > 10)
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


        #endregion
    }
}