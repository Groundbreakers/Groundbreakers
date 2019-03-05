namespace Assets.Scripts
{
    using Assets.Enemies.Scripts;

    using UnityEngine;

    /// <summary>
    /// An external Damage Handler, Please refer to the 'Public APIs' regions to detailed usage.
    /// </summary>
    public class DamageHandler: MonoBehaviour
    {
        #region Internal Fields

        private characterAttributes attributes;

        #endregion

        #region Public APIs

        /// <summary>
        ///     Triggered when hit the target we wanted.
        /// </summary>
        /// <param name="other">
        ///     The other.
        /// </param>
        /// <param name="isMelee">
        ///     The is Melee.
        /// </param>
        public void DeliverDamageTo(GameObject other, bool isMelee = false)
        {
            var damage = this.GetDamage();

            // temp solution
            other.GetComponent<Enemy_Generic>().DamageEnemy(
                damage.Pow, damage.Amp, 1.0f, isMelee, false);
        }

        /// <summary>
        ///     This method should be called with associated character attribute components when the
        ///     bullet object is instantiated.
        /// </summary>
        /// <param name="characterAttributes">
        ///     The character attributes.
        /// </param>
        public void SetCharacterAttribute(characterAttributes characterAttributes)
        {
            this.attributes = characterAttributes;
        }

        #endregion

        #region Internal Functions

        /// <summary>
        ///     In probability theory, the normal(or Gaussian or Gauss or Laplace–Gauss) distribution
        ///     is a very common continuous probability distribution.
        ///     See also
        ///     <seealso cref="https://en.wikipedia.org/wiki/68%E2%80%9395%E2%80%9399.7_rule" />
        /// </summary>
        /// <param name="mu">
        ///     The Mean of the random variable.
        /// </param>
        /// <param name="sigma">
        ///     The Standard deviation of the random variable.
        /// </param>
        /// <returns>
        ///     The <see cref="float" /> standard normal(Gaussian) distribution.
        /// </returns>
        private static float GetGaussianRand(float mu, float sigma)
        {
            var rand1 = Random.Range(0.0f, 1.0f);
            var rand2 = Random.Range(0.0f, 1.0f);

            var n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos(2.0f * Mathf.PI * rand2);

            return mu + sigma * n;
        }

        /// <summary>
        ///     Using this value to compute the actual damage *that is going to be delivered on the
        ///     enemies*. Note this is not necessary the final damage.
        /// </summary>
        /// <returns>
        ///     The <see cref="Damage" />.
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
        ///     A temporary structure that holds damage data in a nice way.
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