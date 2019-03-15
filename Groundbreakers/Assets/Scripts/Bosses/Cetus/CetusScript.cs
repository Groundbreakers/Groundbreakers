using System;
using System.Collections;
using System.Linq;

using Assets.Scripts;

using UnityEngine;

using Random = UnityEngine.Random;

public class CetusScript : MonoBehaviour
{
    public GameObject chargeParticle;

    public GameObject chargeShot;

    public float chargeShotTimer = 36;

    // Stored objects
    public GameObject deathEffect;

    public int health = 20000;

    // Stats
    public int maxHealth = 1;

    public int power = 1;

    public float powerMultiplier = 1;

    public float splashTimer = 24;

    public float waterStrikeTimer = 6;

    private int blightStacks;

    private float blightTimer = 1;

    private float burnTimer = 1;

    private float cleanseTimer = 12;

    private bool combat;

    private bool doingAttack;

    private bool dying = false;

    // Stage flags
    private bool entering = true;

    private float entranceTimer = 4;

    private bool isBlighted;

    private bool isBurned;

    private bool isStunned;

    private Transform[] splashTiles;

    // Status flags
    private float statusMultiplier = 1;

    // Timers
    private float stunTime;

    private Transform[] waterStrikeTiles;

    private Transform TileHolder;

    private int waterStrikeTilesNum = 12;

    private int splashTilesNum = 20;

    private GameMap gamemap;

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

    // Damage function. Takes armorpen, but I don't know if it'll be used in the calculation or not.
    public void DamageCetus(int damage)
    {
        this.health -= damage;
        GameObject.Find("Canvas").GetComponent<DamagePopup>().ProduceText(damage, this.transform);
    }

    // Stun handlers
    public void StunEnemy(float time)
    {
        this.isStunned = true;
        this.stunTime = time;
    }

    // Update combat timers
    private void AttackTimerTick()
    {
        this.waterStrikeTimer -= Time.deltaTime;
        this.splashTimer -= Time.deltaTime;
        this.chargeShotTimer -= Time.deltaTime;
    }

    private void BlightTick()
    {
        var blightDamage = (int)Math.Ceiling(0.02 * this.blightStacks * this.maxHealth * this.statusMultiplier);
        this.health -= blightDamage;
        this.blightTimer = 1;
    }

    private void BurnTick()
    {
        var burnDamage = (int)Math.Ceiling(0.05 * this.maxHealth * this.statusMultiplier);
        this.health -= burnDamage;
        this.burnTimer = 1;
    }

    // Charge Shot action
    private IEnumerator ChargeShot()
    {
        GameObject tmp;
        float randomX;
        float randomY;
        for (var i = 0; i < 30; i++)
        {
            tmp = Instantiate(this.chargeParticle, this.transform);
            randomX = this.transform.localPosition.x + Random.Range(-5f, 5f);
            randomY = this.transform.localPosition.y + Random.Range(-5f, 5f);
            tmp.transform.position = new Vector3(randomX, randomY, 0);
            yield return 0;
        }

        yield return new WaitForSeconds(1f);
        Instantiate(this.chargeShot, this.transform.position, Quaternion.identity);
        this.doingAttack = false;
        yield return 0;
    }

