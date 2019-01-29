namespace Assets.Enemies.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using Assets.Scripts.Enemies;

    using UnityEngine;

    [RequireComponent(typeof(EnemyGeneric))]
    public class Fire_Bat_Script : MonoBehaviour
    {
        #region Variable Declarations

        private EnemyGeneric enemy;

        #endregion

        void Start()
        {
            enemy = this.GetComponent<EnemyGeneric>();
        }

        void Update()
        {
            // Passive effects go here.
        }
    }
}