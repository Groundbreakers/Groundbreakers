namespace Assets.Enemies.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    [RequireComponent(typeof(Enemy_Generic))]
    public class Fire_Bat_Script : MonoBehaviour
    {
        #region Variable Declarations

        private Enemy_Generic enemy;

        #endregion

        void Start()
        {
            enemy = this.GetComponent<Enemy_Generic>();
        }

        void Update()
        {
            // Passive effects go here.
        }
    }
}