    // Cleanse action
    private IEnumerator Cleanse()
    {
        var tmpcolor = Color.white;

        // Fade to blue
        while (tmpcolor.r > 0)
        {
            tmpcolor.r -= 0.04f;
            tmpcolor.g -= 0.04f;
            tmpcolor.b += 0.1f;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmpcolor;
            yield return 0;
        }

        this.PurgeStatus();
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

    // Entrance animation
    private IEnumerator Entrance()
    {
        // (3.5, 2) -> (3.5, 3.5)
        this.transform.position = new Vector3(3.5f, 1f, -1f);
        yield return new WaitForSeconds(10);
        for (var i = this.entranceTimer; i >= 0; i -= Time.deltaTime)
        {
            this.transform.Translate(Vector3.up / 90, Space.World);
            this.transform.Rotate(Vector3.forward, 720 * Time.deltaTime);
            yield return null;
        }

        this.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(1);
        this.startCombat();
    }

    // Get the 20 surrounding tiles
    private void GetSplashTiles()
    {
        var TileHolder = GameObject.Find("TileMap").transform;
        this.splashTiles[0] = TileHolder.GetChild(9);
        this.splashTiles[1] = TileHolder.GetChild(10);
        this.splashTiles[2] = TileHolder.GetChild(11);
        this.splashTiles[3] = TileHolder.GetChild(12);
        this.splashTiles[4] = TileHolder.GetChild(13);
        this.splashTiles[5] = TileHolder.GetChild(14);
        this.splashTiles[6] = TileHolder.GetChild(17);
        this.splashTiles[7] = TileHolder.GetChild(22);
        this.splashTiles[8] = TileHolder.GetChild(25);
        this.splashTiles[9] = TileHolder.GetChild(30);
        this.splashTiles[10] = TileHolder.GetChild(33);
        this.splashTiles[11] = TileHolder.GetChild(38);
        this.splashTiles[12] = TileHolder.GetChild(41);
        this.splashTiles[13] = TileHolder.GetChild(46);
        this.splashTiles[14] = TileHolder.GetChild(49);
        this.splashTiles[15] = TileHolder.GetChild(50);
        this.splashTiles[16] = TileHolder.GetChild(51);
        this.splashTiles[17] = TileHolder.GetChild(52);
        this.splashTiles[18] = TileHolder.GetChild(53);
        this.splashTiles[19] = TileHolder.GetChild(54);
    }

    private void GetWaterStrikeTiles()
    {
        int[] tiles = new int[this.waterStrikeTilesNum];
        int tmp;
        int count = 0;

        // Get 8 random tile numbers. Tiles cannot be in the middle 4x4, or already be a chosen tile.
        while (count < this.waterStrikeTilesNum)
        {
            tmp = Random.Range(0, this.gamemap.CetusWaterStrikeList.Count);
            if (tiles.Contains(tmp) == false)
            {
                tiles[count] = tmp;
                count += 1;
            }
        }

        // Get the tile objects from the tile numbers.
        for (int i = 0; i < this.waterStrikeTilesNum; i++)
        {
            this.waterStrikeTiles[i] = this.gamemap.CetusWaterStrikeList[tiles[i]];
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

    // Splash action
    private IEnumerator Splash()
    {
        this.GetSplashTiles();
        var characterlist = GameObject.Find("CharacterList").transform;

        // Affected tiles flash blue
        var tmpcolor = Color.white;
        while (tmpcolor.r > 0)
        {
            tmpcolor.r -= 0.02f;
            for (var i = 0; i < 20; i++)
            {
                this.splashTiles[i].GetComponent<SpriteRenderer>().color = tmpcolor;
            }

            yield return 0;
        }

        // Stun characters on affected tiles
        for (var i = 0; i < 20; i++)
        {
            var tmpchar = this.splashTiles[i].GetComponent<SelectNode>().GetCharacterIndex();
            if (tmpchar != -1)
            {
                characterlist.GetChild(tmpchar).GetComponent<characterAttack>().stun(5);
            }
        }

        // Affected tiles go back to normal color
        while (tmpcolor.r < 1)
        {
            tmpcolor.r += 0.02f;
            for (var i = 0; i < 20; i++)
            {
                this.splashTiles[i].GetComponent<SpriteRenderer>().color = tmpcolor;
            }

            yield return 0;
        }

        this.doingAttack = false;
        yield return 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        waterStrikeTiles = new Transform[this.waterStrikeTilesNum];
        splashTiles = new Transform[this.splashTilesNum];
        this.gamemap = GameObject.Find("TileMap").GetComponent<GameMap>();
        TileHolder = GameObject.Find("TileMap").transform;
        Random.seed = System.Environment.TickCount;

        StartCoroutine(Entrance());
    }

    // Start combat
    private void startCombat()
    {
        this.combat = true;

        // this.tag = "Boss";
    }

    private void StunTick()
    {
        this.stunTime -= Time.deltaTime;
        if (this.stunTime <= 0)
        {
            this.stunTime = 0;
            this.isStunned = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Only do combat things if the fight has begun
        if (this.combat)
        {
            // Do countdown for cleanse
            this.cleanseTimer -= Time.deltaTime;
            if (this.cleanseTimer <= 0)
            {
                this.StartCoroutine(this.Cleanse());
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
                var effect = Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);
                Destroy(effect, 0.25f);
                Destroy(this.gameObject);
            }

            // Only do attacks & attack timers if not stunned
            if (!this.isStunned)
            {
                if (!this.doingAttack)
                {
                    // Don't try to do multiple attacks at once
                    this.AttackTimerTick();

                    // Choose the strongest available attack. Round the other timers to avoid 'double attacks'
                    if (this.chargeShotTimer <= 0)
                    {
                        this.doingAttack = true;
                        this.StartCoroutine(this.ChargeShot());
                        this.chargeShotTimer = 36;
                        this.splashTimer = Mathf.Round(this.splashTimer);
                        this.waterStrikeTimer = Mathf.Round(this.waterStrikeTimer);
                    }
                    else if (this.splashTimer <= 0)
                    {
                        this.doingAttack = true;
                        this.StartCoroutine(this.Splash());
                        this.splashTimer = 24;
                        this.chargeShotTimer = Mathf.Round(this.chargeShotTimer);
                        this.waterStrikeTimer = Mathf.Round(this.waterStrikeTimer);
                    }
                    else if (this.waterStrikeTimer <= 0)
                    {
                        this.doingAttack = true;
                        this.StartCoroutine(this.WaterStrike());
                        this.waterStrikeTimer = 6;
                        this.chargeShotTimer = Mathf.Round(this.chargeShotTimer);
                        this.splashTimer = Mathf.Round(this.splashTimer);
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
                this.StunTick();
            }
        }
    }

    // Water Strike action
    private IEnumerator WaterStrike()
    {
        this.GetWaterStrikeTiles();
        var characterlist = GameObject.Find("CharacterList").transform;

        // Affected tiles flash blue
        var tmpcolor = Color.white;
        while (tmpcolor.r > 0)
        {
            tmpcolor.r -= 0.02f;
            for (var i = 0; i < this.waterStrikeTilesNum; i++)
            {
                this.waterStrikeTiles[i].GetComponent<SpriteRenderer>().color = tmpcolor;
            }

            yield return 0;
        }

        // Stun characters on affected tiles
        for (var i = 0; i < this.waterStrikeTilesNum; i++)
        {
            var tmpchar = this.waterStrikeTiles[i].GetComponent<SelectNode>().GetCharacterIndex();
            if (tmpchar != -1)
            {
                characterlist.GetChild(tmpchar).GetComponent<characterAttack>().stun(5);
            }
        }

        // Affected tiles go back to normal color
        while (tmpcolor.r < 1)
        {
            tmpcolor.r += 0.02f;
            for (var i = 0; i < this.waterStrikeTilesNum; i++)
            {
                this.waterStrikeTiles[i].GetComponent<SpriteRenderer>().color = tmpcolor;
            }

            yield return 0;
        }

        this.doingAttack = false;
        yield return 0;
    }
}