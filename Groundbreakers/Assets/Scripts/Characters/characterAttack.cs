using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Assets.Scripts;

using TileMaps;

public class characterAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";

    public float fireRate = 1f;

    public float range = 15f;

    public Transform rangeAttackFirepoint;

    public GameObject rangeAttackPrefab;

    public Animator animator;

    private LineRenderer lineRenderer;

    //public LineRenderer lineRenderer;

    public MonoBehaviour ability1;

    public string stance;
    public GameObject hitbox;
    private characterAttributes trickster;
    private bool isChanging = false;
    private Vector3 firePoint;
    private float fireCountdown = 0f;
    private bool isStunned = false;

    private Transform target;

    private string attackMode = "default";

    private List<GameObject> targetedEnemies;
    // draw the attack range of the character selected

    private CircleCollider2D myCollider;

    /// <summary>
    /// Ivan: we keep a reference to the weapon GameObject here. Note the second child of Character
    /// GameObject must be Weapons
    /// </summary>
    private GameObject rangedWeapon;

    /// <summary>
    /// Written by Ivan
    /// </summary>
    private void OnEnable()
    {
        var obj = this.transform.Find("RangedWeapon");

        if (obj)
        {
            this.rangedWeapon = obj.gameObject; // should check error
        }
    }

    void Awake()
    {
        targetedEnemies = new List<GameObject>();
        myCollider = GetComponent<CircleCollider2D>();
        trickster = GetComponent<characterAttributes>();
       firePoint = rangeAttackFirepoint.position;
       
        if (this.gameObject.GetComponent<Laserbeam>() != null)
       {
           lineRenderer = this.gameObject.GetComponent<Laserbeam>().lineRenderer;
       }
    }

    void Start()
    {
        fireRate = trickster.ROF * .5f;
        animator.SetFloat("FireRate", fireRate);
        myCollider.radius = trickster.RNG + .5f;
    }

    void Update() {
        
        if (target != null && !isChanging)
        {
            //calculate angle
            Vector2 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle = angle + 360f;
            }

            //check if it's pointing right
            if (!trickster.disabled && !isStunned)
            {

                    if ((angle <= 360 && angle >= 337.5) || (angle >= 0 && angle < 22.5))
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", true);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", false);

                        firePoint = new Vector3(gameObject.transform.position.x + .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                    }
                    else if ((angle >= 22.5 && angle <= 67.5))//check if it's upper right
                    {
                        animator.SetBool("FacingUpperRight", true);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", false);

                        firePoint = new Vector3(gameObject.transform.position.x + .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                    }
                    else if ((angle < 337.5 && angle >= 292.5)) //LowerRight
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", true);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", false);

                        firePoint = new Vector3(gameObject.transform.position.x + .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                    }
                    else if (angle > 67.5 && angle < 112.5) //check if it's pointing up
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", true);
                        animator.SetBool("FacingDown", false);
                        firePoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + .6f, gameObject.transform.position.z);
                    }
                    else if (angle >= 157.5 && angle < 202.5) //check if it's pointing left
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", true);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", false);
                        firePoint = new Vector3(gameObject.transform.position.x - .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                    }
                    else if ((angle >= 112.5 && angle < 157.5))//check if it's upper Left
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", true);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", false);

                        firePoint = new Vector3(gameObject.transform.position.x + .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                    }
                    else if ((angle >= 202.5 && angle < 247.5)) //LowerLeft
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", true);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", false);

                        firePoint = new Vector3(gameObject.transform.position.x + .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                    }
                    else if (angle >= 247.5 && angle < 292.5) //check if it's pointing down
                    {
                        animator.SetBool("FacingUpperRight", false);
                        animator.SetBool("FacingLowerRight", false);
                        animator.SetBool("FacingRight", false);
                        animator.SetBool("FacingUpperLeft", false);
                        animator.SetBool("FacingLowerLeft", false);
                        animator.SetBool("FacingLeft", false);
                        animator.SetBool("FacingUp", false);
                        animator.SetBool("FacingDown", true);
                        firePoint = new Vector3(gameObject.transform.position.x - .05f, gameObject.transform.position.y - .05f, gameObject.transform.position.z);
                    }
                
                
                

                //Debug.Log(angle);
            }

        }

        if (!isChanging && !trickster.disabled && !isStunned)
        {
            this.fireCount();
        }

        if (trickster.disabled)
        {
            myCollider.radius = 0;
            target = null;
        }

        if (isStunned)
        {
            animator.SetBool("Firing", false);
        }

        if (!animator.GetBool("Transition"))
        {
            isChanging = false;

        }
    }

    //if an enemy enters in range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
                targetedEnemies.Add(other.gameObject);
                updateTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
                targetedEnemies.Remove(other.gameObject);
                updateTarget();
        }
    }

    // update the closest target in range
    void updateTarget()
    {
        if (this.attackMode == "default") defaultMode();
    }


    void fireCount()
    {


        myCollider.radius = trickster.RNG + .5f; // or whatever radius you want.
        if (this.target == null)
        {
            animator.SetBool("Firing", false);

           if (this.gameObject.GetComponent<Laserbeam>() != null)
            {
                this.lineRenderer.enabled = false;
            }

            return;
        }
        else
        {

           if (this.gameObject.GetComponentInChildren<BulletLauncher>().type == BulletLauncher.Type.Laser)
           {
                this.lineRenderer.enabled = true;
                this.fireCountdown = 0f;
           }
        }

        if (this.fireCountdown <= 0f)
        {
            animator.SetBool("Firing", true);

            this.PerformAttack();

            if (GameObject.Find("RangedWeapon").GetComponent<BulletLauncher>().type != BulletLauncher.Type.Laser)
            {
                this.fireCountdown = 1f / this.fireRate;
            }
        }

        this.fireCountdown -= Time.deltaTime;
    }

    public void PerformAttack() {
      
        if (this.stance.Equals("Melee"))
        {
            this.MeleeAttack();
        }
        else if (this.gameObject.GetComponentInChildren<BulletLauncher>().type == BulletLauncher.Type.Laser && this.gameObject.GetComponent<Laserbeam>() != null)
        {
            this.gameObject.GetComponent<Laserbeam>().LaserAttack(this.target);
          
        }
        else
        {
            this.RangedAttack();
        }
    }


    /// <summary>
    /// This is the alternative way of shooting bullets written by Ivan.
    /// </summary>
    public void RangedAttack()
    {
        this.rangedWeapon.SendMessage("FireAt", this.target);
    }

    public void MeleeAttack()
    {
        MeleeManager meleeattack = hitbox.GetComponent<MeleeManager>();
        // should use a melee attack module here. This is temp solution :(
        this.setMeleeStatusAttributes(meleeattack);

    }

    private void setMeleeStatusAttributes(MeleeManager meleeattack)
    {
        meleeattack.setAttack(trickster.POW);
        meleeattack.setArmorPen(trickster.AMP);
        if (this.trickster.burnSE == true) meleeattack.setBurn();
        if (this.trickster.blightSE == true) meleeattack.setBlight();
        if (this.trickster.slowSE == true) meleeattack.setSlow();
        if (this.trickster.stunSE == true) meleeattack.setStun();
        if (this.trickster.markSE == true) meleeattack.setMark();
        if (this.trickster.breakSE == true) meleeattack.setBreak();
        if (this.trickster.netSE == true) meleeattack.setNet();
        if (this.trickster.purgeSE == true) meleeattack.setPurge();
    }



    // Instantiate and and chase the target
    void shoot()
    {
        GameObject rangeAttack_object = (GameObject)Instantiate(
            this.rangeAttackPrefab,
            firePoint,
            this.rangeAttackFirepoint.rotation);

        rangeattack rangeattack = rangeAttack_object.GetComponent<rangeattack>();
        if (stance.Equals("Melee"))
        {
            rangeattack.speed = 10f;
            rangeAttack_object.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            rangeattack.speed = 3f;
        }
        if (rangeattack != null)
        {
            this.setProjectileStatusAttributes(rangeattack);
            rangeattack.updateStats(trickster.POW, trickster.AMP);
            rangeattack.chase(this.target);
        }
    }

    void defaultMode()
    {
        if (targetedEnemies.Count != 0)
        {
            target = targetedEnemies[0].transform;
        }
        else
        {
            target = null;
        }

    }

    public void change()
    {
        this.GetComponent<characterAttributes>().Transform();
        if (GameObject.Find("CharactersPanel") != null)
        {
            GameObject.Find("CharactersPanel").GetComponent<CharacterManager>().UpdatePanel();
        }

        isChanging = true;
        if (stance.Equals("Melee"))
        {
            stance = "Gun";
            animator.SetBool("Transition", true);
            animator.SetBool("Sitting", true);
            animator.SetBool("Standing", false);
            trickster.gun();
            myCollider.radius = trickster.RNG + .5f; // or whatever radius you want.
        }
        else
        {
            stance = "Melee";
            animator.SetBool("Transition", true);
            animator.SetBool("Sitting", false);
            animator.SetBool("Standing", true);
            trickster.melee();
            myCollider.radius = trickster.RNG + .5f; // or whatever radius you want.
        }
    }

    private void setProjectileStatusAttributes(rangeattack rangeattack)
    {
        if (this.trickster.burnSE == true) rangeattack.setBurn();
        if (this.trickster.blightSE == true) rangeattack.setBlight();
        if (this.trickster.slowSE == true) rangeattack.setSlow();
        if (this.trickster.stunSE == true) rangeattack.setStun();
        if (this.trickster.markSE == true) rangeattack.setMark();
        if (this.trickster.breakSE == true) rangeattack.setBreak();
        if (this.trickster.netSE == true) rangeattack.setNet();
        if (this.trickster.purgeSE == true) rangeattack.setPurge();
    }

    public void stun(int time)
    {
        isStunned = true;
        StartCoroutine(stunDuration(time));

    }

    private IEnumerator stunDuration(int time)
    {
        yield return new WaitForSeconds(time);
        isStunned = false;
    }

    public void setStance()
    {
        if(stance == "Melee")
        {
            animator.SetBool("Sitting", false);
            animator.SetBool("Standing", true);
        }
        else
        {
            animator.SetBool("Sitting", true);
            animator.SetBool("Standing", false);
        }
        
    }
}
