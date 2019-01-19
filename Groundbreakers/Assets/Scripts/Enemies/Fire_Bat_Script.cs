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
        /// Animator declaration
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Enemy Stats
        /// </summary>
        private float strength = 1;
        private float health = 1;
        private float speed = 1f;
        private int damage = 1;

        /// <summary>
        /// Status effects
        /// </summary>
        private bool isStunned = false;
        private float stunTime = 0;

        /// <summary>
        /// Current waypoint based on waypoint array
        /// </summary>
        private int waypointIndex = -1;

        #endregion

        void Start()
        {
            this.GetNextWaypoint();
            this.animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            // Check health, die if health <= 0. Spawns a 'death effect'.
            if (this.health <= 0)
            {
                GameObject effect = (GameObject)Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);
                Destroy(effect, 1);
                Destroy(this.gameObject);
            }

            if (!this.isStunned)
            {
                // Move the enemy towards the target waypoint
                Vector2 dir = this.target.position - this.transform.position;
                this.transform.Translate(dir.normalized * this.speed * Time.deltaTime, Space.World);

                // Check if the waypoint has been reached
                if (Vector2.Distance(this.transform.position, this.target.position) <= 0.01f)
                {
                    this.GetNextWaypoint();
                }
            }
        }

        // Stun handlers
        void StunEnemy(float time)
        {
            this.isStunned = true;
            this.animator.SetBool("Stun",true);
            this.stunTime = time;
            this.StartCoroutine(this.StunTick());
        }

        IEnumerator StunTick()
        {
            this.stunTime -= Time.deltaTime;
            if (this.stunTime <= 0)
            {
                this.stunTime = 0;
                this.isStunned = false;
                this.animator.SetBool("Stun", false);
                yield return 0;
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
            /*
            Vector2 dir = this.target.position - this.transform.position; // Change animation to fit the angle
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && dir.y > 0) { this.animator.SetInteger("Direction", 0); } // Up
            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x) && dir.x > 0) { this.animator.SetInteger("Direction", 1); } // Right
            else if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && dir.y < 0) { this.animator.SetInteger("Direction", 2); } // Down
            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x) && dir.x < 0) { this.animator.SetInteger("Direction", 3); } // Left
            */
        }

        // When the enemy reaches the end of its path
        void EndPath()
        {
            HP.healthPoint -= this.damage;
            Destroy(this.gameObject);
        }
    }
}