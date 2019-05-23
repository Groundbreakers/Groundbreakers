namespace Assets.Enemies.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public class Enemy_Generic : MonoBehaviour
    {
        #region Variable Declarations

        // Stats, some modified by enemy scripts
        public string specie;

        public int maxHealth = 1;

        public int health = 1;

        public float speed = 1f;

        public float speedMultiplier = 1;

        public int power = 1;

        public float powerMultiplier = 1;

        public float evasion;

        public int regen;

        // Attributes and related flags
        public List<string> attributes = new List<string>();

        private bool isEnraged;

        private bool isClone;

        // Death effect object
        [SerializeField]
        private GameObject deathEffect;

        private Vector3 target;

        private Vector3 startingPosition;

        private Vector2 dir;

        private bool waiting;

        // Animator & visuals
        private Animator animator;

        private bool fadingIn = true;

        // Status flags
        private float statusMultiplier = 1;

        private bool isStunned;

        private bool isBlighted;

        private int blightStacks;

        private bool isBurned;

        private bool isSlowed;

        private bool isPurged = true;

        private float strongestSlow;

        // Timers
        private float stunTime;

        private float blightTimer = 1;

        private float burnTimer = 1;

        private float regenTimer = 1;

        #endregion

        private GameObject crystalCounter;

        public int time;

        private void OnEnable()
        {
            this.animator = this.GetComponent<Animator>();
            GameObject o;

            // Enemy fade-in
            var tmpColor = this.gameObject.GetComponent<SpriteRenderer>().color;
            tmpColor.a = 0;

            (o = this.gameObject).GetComponent<SpriteRenderer>().color = tmpColor;

            this.startingPosition = o.transform.position;

            this.InitializeAttributes();
        }

        private void Start()
        {
            if (this.attributes.Contains("Aura") && this.attributes.Contains("Revenge"))
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                this.isPurged = false;
            }

            this.crystalCounter = GameObject.Find("CrystalCounter");
        }

        private void FixedUpdate()
        {
            // Testing functions
            if (!this.isBlighted && Input.GetKeyDown("b"))
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
                var effect = Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);
                var death = effect.GetComponent<Enemy_Death>();
                death.setDirection(this.animator.GetInteger("Direction"));
                CrystalCounter temp = crystalCounter.GetComponent<CrystalCounter>();
                temp.SetCrystals((int)(10 * UnityEngine.Random.Range(1.0f - .2f, 1.0f + .2f)));
                Destroy(effect, 0.5f);
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
                //// Move the enemy towards the target waypoint
                //this.transform.Translate(
                //    this.dir.normalized * this.speed * this.speedMultiplier * Time.deltaTime,
                //    Space.World);

                //// Check if the waypoint has been reached
                //if (Vector2.Distance(this.transform.position, this.target) <= this.waypointDetection)
                //{
                //    this.waiting = true;
                //}

                //// Do random 'wait time' if applicable
                //if (this.waiting)
                //{
                //    if (this.waypointWaitTime > 0)
                //    {
                //        this.waypointWaitTime -= Time.deltaTime;
                //    }
                //    else
                //    {
                //        this.GetNextWaypoint();
                //    }
                //}
            }
            else
            {
                this.StunTick();
            }

            this.time++;
        }

        // Damage handler
        public void DamageEnemy(int damage, int armorpen, float accuracy, bool isMelee, bool isMarked)
        {
            // Check if the attack missed, or was dodged. If it hits, do damage calculation
            var accuracyroll = Random.Range(0.0f, 1.0f);
            var dodgeroll = Random.Range(0.0f, 1.0f);
            float flyingMod = 0;
            if (this.attributes.Contains("Flying") && isMelee)
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

                if (isMarked)
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
                GameObject.Find("Canvas").GetComponent<DamagePopup>().ProduceText(0, this.transform);
            }
        }

        #region Status Handlers

        // Stun handlers
        public void StunEnemy(float time)
        {
            this.isStunned = true;
            this.animator.SetBool("Stun", true);
            this.stunTime = time;
        }

        private void StunTick()
        {
            this.stunTime -= Time.deltaTime;
            if (this.stunTime <= 0)
            {
                this.stunTime = 0;
                this.isStunned = false;
                this.animator.SetBool("Stun", false);
            }
        }

        // Blight handlers
        public void BlightEnemy()
        {
            if (!this.isBlighted)
            {
                this.isBlighted = true;
                if (!this.isPurged && this.isBurned)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
                else if (!this.isPurged)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                else if (this.isBurned)
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

        public void breakEnemyArmor()
        {
            this.attributes.Remove("Armored");
        }

        public void purgeEnemy()
        {
            this.attributes.Remove("Aura");
            this.attributes.Remove("Revenge");
            this.GetComponent<SpriteRenderer>().color = Color.white;
            this.isPurged = true;
        }

        private void BlightTick()
        {
            var blightDamage = (int)Math.Ceiling(0.02 * this.blightStacks * this.maxHealth * this.statusMultiplier);
            this.health -= blightDamage;
            this.blightTimer = 1;
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
                else if (!this.isPurged)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        private void BurnTick()
        {
            var burnDamage = (int)Math.Ceiling(0.05 * this.maxHealth * this.statusMultiplier);
            this.health -= burnDamage;
            this.burnTimer = 1;
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

        // Regen handler
        private void RegenTick()
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

        #endregion

        private void InitializeAttributes()
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

        private void FadeIn()
        {
            var tmp = this.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a += 0.05f;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            if (tmp.a >= 1)
            {
                this.fadingIn = false;
            }
        }

        private void MakeAggregationClone()
        {
            if (!this.isClone)
            {
                var thisClone = Instantiate(this.gameObject, this.startingPosition, Quaternion.identity);
                thisClone.GetComponent<Enemy_Generic>().isClone = true;
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

        public bool getIsPurged()
        {
            return this.isPurged;
        }

        public bool getIsSlowed()
        {
            return this.isSlowed;
        }

        public bool getIsStunned()
        {
            return this.isStunned;
        }
    }
}