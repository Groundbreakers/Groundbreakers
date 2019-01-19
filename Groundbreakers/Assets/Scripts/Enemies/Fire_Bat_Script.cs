namespace Assets.Enemies.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    public class Fire_Bat_Script : MonoBehaviour
    {
        #region Variable Declarations

        /// <summary>
        /// Death effect object
        /// </summary>
        public GameObject deathEffect;

        /// <summary>
        /// Waypoint-storing object
        /// </summary>
        private Transform target;

        /// <summary>
        /// Enemy Stats
        /// </summary>
        private float strength = 1;
        private float health = 1;
        private float speed = 1f;
        private int damage = 1;

        /// <summary>
        /// Current waypoint based on waypoint array
        /// </summary>
        private int waypointIndex = 0;

        #endregion

        void Start()
        {
            this.target = Waypoints.points[0];
        }

        void Update()
        {
            // Move the enemy towards the target waypoint
            Vector2 dir = this.target.position - this.transform.position;
            this.transform.Translate(dir.normalized * this.speed * Time.deltaTime, Space.World);

            // Check if the waypoint has been reached
            if (Vector2.Distance(this.transform.position, this.target.position) <= 0.01f)
            {
                this.GetNextWaypoint();
            }

            // Check health, die if health <= 0. Spawns a 'death effect'.
            if (this.health <= 0)
            {
                GameObject effect = (GameObject)Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);
                Destroy(effect, 1);
                Destroy(this.gameObject);
            }
        }

        // Collision detector for projectiles
        void OnCollisionEnter(Collision col)
        {
            // If projectile, destroy the projectile and do damage calculation. Firebat is ARMORED, and takes damage from armor penetration.
            if (col.gameObject.tag == "Projectile")
            {
                Destroy(col.gameObject);
                ////+Change this line to the proper projectile script+ health -= col.gameObject."ProjectileScript".armorpen
            }
        }

        // Get the next waypoint and update the index
        void GetNextWaypoint()
        {
            if (this.waypointIndex >= Waypoints.points.Length - 1)
            {
                this.EndPath();
                return;
            }

            this.waypointIndex++;
            this.target = Waypoints.points[this.waypointIndex];
        }

        // When the enemy reaches the end of its path
        void EndPath()
        {
            HP.healthPoint -= this.damage;
            Destroy(this.gameObject);
        }
    }
}