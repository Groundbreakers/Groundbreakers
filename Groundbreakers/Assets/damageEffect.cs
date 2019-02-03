using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts;
using UnityEditor.Experimental.UIElements.GraphView;

public class damageEffect : MonoBehaviour
{
    public string enemyTag = "Enemy";

    public float fireRate = 1f;

    public float range = 15f;

    public Transform rangeAttackFirepoint;

    public GameObject rangeAttackPrefab;

    private float fireCountdown = 0f;

    private Transform target;

    private List<GameObject> targetedEnemies;
    // draw the attack range of the character selected

    private GameObject moduleObject;

    void Awake() { targetedEnemies = new List<GameObject>(); }

    void Update()
    {
        this.fireCount();
        
    }

    //if an enemy enters in range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            targetedEnemies.Add(other.gameObject);
            updateTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            targetedEnemies.Remove(other.gameObject);
            updateTarget();
        }
    }

    // update the closest target in range
    void updateTarget() {
        defaultMode();
    }

    void RangeAttackMode() {
        this.moduleObject = GameObject.Find("BattleManager");
        ModuleTemplate module = this.moduleObject.GetComponent<ModuleTemplate>();
        if (module.multiShotAE == true) Debug.Log("multishot");
        else shoot();
    }
    
    void fireCount()
    {
        if (this.target == null)
        {
           return;
        }

        if (this.fireCountdown <= 0f)
        {
            RangeAttackMode();
            this.fireCountdown = 1f / this.fireRate;
        }

        this.fireCountdown -= Time.deltaTime;
    }

    // Instantiate and and chase the target
    void shoot() {
        instantiateBullet(
            this.rangeAttackPrefab,
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation); 
    }

    void multiShot(float delay) {
    

    }

    void instantiateBullet(GameObject rangeAttackPrefab, Vector3 rangeAttackFirepointPosition, Quaternion rangeAttackFirepointRoatation) {

        
        GameObject rangeAttackObject= (GameObject)Instantiate(
             rangeAttackPrefab,
             rangeAttackFirepointPosition,
             rangeAttackFirepointRoatation);
         rangeattack rangeattack = rangeAttackObject.GetComponent<rangeattack>();


         if (rangeattack != null)
         {
             rangeattack.chase(this.target);
         }
       
    }



    void defaultMode()
    {
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
}
