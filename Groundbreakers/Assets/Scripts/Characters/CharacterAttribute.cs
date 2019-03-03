namespace Characters
{
    using BattleManager;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// The main character attribute component rewritten by Ivan. This component serves a passive
    /// Information container of everything about character attributes. Please refer to the
    /// Public Property region for more detailed usage of the API.
    /// </summary>
    public class CharacterAttribute : MonoBehaviour
    {
        #region Inspector Values

        /// <summary>
        /// The attack effects.
        /// <see cref="AttackEffects"/>
        /// </summary>
        [SerializeField]
        [EnumToggleButtons]
        private AttackEffects attackEffects;

        /// <summary>
        /// The monster effects.
        /// <see cref="MonsterEffects"/>
        /// </summary>
        [SerializeField]
        [EnumToggleButtons]
        private MonsterEffects monsterEffects;

        [SerializeField]
        [Range(1, 10)]
        private int RNG;

        [SerializeField]
        [Range(1, 10)]
        private int ROF;

        [SerializeField]
        [Range(1, 10)]
        private int POW;

        [SerializeField]
        [Range(1, 10)]
        private int AMP;

        [SerializeField]
        [Range(1, 10)]
        private int MOB;

        #endregion

        #region Internal Fields

        private GameObject[] modules = new GameObject[6];

        private characterAttack attack;

        private Stance stance = Stance.Ranged;

        #endregion

        #region Public Properties

        private enum Stance
        {
            Ranged,
            Melee,
        }

        #endregion

        #region Public Function

        /// <summary>
        /// Updating the attributes based on the inventory items.
        /// </summary>
        /// <param name="inventory">
        /// Not sure how exactly this works, but seems like it contains the data we want.
        /// </param>
        public void UpdateAttributes(int[] inventory)
        {
            this.ResetStats();
            this.POW += inventory[0];
            this.ROF += inventory[1];
            this.RNG += inventory[2];
            this.MOB += inventory[3];
            this.AMP += inventory[4];
        }

        public void Transform()
        {
            if (this.stance == Stance.Ranged)
            {
                this.POW += 1;
                this.RNG -= 1;
                this.AMP += 1;
            }
            else
            {
                this.POW -= 1;
                this.RNG += 1;
                this.AMP -= 1;
            }
        }

        #endregion

        #region Unity Callback

        private void OnEnable()
        {
            this.attack = this.GetComponent<characterAttack>();
        }

        #endregion

        #region Internal Functions

        private void ResetStats()
        {
            if (this.stance == Stance.Ranged)
            {
                this.POW = 2;
                this.ROF = 2;
                this.RNG = 2;
                this.MOB = 2;
                this.AMP = 1;
            }
            else
            {
                this.POW = 3;
                this.ROF = 2;
                this.RNG = 1;
                this.MOB = 2;
                this.AMP = 2;
            }
        }

        private void SetStance(Stance newStance)
        {
            this.stance = newStance;

            if (newStance == Stance.Ranged)
            {
                this.POW = 2;
                this.RNG = 2;
                this.AMP = 1;
            }
            else
            {
                this.POW = 3;
                this.RNG = 1;
                this.AMP = 2;
            }
        }

        #endregion

        //public void updateAttributes(int[] inventory)
        //{
        //    // reset stats and update character attributes
        //    this.ResetStats();
        //    this.POW += inventory[0];
        //    this.ROF += inventory[1];
        //    this.RNG += inventory[2];
        //    this.MOB += inventory[3];
        //    this.AMP += inventory[4];

        //    var temp = new bool[17];
        //    for (var i = 5; i < inventory.Length; i++)
        //    {
        //        if (i > 0)
        //        {
        //            temp[i - 5] = true;
        //        }
        //        else
        //        {
        //            temp[i - 5] = false;
        //        }
        //    }

        //    // update effects
        //    this.burstAE = temp[0];
        //    this.ricochetAE = temp[1];
        //    this.laserAE = temp[2];
        //    this.splashAE = temp[3];
        //    this.pierceAE = temp[4];
        //    this.traceAE = temp[5];
        //    this.cleaveAE = temp[6];
        //    this.whirwindAE = temp[7];
        //    this.reachAE = temp[8];
        //    this.slowSE = temp[9];
        //    this.stunSE = temp[10];
        //    this.burnSE = temp[11];
        //    this.markSE = temp[12];
        //    this.purgeSE = temp[13];
        //    this.breakSE = temp[14];
        //    this.blightSE = temp[15];
        //    this.netSE = temp[16];
        //}
    }
}