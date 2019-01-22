namespace Assets.Enemies.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    [RequireComponent(typeof(Enemy_Generic))]
    public class Fire_Bat_Script : MonoBehaviour
    {
        #region Variable Declarations

        /// <summary>
        /// Enemy Stats
        /// </summary>
        private int health = 5;
        private float speed = 1f;
        private int power = 1;
        private string[] attributes = new string[] { "Armored", "Flying" };
        private Enemy_Generic enemy;

        #endregion

        void Start()
        {
            enemy = this.GetComponent<Enemy_Generic>();
            this.EnemyInit();
        }

        void Update()
        {
            // Passive effects go here.
        }

        // Initialization script
        void EnemyInit()
        {
            this.enemy.maxHealth = this.health;
            this.enemy.health = this.health;
            this.enemy.speed = this.speed;
            this.enemy.power = this.power;
            for (int i = 0; i < this.attributes.Length; i++)
            {
                this.enemy.attributes.Add(this.attributes[i]);
            }
        }
    }
}