using System;

using Assets.Enemies.Scripts;

using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public float speed = 70f;

    public Transform target;
    
    public int damage;

    public int armorpen;

    public Boolean hit;

    public void chase(Transform _target) {
        this.target = _target;
    }

    public void updateStats(int pow, int amp)
    {
        damage = pow * 50;
        armorpen = amp;
    }

    void Update() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 direction = this.target.position - this.transform.position;
        float distancePerFrame = this.speed * Time.deltaTime;

        transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }
      
    // Deals damage to the enemies
    void OnTriggerEnter2D(Collider2D hitTarget)
    {
        if (hitTarget.gameObject.tag == "Enemy")
        {
            if (!hit)
            {
                hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false);
                //hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy((float)0.2);
            }
            this.hit = true;
            Destroy(this.gameObject);
        }
        if (hitTarget.gameObject.tag == "Boss")
        {
            if (!hit)
            {
                hitTarget.gameObject.GetComponent<Cetus_Script>().DamageCetus(this.damage, this.armorpen, 1);
            }
            this.hit = true;
            Destroy(this.gameObject);
        }
    }
}
