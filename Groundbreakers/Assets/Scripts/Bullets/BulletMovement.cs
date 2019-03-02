namespace Assets.Scripts
{
    using Assets.Enemies.Scripts;

    using UnityEngine;

    using Random = UnityEngine.Random;

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

        // temp
        private characterAttributes attributes;

        #endregion

        #region Public Functions

        /// <summary>
        /// This method should be called with associated character attribute components when the
        /// bullet object is instantiated.
        /// </summary>
        /// <param name="characterAttributes">
        /// The character attributes.
        /// </param>
        public void SetCharacterAttribute(characterAttributes characterAttributes)
        {
            this.attributes = characterAttributes;
        }

        #endregion

        #region IBullet

        /// <summary>
        /// The main entry method. Basically this gives an signal when the bullet is ready to be
        /// released. (Linear/Laser) 
        /// </summary>
        /// <param name="direction">
        /// The direction shooting towards.
        /// </param>
        public void Launch(Vector3 direction)
        {
            this.linearDirection = direction.normalized;
        }

        /// <summary>
        /// Triggered when hit the target we wanted.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        public void HandleBulletHit(GameObject other)
        {
            GameObject.Destroy(this.gameObject);

            var damage = this.GetDamage();

            // temp solution
            other.GetComponent<Enemy_Generic>().DamageEnemy(
                damage.Pow, 
                damage.Amp, 
                1.0f, 
                false);
        }

        #endregion

        #region Internal Static Helpers

        /// <summary>
        /// In probability theory, the normal(or Gaussian or Gauss or Laplace–Gauss) distribution
        /// is a very common continuous probability distribution.
        /// See also
        /// <seealso cref="https://en.wikipedia.org/wiki/68%E2%80%9395%E2%80%9399.7_rule"/>
        /// </summary>
        /// <param name="mu">
        /// The Mean of the random variable.
        /// </param>
        /// <param name="sigma">
        /// The Standard deviation of the random variable.
        /// </param>
        /// <returns>
        /// The <see cref="float"/> standard normal(Gaussian) distribution.
        /// </returns>
        private static float GetGaussianRand(float mu, float sigma)
        {
            var rand1 = Random.Range(0.0f, 1.0f);
            var rand2 = Random.Range(0.0f, 1.0f);

            var n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

            return (mu + sigma * n);
        }

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
                this.HandleBulletHit(go);
            }
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Using this value to compute the actual damage *that is going to be delivered onto the
        /// enemies*. Note this is not necessary the final damage.
        /// </summary>
        /// <returns>
        /// The <see cref="Damage"/>.
        /// </returns>
        private Damage GetDamage()
        {
            const float FloatingDamage = 3.0f;
            var pow = this.attributes.POW;
            var amp = this.attributes.AMP;

            var damage = new Damage(
                Mathf.RoundToInt(pow * GetGaussianRand(50.0f, FloatingDamage)),
                amp);

            return damage;
        }

        /// <summary>
        /// A temporary structure that holds damage data in a nice way.
        /// </summary>
        private struct Damage
        {
            public readonly int Pow;

            public readonly int Amp;

            public Damage(int pow, int amp)
            {
                this.Pow = pow;
                this.Amp = amp;
            }
        }
        #endregion
    }
}