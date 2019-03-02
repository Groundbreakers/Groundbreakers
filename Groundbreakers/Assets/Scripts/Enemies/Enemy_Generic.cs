namespace Assets.Enemies.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public class Enemy_Generic : MonoBehaviour
    {
        // Attributes and related flags
        public List<string> attributes = new List<string>();

        // Death effect object
        public GameObject deathEffect;

        public float evasion = 0;

        public int health = 1;

        // Stats, some modified by enemy scripts
        public int maxHealth = 1;

        public int power = 1;

        public float powerMultiplier = 1;

        public int regen = 0;

        public float speed = 1f;

        public float speedMultiplier = 1;

        public int time = 0;

        // Positioning objects and variables
        public List<Vector3> waypointList;

        // Animator & visuals
        private Animator animator;

        private int blightStacks = 0;

        private float blightTimer = 1;

        private float burnTimer = 1;

        private Vector2 dir;

        private bool fadingIn = true;

        private bool isBlighted = false;

        private bool isBurned = false;

        private bool isClone = false;

        private bool isEnraged = false;

        private bool isSlowed = false;

        private bool isStunned = false;

        private float regenTimer = 1;

        private Vector3 startingPosition;

        // Status flags
        private float statusMultiplier = 1;

        private float strongestSlow = 0;

        // Timers
        private float stunTime = 0;

        private Vector3 target;

        private bool waiting = false;

        private float waypointDetection;

        private int waypointIndex = 0;

        private float waypointWaitTime;

        // Blight handlers
        public void BlightEnemy()
        {
            if (!this.isBlighted)
            {
                this.isBlighted = true;
                if (this.isBurned)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }

            this.blightStacks += 1;
        }

        // Burn handlers
        public void BurnEnemy()
        {
            if (!this.isBurned)
            {
                this.isBurned = true;
                if (this.isBlighted)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        // Damage handler
        public void DamageEnemy(int damage, int armorpen, float accuracy, bool isMelee, bool isMarked)
        {
            // Check if the attack missed, or was dodged. If it hits, do damage calculation
            float accuracyroll = Random.Range(0.0f, 1.0f);
            float dodgeroll = Random.Range(0.0f, 1.0f);
            float flyingMod = 0;
            if (this.attributes.Contains("Flying") && isMelee == true)
            {
                flyingMod = 0.75f;
            }

            if (accuracyroll <= accuracy && dodgeroll > this.evasion + flyingMod)
            {
                int damagevalue;
                if (this.attributes.Contains("Armored"))
                {
                    damagevalue = (int)(damage * armorpen * .25f);
                }
                else
                {
                    damagevalue = damage;
                }

                if (isMarked == true)
                {
                    damagevalue = (int)(damagevalue * 1.25);

                    // Debug.Log("Marked Damage = " + damagevalue);
                    this.health -= damagevalue;
                    GameObject.Find("Canvas").GetComponent<DamagePopup>().ProduceText(damagevalue, this.transform);
                }
                else
                {
                    this.health -= damagevalue;
                    GameObject.Find("Canvas").GetComponent<DamagePopup>().ProduceText(damagevalue, this.transform);

                    // Debug.Log("Un Marked Damage = " + damagevalue);
                }
            }
            else
            {
                return;
            }
        }

        // access functions
        public bool getIsBlighted()
        {
            return this.isBlighted;
        }

        public bool getIsBurned()
        {
            return this.isBurned;
        }

        public bool getIsSlowed()
        {
            return this.isSlowed;
        }

        public bool getIsStunned()
        {
            return this.isStunned;
        }

        // Slow handler. Takes a float for slow strength (0.2 = 20% slow, 0.05 = 5% slow, etc)
        public void SlowEnemy(float strength)
        {
            this.isSlowed = true;
            if (strength > this.strongestSlow)
            {
                this.strongestSlow = strength;
                this.speedMultiplier = 1 - this.strongestSlow;
            }
        }

        // Stun handlers
        public void StunEnemy(float time)
        {
            this.isStunned = true;
            this.animator.SetBool("Stun", true);
            this.stunTime = time;
        }

        void BlightTick()
        {
            int blightDamage = (int)Math.Ceiling(0.02 * this.blightStacks * this.maxHealth * this.statusMultiplier);
            this.health -= blightDamage;
            this.blightTimer = 1;
        }

        void BurnTick()
        {
            int burnDamage = (int)Math.Ceiling(0.05 * this.maxHealth * this.statusMultiplier);
            this.health -= burnDamage;
            this.burnTimer = 1;
        }

        // When the enemy reaches the end of its path
        void EndPath()
        {
            GameObject counter = GameObject.Find("HPCounter");
            HP hp = counter.GetComponent<HP>();

            // Debug.Log("Took damage: " + (int)Math.Ceiling(this.power * this.powerMultiplier));
            hp.UpdateHealth(-(int)Math.Ceiling(this.power * this.powerMultiplier));
            Destroy(this.gameObject);
        }

        void FadeIn()
        {
            Color tmp = this.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a += 0.05f;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            if (tmp.a >= 1)
            {
                this.fadingIn = false;
            }
        }

        void FixedUpdate()
        {
            // Testing functions
            /* if (!this.isBlighted && Input.GetKeyDown("b"))
             {
                 this.BlightEnemy();
             }
             if (!this.isBurned && Input.GetKeyDown("u"))
             {
                 this.BurnEnemy();
             }

             if (!this.isSlowed && Input.GetKeyDown("s"))
             {
                 this.SlowEnemy(0.5f);
             }
             */

            // Placeholder for aura and revenge
            if (this.attributes.Contains("Aura") && this.attributes.Contains("Revenge"))
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }

            // Do fade-in if not opaque yet
            if (this.fadingIn)
            {
                this.FadeIn();
            }

            // Do Blight & Burn damage
            if (this.isBlighted)
            {
                this.blightTimer -= Time.deltaTime;
                if (this.blightTimer <= 0)
                {
                    this.BlightTick();
                }
            }

            if (this.isBurned)
            {
                this.burnTimer -= Time.deltaTime;
                if (this.burnTimer <= 0)
                {
                    this.BurnTick();
                }
            }

            // Check health, die if health <= 0.
            if (this.health <= 0)
            {
                GameObject effect = (GameObject)Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);
                Enemy_Death death = effect.GetComponent<Enemy_Death>();
                death.setDirection(animator.GetInteger("Direction"));
                Destroy(effect, 0.25f);
                Destroy(this.gameObject);
            }

            // Check for "Rage" attribute and HP < 50%
            if (this.attributes.Contains("Rage") && this.health <= this.maxHealth / 2)
            {
                if (!this.isEnraged)
                {
                    this.isEnraged = true;
                    this.powerMultiplier += 1f;
                    this.speedMultiplier += 1f;
                }
            }

            // Do regen
            this.regenTimer -= Time.deltaTime;
            if (this.regenTimer <= 0)
            {
                this.RegenTick();
            }

            // Move if not stunned, otherwise reduce stun time
            if (!this.isStunned)
            {
                // Move the enemy towards the target waypoint
                this.transform.Translate(
                    this.dir.normalized * this.speed * this.speedMultiplier * Time.deltaTime,
                    Space.World);

                // Check if the waypoint has been reached
                if (Vector2.Distance(this.transform.position, this.target) <= this.waypointDetection)
                {
                    this.waiting = true;
                }

                // Do random 'wait time' if applicable
                if (this.waiting == true)
                {
                    if (this.waypointWaitTime > 0)
                    {
                        this.waypointWaitTime -= Time.deltaTime;
                    }
                    else
                    {
                        this.GetNextWaypoint();
                    }
                }
            }
            else
            {
                this.StunTick();
            }

            this.time++;
        }

        // Get the next waypoint and update the index
        void GetNextWaypoint()
        {
            if (this.waypointIndex >= this.waypointList.Count - 1)
            {
                this.EndPath();
                return;
            }

            this.waypointIndex++;
            this.target = this.waypointList[this.waypointIndex];

            // Get new random waypoint accuracy
            this.dir = this.target - this.transform.position;
            float positiondiceroll = Random.Range(-0.4f, 0.4f);
            if (positiondiceroll < 0)
            {
                this.waypointDetection = 0.03f;
                this.waypointWaitTime = Math.Abs(positiondiceroll);
            }
            else
            {
                this.waypointDetection = 0.03f + positiondiceroll;
                this.waypointWaitTime = 0;
            }

            this.waiting = false;

            Vector2 dir;
            dir.x = this.target.x - this.transform.position.x; // Change animation to fit the angle
            dir.y = this.target.y - this.transform.position.y; // Change animation to fit the angle
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && dir.y > 0)
            {
                this.animator.SetInteger("Direction", 0);
            }

            // Up
            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x) && dir.x > 0)
            {
                this.animator.SetInteger("Direction", 1);
            }

            // Right
            else if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && dir.y < 0)
            {
                this.animator.SetInteger("Direction", 2);
            }

            // Down
            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x) && dir.x < 0)
            {
                this.animator.SetInteger("Direction", 3);
            }

            // Left
        }

        void InitializeAttributes()
        {
            if (this.attributes.Contains("Resistance"))
            {
                this.statusMultiplier = 0.5f;
            }

            if (this.attributes.Contains("Immune"))
            {
                this.statusMultiplier = 0;
            }

            if (this.attributes.Contains("Evasion"))
            {
                this.evasion += 0.25f;
            }

            if (this.attributes.Contains("Regen"))
            {
                this.regen += (int)Math.Ceiling(0.05 * this.maxHealth);
            }

            if (this.attributes.Contains("Aggregation"))
            {
                this.Invoke("MakeAggregationClone", 0.3f);
                this.Invoke("MakeAggregationClone", 0.6f);
            }
        }

        void MakeAggregationClone()
        {
            if (!this.isClone)
            {
                GameObject thisClone = Instantiate(this.gameObject, this.startingPosition, Quaternion.identity);
                thisClone.GetComponent<Enemy_Generic>().isClone = true;
            }
        }

        // Regen handler
        void RegenTick()
        {
            if (this.health < this.maxHealth)
            {
                this.health += this.regen;
                if (this.health > this.maxHealth)
                {
                    this.health = this.maxHealth;
                }
            }

            this.regenTimer = 1;
        }

        void Start()
        {
            // Enemy fade-in
            Color tmpcolor = this.gameObject.GetComponent<SpriteRenderer>().color;
            tmpcolor.a = 0;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmpcolor;

            this.startingPosition = this.gameObject.transform.position;
            this.animator = this.GetComponent<Animator>();
            this.InitializeAttributes();

            // Randomize position and waypoint accuracy
            this.transform.Translate(new Vector3(Random.Range(-0.2f, 0.3f), Random.Range(-0.2f, 0.3f), 0));

            // Get first waypoint
            this.GetNextWaypoint();
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
    }
}