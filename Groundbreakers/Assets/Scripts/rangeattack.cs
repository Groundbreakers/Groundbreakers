// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildManager.cs" company="UCSC">
//   MIT
// </copyright>
// <summary>
//   Javy Wu
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public int damage = 1;

    public float speed = 10f;

    Enemy_movement bullet;

    private Enemy_movement enemy;

    private Transform target;

    public void chase(Transform _target) {
        this.target = _target;
    }

    void hitTarget() {
        Destroy(this.gameObject);
    }

    // Deals damage to the enemies, but sometimes this.enemy.health is not updated ?
    void OnTriggerEnter2D(Collider2D target) {
        if (target.gameObject.tag == "Enemy")
        {
            this.enemy = FindObjectOfType<Enemy_movement>();

            if (this.enemy != null && this.enemy.health >= 0)
            {
                this.enemy.health -= this.damage;
                Debug.Log(this.enemy.health);
            }
        }
    }

    void Update() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 direction = this.target.position - this.transform.position;
        float distancePerFrame = this.speed * Time.deltaTime;

        if (direction.magnitude <= distancePerFrame)
        {
            this.hitTarget();
        }

        this.transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }
}
