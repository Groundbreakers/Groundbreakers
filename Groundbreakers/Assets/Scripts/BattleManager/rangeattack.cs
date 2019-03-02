using System;
using Assets.Enemies.Scripts;
using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public float speed = 70f;

    public Transform target;
    
    public int damage;

    public int armorpen;

    public bool hit;

    private bool burn;

    private bool blight;

    private bool slow;

    private bool stun;

    private bool mark;

    private bool _break;

    // private Boolean mark;

    private Vector3 direction;

    //private Enemy_Generic enemyGeneric;

    private float FLOATING_DAMAGE = 0.2f;
    
    // access functions
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

    public void setMark()
    {
        this.mark = true;
    }

    public void setStun()
    {
        this.stun = true;
    }

    public void setBreak()
    {
        this._break = true;
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
                // prioritize armor breaking
                if (this._break == true)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().attributes.Remove("Armored");
                   
                }

                if(this.mark != true)
                { 
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false, false);
                }
                else
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false, true);
                }

                if (this.burn == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().getIsBurned() == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().BurnEnemy();
                }

                if (this.blight == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().getIsBlighted() == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().BlightEnemy();
                }

                if (this.slow == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().getIsSlowed() == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().SlowEnemy(0.5f);
                }

                if (this.stun == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().getIsStunned() == false)
                {
                    hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy(0.5f);
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
