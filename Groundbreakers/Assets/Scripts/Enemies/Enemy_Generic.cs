namespace Assets.Enemies.Scripts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    public class Enemy_Generic : MonoBehaviour
    {
        #region Variable Declarations

        // Stat placeholders
        public float health = 1;
        public float speed = 1f;
        public int power = 1;
        public List<String> attributes = new List<string>();

        // Death effect object
        public GameObject deathEffect;

        // Object that contains waypoint list, and default index
        private Transform target;
        private int waypointIndex = -1;

        // Animator
        private Animator animator;

        // Status handlers
        private bool isStunned = false;
        private float stunTime = 0;

        #endregion

        public int time = 0;
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
                Destroy(effect, (float)0.25);
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
            else
            {
                this.StunTick();
            }

            time++;
        }

        // Damage handler
        public void DamageEnemy(float damage, float armorpen)
        {
            float damagevalue;
            if (this.attributes.Contains("Armored"))
            {
                damagevalue = armorpen;
            }
            else
            {
                damagevalue = damage;
            }
            this.health -= damagevalue;
        }

        // Stun handlers
        public void StunEnemy(float time)
        {
            this.isStunned = true;
            this.animator.SetBool("Stun", true);
            this.stunTime = time;
        }

        void StunTick()
        {
            this.stunTime -= Time.deltaTime;
            if (this.stunTime <= 0)
            {
                this.stunTime = 0;
                this.isStunned = false;
                this.animator.SetBool("Stun", false);
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
            HP.healthPoint -= this.power;
            Destroy(this.gameObject);
        }
    }
}