using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAttack : MonoBehaviour
{
    private Transform target;
    public float range = 15f;

    public string enemyTag = "Enemy";

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject rangeAttackPrefab;
    public Transform rangeAttackFirepoint;
    
    
    void Start()
    {
        InvokeRepeating("updateTarget", 0f, 0.5f);
    }

    
    void Update()
    {
        if (target == null)
            return;

        if (fireCountdown <= 0f) {
            shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

    }

    void shoot() {
       GameObject rangeAttack_object= (GameObject) Instantiate(rangeAttackPrefab, rangeAttackFirepoint.position, rangeAttackFirepoint.rotation);
        rangeattack rangeattack = rangeAttack_object.GetComponent<rangeattack>();

        if (rangeattack != null) {
            rangeattack.chase(target);
        }
    }

    void updateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position , enemy.transform.position);

            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else {
                target = null;
            }
        }
    }
    
    // draw the attack range of the character selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
