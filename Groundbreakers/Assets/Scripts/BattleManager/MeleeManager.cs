using Assets.Enemies.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour
{
    public Collider2D hitbox;
    private List<GameObject> hitEnemies;

    private int armorpen;

    private int damage;

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


    public void setAttack(int i)
    {
        damage = i * 50;
    }

    public void setArmorPen(int i)
    {
        armorpen = i;
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


    // Start is called before the first frame update
    void Start()
    {
        hitEnemies = new List<GameObject>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hitbox.isTrigger)
        {
            hitEnemies.Clear();
        }
    }

    //if an enemy enters in range
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Enemy")
        {
            if (hitEnemies.Contains(other.gameObject))
            {
                return;
            }
            else
            {
                hitEnemies.Add(other.gameObject);
                statusEffectHandler(other);
                Debug.Log(damage);
            }
        }
    }

    public void statusEffectHandler(Collider2D hitTarget)
    {
        // prioritize armor breaking and purge 
        if (this._break == true)
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().breakEnemyArmor();
        }

        if (this.purge == true && hitTarget.gameObject.GetComponent<Enemy_Generic>().getIsPurged() == false)
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().purgeEnemy();
        }

        if (this.mark != true)
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, true, false);
        }
        else
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, true, true);
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



    void OnTriggerExit2D(Collider2D other)
    {

    }
}
