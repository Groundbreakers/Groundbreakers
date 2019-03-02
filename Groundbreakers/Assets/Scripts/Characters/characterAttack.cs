using UnityEngine;
using System.Collections.Generic;

public class characterAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";

    public float fireRate = 1f;

    public float range = 15f;

    public Transform rangeAttackFirepoint;
    
    public GameObject rangeAttackPrefab;

    public Animator animator;
    
    public string stance = "Gun";

    private characterAttributes trickster; 
    private bool isChanging = false;
    private Vector3 firePoint;
    private float fireCountdown = 0f;

    private Transform target;

    private string attackMode = "default";

    private List<GameObject> targetedEnemies;
    // draw the attack range of the character selected

    private CircleCollider2D myCollider;

    void Awake()
    {
        targetedEnemies = new List<GameObject>();
        myCollider = GetComponent<CircleCollider2D>();
        trickster = GetComponent<characterAttributes>();
        firePoint = rangeAttackFirepoint.position;
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
            if(!trickster.disabled)
            {
                if ((angle <= 360 && angle >= 315) || (angle >= 0 && angle < 45))
                {
                    animator.SetBool("FacingRight", true);
                    animator.SetBool("FacingLeft", false);
                    animator.SetBool("FacingUp", false);
                    animator.SetBool("FacingDown", false);
                    firePoint = new Vector3(gameObject.transform.position.x + .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                }
                else if (angle >= 45 && angle < 135) //check if it's pointing up
                {
                    animator.SetBool("FacingRight", false);
                    animator.SetBool("FacingLeft", false);
                    animator.SetBool("FacingUp", true);
                    animator.SetBool("FacingDown", false);
                    firePoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + .6f, gameObject.transform.position.z);
                }
                else if (angle >= 135 && angle < 225) //check if it's pointing left
                {
                    animator.SetBool("FacingRight", false);
                    animator.SetBool("FacingLeft", true);
                    animator.SetBool("FacingUp", false);
                    animator.SetBool("FacingDown", false);
                    firePoint = new Vector3(gameObject.transform.position.x - .475f, gameObject.transform.position.y + .5f, gameObject.transform.position.z);
                }
                else if (angle >= 225 && angle < 315) //check if it's pointing down
                {
                    animator.SetBool("FacingRight", false);
                    animator.SetBool("FacingLeft", false);
                    animator.SetBool("FacingUp", false);
                    animator.SetBool("FacingDown", true);
                    firePoint = new Vector3(gameObject.transform.position.x - .05f, gameObject.transform.position.y - .05f, gameObject.transform.position.z);
                }
                //Debug.Log(angle);
            }

        }

        if (!isChanging && !trickster.disabled)
        {
            this.fireCount();
        }

        if(trickster.disabled)
        {
            myCollider.radius = 0;
            target = null;
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
    void updateTarget() {
        if (this.attackMode == "default") defaultMode();
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown("r")) {
            this.attackMode = "multi-shot";
            //Debug.Log(this.attackMode);
        }
        else if (Input.GetKeyDown("n")) {
            this.attackMode = "default";
            //Debug.Log(this.attackMode);
        }
    }

    void fireCount() {
        myCollider.radius = trickster.RNG + .5f; // or whatever radius you want.
        if (this.target == null)
        {
            animator.SetBool("Firing", false);
            return;
        }

        if (this.fireCountdown <= 0f)
        {
            animator.SetBool("Firing", true);
            this.shoot();

            this.fireCountdown = 1f / this.fireRate;
        }

        this.fireCountdown -= Time.deltaTime;
    }

    // Instantiate and and chase the target
    void shoot()
    {
        GameObject rangeAttack_object = (GameObject)Instantiate(
            this.rangeAttackPrefab,
            firePoint,
            this.rangeAttackFirepoint.rotation);
       
        rangeattack rangeattack = rangeAttack_object.GetComponent<rangeattack>();
        if(stance.Equals("Melee"))
        {
            rangeAttack_object.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (rangeattack != null)
        {
            if (this.trickster.burnSE == true) rangeattack.setBurn();
            if (this.trickster.blightSE == true) rangeattack.setBlight();
            if (this.trickster.slowSE == true) rangeattack.setSlow();
            if (this.trickster.stunSE == true) rangeattack.setStun();
            if (this.trickster.markSE== true) rangeattack.setMark();
            if (this.trickster.breakSE == true) rangeattack.setBreak();


            rangeattack.updateStats(trickster.POW, trickster.AMP);
            rangeattack.chase(this.target);
        }
    }

    void defaultMode() {
        if(targetedEnemies.Count != 0)
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
}
