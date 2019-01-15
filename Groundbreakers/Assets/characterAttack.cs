using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAttack : MonoBehaviour
{
    public Transform target;
    public float range = 15f;

    public string enemyTag = "Enemy"; 

    void Start()
    {
        InvokeRepeating("updateTarget", 0f, 0.5f);
    }

    
    void Update()
    {
        if (target == null)
            return;

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
