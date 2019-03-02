using Assets.Enemies.Scripts;

using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public int armorpen;

    public int damage;

    public bool hit;

    public float speed = 3f;

    public Transform target;

    private bool _break;

    private bool blight;

    private bool burn;

    private Vector3 direction;

    // private Enemy_Generic enemyGeneric;
    private float FLOATING_DAMAGE = 0.2f;

    private bool mark;

    private bool net;

    private bool purge;

    private bool slow;

    private bool stun;

    // access functions
    public void chase(Transform _target)
    {
        this.target = _target;
    }

    public void setBlight()
    {
        this.blight = true;
    }

    public void setBreak()
    {
        this._break = true;
    }

    public void setBurn()
    {
        this.burn = true;
    }

    public void setMark()
    {
        this.mark = true;
    }

    public void setNet()
    {
        this.net = true;
    }

    public void setPurge()
    {
        this.purge = true;
    }

    public void setSlow()
    {
        this.slow = true;
    }

    public void setStun()
    {
        this.stun = true;
    }

    public void statusEffectHandler(Collider2D hitTarget)
    {
        // prioritize armor breaking and purge 
        if (this._break == true)
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().attributes.Remove("Armored");
        }

        if (this.purge == true)
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().attributes.Remove("Aura");
            hitTarget.gameObject.GetComponent<Enemy_Generic>().attributes.Remove("Revenge");
        }

        if (this.mark != true)
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

    public void updateStats(int pow, int amp)
    {
        this.damage = Mathf.RoundToInt(
            pow * 50 * UnityEngine.Random.Range(1.0f - this.FLOATING_DAMAGE, 1.0f + this.FLOATING_DAMAGE));
        this.armorpen = amp;
    }

    void FixedUpdate()
    {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        float distancePerFrame = this.speed * Time.deltaTime;
        if (this.net == true) this.direction = this.target.position - this.transform.position;
        this.transform.Translate(this.direction.normalized * distancePerFrame, Space.World);
    }

    // Deals damage to the enemies
    void OnTriggerEnter2D(Collider2D hitTarget)
    {
        if (hitTarget.gameObject.tag == "Enemy")
        {
            if (!this.hit)
            {
                this.statusEffectHandler(hitTarget);
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

    void Start()
    {
        if (this.net == false) this.direction = this.target.position - this.transform.position;
    }
}