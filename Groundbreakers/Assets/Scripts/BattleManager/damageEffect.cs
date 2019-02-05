// Created by Javy
// This script handles different damage effects and target searching modes

using System.Collections;
using System.Collections.Generic;
using Assets.Enemies.Scripts;
using UnityEngine;

public class damageEffect : MonoBehaviour
{
    [Header("Set Up")]
    public string enemyTag = "Enemy";

    public float range = 15f;

    public Transform rangeAttackFirepoint;

    public GameObject rangeAttackPrefab;

    [Header("Discrete Projectile Attack")]
    public float fireRate = 1f;

    [Header("Laser")]
    public LineRenderer lineRenderer;
    
    public int laserArmorPen = 0;

    public int laserDamage = 0;
    
    private float fireCountdown = 0f;

    private ModuleTemplate module;

    [Header("MultiShot")]
    public float sprayAngle = 140f;

    public int bulletNum = 5;

    public float damageReductionPercent;

    // draw the attack range of the character selected
    private GameObject moduleObject;

    private Transform target;

    private List<GameObject> targetedEnemies;

    void Awake() {
        this.targetedEnemies = new List<GameObject>();
        this.moduleObject = GameObject.Find("BattleManager");
        this.module = this.moduleObject.GetComponent<ModuleTemplate>();
    }

    // control flow for discrete projectile attack mode
    void BulletMode() {
        if (this.module.burstAE == true) this.StartCoroutine(this.burstShot(this.fireCountdown, 3));
        else if (this.module.multiShotAE == true) multiShot();
        else this.shoot();
    }

    void multiShot() {
     //   Debug.Log("multishot");
      //  Vector2 direction = target.transform.position - this.transform.position;
     //   float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation,0f);

        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation, 15f);


        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation, 30f);

        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation, -15f);

        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation, -30f);
    }

    // default target searching, set the nearest enemy as target
    void defaultMode() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(this.enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(this.transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDistance <= this.range)
            {
                this.target = nearestEnemy.transform;
            }
            else
            {
                this.target = null;
            }
        }
    }

    void fireCount() {
        if (this.target == null)
        {
            if (this.module.laserAE == true)
            {
                if (this.lineRenderer.enabled) this.lineRenderer.enabled = false;
            }
            return;
        }
        
        if (this.module.laserAE == true)
        {
            this.Laser();
        }
        else
        {
            this.lineRenderer.enabled = false;

            if (this.fireCountdown <= 0f)
            {
                this.fireCountdown = 1f / this.fireRate;
                this.BulletMode();
            }

            this.fireCountdown -= Time.deltaTime;
        }
    }

    void instantiateBullet(
        GameObject rangeAttackPrefab,
        Vector3 rangeAttackFirepointPosition,
        Quaternion rangeAttackFirepointRoatation, float angleToRotate) {
        GameObject rangeAttackObject = (GameObject)Instantiate(
            rangeAttackPrefab,
            rangeAttackFirepointPosition,
            rangeAttackFirepointRoatation);
        rangeattack rangeattack = rangeAttackObject.GetComponent<rangeattack>();

        if (rangeattack != null && this.module.multiShotAE != true)
        {
            rangeattack.chase(this.target);
        }
        else if(rangeattack != null && this.module.multiShotAE == true)
        {
            rangeattack.chase(this.target);
            rangeattack.setAngle(angleToRotate);
        }
    }

    void Laser() {
        if (this.lineRenderer.enabled == false) this.lineRenderer.enabled = true;
        this.lineRenderer.SetPosition(0, this.rangeAttackFirepoint.position);
        this.lineRenderer.SetPosition(1, this.target.position);

        //Collision detection using Raycast
        RaycastHit2D[] hits;
        Vector2 direction = this.target.position - this.transform.position;
        hits = Physics2D.RaycastAll(this.lineRenderer.transform.position, direction, Mathf.Infinity);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag == "Enemy")
            {
                if (hits[i].collider.gameObject != null)
                {
                    Enemy_Generic enemy = hits[i].collider.gameObject.GetComponent<Enemy_Generic>();
                    //Currently set to 0 damage because Update function 1 shot the Enemy
                    enemy.DamageEnemy(this.laserDamage, this.laserArmorPen, 1, false);
                }
            }
        }
    }

    // burst shot
    IEnumerator burstShot(float fireCountdown, int burstSize) {
        for (int i = 0; i < burstSize; i++)
        {
            this.instantiateBullet(
                this.rangeAttackPrefab,
                this.rangeAttackFirepoint.position,
                this.rangeAttackFirepoint.rotation, 0f);
            yield return new WaitForSeconds(5f * Time.deltaTime);
        }
    }

    // if an enemy enters in range
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            this.targetedEnemies.Add(other.gameObject);
            this.updateTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            this.targetedEnemies.Remove(other.gameObject);
            this.updateTarget();
        }
    }

    // Instantiate and and chase the target
    void shoot() {
        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation,0f);
    }

    void Update() {
        this.fireCount();
    }

    void updateTarget() {
        this.defaultMode();
    }
}
