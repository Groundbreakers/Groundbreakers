using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Enemies.Scripts;
using UnityEngine;
using UnityEngineInternal;

public class damageEffect : MonoBehaviour
{
    public string enemyTag = "Enemy";

    public float fireRate = 1f;

    public float range = 15f;

    public Transform rangeAttackFirepoint;

    public GameObject rangeAttackPrefab;

    private float fireCountdown = 0f;

    // draw the attack range of the character selected
    private GameObject moduleObject;

    private Transform target;

    private List<GameObject> targetedEnemies;

    private ModuleTemplate module;

    public LineRenderer lineRenderer;

    void Awake() {
        this.targetedEnemies = new List<GameObject>();
        this.moduleObject = GameObject.Find("BattleManager");
        module = this.moduleObject.GetComponent<ModuleTemplate>();
    }

    void Update()
    {
        this.fireCount();
       
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

        this.module.laserAE = true;
        if (this.module.laserAE == true)
        {
            Laser();

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

    void Laser() {
        if (this.lineRenderer.enabled == false) this.lineRenderer.enabled = true;
        this.lineRenderer.SetPosition(0, this.rangeAttackFirepoint.position);
        this.lineRenderer.SetPosition(1,this.target.position);

     
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(this.lineRenderer.transform.position, this.target.position - this.transform.position, Mathf.Infinity);

        for (int i = 0; i < hits.Length; i++)
        {
            
               // Enemy_Generic test = hits[i].transform.GetComponent<Enemy_Generic>();
               if(hits[i].collider.gameObject.tag == "Enemy")
                Debug.Log(hits[i].collider.gameObject.name);
            
        }
    }

    void instantiateBullet(
        GameObject rangeAttackPrefab,
        Vector3 rangeAttackFirepointPosition,
        Quaternion rangeAttackFirepointRoatation) {
        GameObject rangeAttackObject = (GameObject)Instantiate(
            rangeAttackPrefab,
            rangeAttackFirepointPosition,
            rangeAttackFirepointRoatation);
        rangeattack rangeattack = rangeAttackObject.GetComponent<rangeattack>();

        if (rangeattack != null)
        {
            rangeattack.chase(this.target);
        }
    }

    // burst shot
    IEnumerator multiShot(float fireCountdown, int burstSize) {
        for (int i = 0; i < burstSize; i++)
        {
            this.instantiateBullet(
                this.rangeAttackPrefab,
                this.rangeAttackFirepoint.position,
                this.rangeAttackFirepoint.rotation);
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

    // control flow for attack mode
    void BulletMode() {
       // ModuleTemplate module = this.moduleObject.GetComponent<ModuleTemplate>();
        if (module.multiShotAE == true) this.StartCoroutine(this.multiShot(this.fireCountdown, 3));
        else this.shoot();
    }

    // Instantiate and and chase the target
    void shoot() {
        this.instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation);
    }
    
    void updateTarget() {
        this.defaultMode();
    }
}
