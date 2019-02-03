using System;
using Assets.Enemies.Scripts;
using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public int armorpen = 2;

    public int damage = 10;

    public bool hit;

    public float speed = 70f;

    public Transform target;

    private Vector2 direction;

    public void chase(Transform _target) {
        this.target = _target;
    }

    // Deals damage to the enemies
    void OnTriggerEnter2D(Collider2D hitTarget) {
        if (hitTarget.gameObject.tag == "Enemy")
        {
            if (!this.hit)
            {
                hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false);

                // hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy((float)0.2);
            }

            this.hit = true;
            Destroy(this.gameObject);
        }
    }

    void Start() {
        this.direction = this.target.position - this.transform.position;
    }

    void Update() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        float distancePerFrame = this.speed * Time.deltaTime;

        // transform.Translate(direction.normalized * distancePerFrame, Space.World); // if bullet tracking
        this.transform.Translate(Time.deltaTime * this.direction.normalized * distancePerFrame, Space.World);
    }
}
