using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Bat_Behavior : MonoBehaviour
{
    // Player's health
    private float playerhealth = 0; // 0 is a default, change later

    // Stats
    public float strength = 1;
    public float health = 1;
    public float speed = 10f;
    public float damage = 1;

    // Animation + death effect
    public GameObject deathEffect;

    // Waypoints
    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        target = Waypoints.points[0];
        //+Change when player health is ready+ playerhealth = "PlayerObject".health;
    }

    void Update()
    {
        // Move the enemy towards the target waypoint
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Check if the waypoint has been reached
        if (Vector3.Distance(transform.position, target.position) <= 0.3f)
        {
            GetNextWaypoint();
        }

        // Check health, die if health <= 0. Spawns a death effect.
        if (health <= 0)
        {
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3);
            Destroy(gameObject);
        }
    }

    // Collision detector for projectiles
    void OnCollisionEnter(Collision col)
    {
        // If projectile, destroy the projectile and do damage calculation. Firebat is ARMORED, and takes damage from armor penetration.
        if (col.gameObject.tag == "Projectile")
        {
            Destroy(col.gameObject);
            //+Change this line to the proper projectile script+ health -= col.gameObject."ProjectileScript".armorpen
        }
    }

    // Get the next waypoint and update the index
    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // When the enemy reaches the end of its path
    void EndPath()
    {
        playerhealth -= damage;
        Destroy(gameObject);
    }
}
