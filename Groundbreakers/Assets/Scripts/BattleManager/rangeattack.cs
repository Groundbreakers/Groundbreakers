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

    private Boolean burn;

    public Boolean blight;

    public Boolean slow;

    private Vector3 direction;

    //private Enemy_Generic enemyGeneric;

    private float FLOATING_DAMAGE = 0.2f;

    public void chase(Transform _target)
    {
        this.target = _target;
    }

    public void setBurn()
    {
        this.burn = true;
    }

    public void setBlight()
    {
        this.blight = true;
    }

    public void setSlow()
    {
        this.slow = true;
    }

    public void updateStats(int pow, int amp)
    {
        this.damage = Mathf.RoundToInt(pow * 50 * UnityEngine.Random.Range(1.0f - this.FLOATING_DAMAGE, 1.0f + this.FLOATING_DAMAGE));
        this.armorpen = amp;
    }

    void Start() {
       direction = this.target.position - this.transform.position;
    }

    void FixedUpdate() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

       float distancePerFrame = this.speed * Time.deltaTime;

        this.transform.Translate( direction.normalized * distancePerFrame, Space.World);
    }
      
    // Deals damage to the enemies
    void OnTriggerEnter2D(Collider2D hitTarget)
    {
        if (hitTarget.gameObject.tag == "Enemy")
        {
            if (!this.hit)
            {
                hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false);
               
                if (this.burn == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().isBurned == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().BurnEnemy();
                }

                if (this.blight == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().isBlighted == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().BlightEnemy();
                }

                if (this.slow == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().isSlowed == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().SlowEnemy(0.5f);
                }
            }
            this.hit = true;
            Destroy(this.gameObject);
        }
        if (hitTarget.gameObject.tag == "Boss")
        {
            if (!this.hit)
            {
                hitTarget.gameObject.GetComponent<Cetus_Script>().DamageCetus(this.damage, this.armorpen, 1);
            }
            this.hit = true;
            Destroy(this.gameObject);
        }
    }
}
