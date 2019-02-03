using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cetus_Script : MonoBehaviour
{
    // Stats
    public int maxHealth = 1;
    public int health = 1;
    public int power = 1;
    public float powerMultiplier = 1;

    // Stage flags
    private bool entering = true;
    private bool combat = false;
    private bool dying = false;
    private bool doingAttack = false;

    // Status flags
    private float statusMultiplier = 1;
    private bool isStunned = false;
    private bool isBlighted = false;
    private int blightStacks = 0;
    private bool isBurned = false;

    // Stored objects
    public GameObject deathEffect;
    public GameObject chargeParticle;

    // Timers
    private float stunTime = 0;
    private float blightTimer = 1;
    private float burnTimer = 1;

    private float cleanseTimer = 12;
    private float waterStrikeTimer = 6;
    private float splashTimer = 24;
    private float chargeShotTimer = 3;

    private float entranceTimer = 4;

    // Sorting layer groundtiles 4.5

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Entrance());
    }

    // Update is called once per frame
    void Update()
    {
        // Only do combat things if the fight has begun
        if (this.combat)
        {
            // Do countdown for cleanse
            this.cleanseTimer -= Time.deltaTime;
            if (this.cleanseTimer <= 0)
            {
                StartCoroutine(Cleanse());
                this.cleanseTimer = 12;
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
                Destroy(effect, 0.25f);
                Destroy(this.gameObject);
            }

            // Only do attacks & attack timers if not stunned
            if (!this.isStunned)
            {
                if (!doingAttack) // Don't try to do multiple attacks at once
                {
                    this.AttackTimerTick();
                    // Choose the strongest available attack
                    if (this.chargeShotTimer <= 0)
                    {
                        this.doingAttack = true;
                        StartCoroutine(ChargeShot());
                        this.chargeShotTimer = 3;
                    }
                    else if (this.splashTimer <= 0)
                    {
                        this.doingAttack = true;
                        StartCoroutine(Splash());
                        this.splashTimer = 24;
                    }
                    else if (this.waterStrikeTimer <= 0)
                    {
                        this.doingAttack = true;
                        StartCoroutine(WaterStrike());
                        this.waterStrikeTimer = 6;
                    }

                    // Reset timers for overridden attacks
                    if (this.splashTimer <= 0)
                    {
                        this.splashTimer = 24;
                    }
                    if (this.waterStrikeTimer <= 0)
                    {
                        this.waterStrikeTimer = 6;
                    }
                }
            }
            else
            {
                StunTick();
            }
        }
    }

    // Entrance animation
    private IEnumerator Entrance()
    {
        // (3.5, 2) -> (3.5, 3.5)
        this.transform.position = new Vector3(3.5f,1f,-1f);
        yield return new WaitForSeconds(10);
        for (float i = entranceTimer; i >= 0; i -= Time.deltaTime)
        {
            transform.Translate(Vector3.up / 90, Space.World);
            transform.Rotate(Vector3.forward, 720 * Time.deltaTime);
            yield return null;
        }
        this.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(1);
        startCombat();
    }

    // Start combat
    private void startCombat()
    {
        this.combat = true;
        this.tag = "Enemy";
    }

    // Update combat timers
    private void AttackTimerTick()
    {
        this.waterStrikeTimer -= Time.deltaTime;
        this.splashTimer -= Time.deltaTime;
        this.chargeShotTimer -= Time.deltaTime;
    }

    #region Status Handlers

    // Stun handlers
    public void StunEnemy(float time)
    {
        this.isStunned = true;
        this.stunTime = time;
    }

    void StunTick()
    {
        this.stunTime -= Time.deltaTime;
        if (this.stunTime <= 0)
        {
            this.stunTime = 0;
            this.isStunned = false;
        }
    }

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

    void BlightTick()
    {
        int blightDamage = (int)Math.Ceiling(0.02 * this.blightStacks * this.maxHealth * this.statusMultiplier);
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
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    void BurnTick()
    {
        int burnDamage = (int)Math.Ceiling(0.05 * this.maxHealth * this.statusMultiplier);
        this.health -= burnDamage;
        this.burnTimer = 1;
    }

    #endregion

    #region Boss Actions

    // Cleanse action
    private IEnumerator Cleanse()
    {
        Color tmpcolor = this.gameObject.GetComponent<SpriteRenderer>().color;
        // Fade to blue
        while (tmpcolor.r > 0)
        {
            tmpcolor.r -= 0.04f;
            tmpcolor.g -= 0.04f;
            tmpcolor.b += 0.1f;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmpcolor;
            yield return 0;
        }
        PurgeStatus();
        tmpcolor.b = 1f;
        // Fade back to color
        while (tmpcolor.r < 1 || tmpcolor.g < 1)
        {
            tmpcolor.r += 0.04f;
            tmpcolor.g += 0.04f;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmpcolor;
            yield return 0;
        }
    }

    // Used in Cleanse()
    private void PurgeStatus()
    {
        this.isStunned = false;
        this.stunTime = 0;
        this.isBlighted = false;
        this.blightStacks = 0;
        this.blightTimer = 1;
        this.isBurned = false;
        this.burnTimer = 1;
    }

    // Charge Shot action
    private IEnumerator ChargeShot()
    {
        GameObject tmp;
        float randomX;
        float randomY;
        for (int i = 0; i < 30; i++)
        {
            tmp = Instantiate(chargeParticle, this.transform);
            randomX = this.transform.localPosition.x + Random.Range(-5f, 5f);
            randomY = this.transform.localPosition.y + Random.Range(-5f, 5f);
            tmp.transform.position = new Vector3(randomX, randomY, 0);
            yield return 0;
        }
        this.doingAttack = false;
        yield return 0;
    }

    // Splash action
    private IEnumerator Splash()
    {
        yield return 0;
    }

    // Water Strike action
    private IEnumerator WaterStrike()
    {
        yield return 0;
    }

    #endregion
}
