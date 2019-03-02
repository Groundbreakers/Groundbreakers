using System.Collections.Generic;

using UnityEngine;

public class characterAttack : MonoBehaviour
{
    public Animator animator;

    public string enemyTag = "Enemy";

    public float fireRate = 1f;

    public float range = 15f;

    public Transform rangeAttackFirepoint;

    public GameObject rangeAttackPrefab;

    public string stance = "Gun";

    private string attackMode = "default";

    private float fireCountdown = 0f;

    private Vector3 firePoint;

    private bool isChanging = false;

    // draw the attack range of the character selected
    private CircleCollider2D myCollider;

    private Transform target;

    private List<GameObject> targetedEnemies;

    private characterAttributes trickster;

    public void change()
    {
        this.GetComponent<characterAttributes>().Transform();
        if (GameObject.Find("CharactersPanel") != null)
        {
            GameObject.Find("CharactersPanel").GetComponent<CharacterManager>().UpdatePanel();
        }

        this.isChanging = true;
        if (this.stance.Equals("Melee"))
        {
            this.stance = "Gun";
            this.animator.SetBool("Transition", true);
            this.animator.SetBool("Sitting", true);
            this.animator.SetBool("Standing", false);
            this.trickster.gun();
            this.myCollider.radius = this.trickster.RNG + .5f; // or whatever radius you want.
        }
        else
        {
            this.stance = "Melee";
            this.animator.SetBool("Transition", true);
            this.animator.SetBool("Sitting", false);
            this.animator.SetBool("Standing", true);
            this.trickster.melee();
            this.myCollider.radius = this.trickster.RNG + .5f; // or whatever radius you want.
        }
    }

    void Awake()
    {
        this.targetedEnemies = new List<GameObject>();
        this.myCollider = this.GetComponent<CircleCollider2D>();
        this.trickster = this.GetComponent<characterAttributes>();
        this.firePoint = this.rangeAttackFirepoint.position;
    }

    void defaultMode()
    {
        if (this.targetedEnemies.Count != 0)
        {
            this.target = this.targetedEnemies[0].transform;
        }
        else
        {
            this.target = null;
        }
    }

    void fireCount()
    {
        this.myCollider.radius = this.trickster.RNG + .5f; // or whatever radius you want.
        if (this.target == null)
        {
            this.animator.SetBool("Firing", false);
            return;
        }

        if (this.fireCountdown <= 0f)
        {
            this.animator.SetBool("Firing", true);
            this.shoot();

            this.fireCountdown = 1f / this.fireRate;
        }

        this.fireCountdown -= Time.deltaTime;
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown("r"))
        {
            this.attackMode = "multi-shot";

            // Debug.Log(this.attackMode);
        }
        else if (Input.GetKeyDown("n"))
        {
            this.attackMode = "default";

            // Debug.Log(this.attackMode);
        }
    }

    // if an enemy enters in range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            this.targetedEnemies.Add(other.gameObject);
            this.updateTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            this.targetedEnemies.Remove(other.gameObject);
            this.updateTarget();
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

    // Instantiate and and chase the target
    void shoot()
    {
        GameObject rangeAttack_object = (GameObject)Instantiate(
            this.rangeAttackPrefab,
            this.firePoint,
            this.rangeAttackFirepoint.rotation);

        rangeattack rangeattack = rangeAttack_object.GetComponent<rangeattack>();
        if (this.stance.Equals("Melee"))
        {
            rangeattack.speed = 10f;
            rangeAttack_object.GetComponent<SpriteRenderer>().enabled = false;
        }else
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

    void Start()
    {
        this.fireRate = this.trickster.ROF * .5f;
        this.animator.SetFloat("FireRate", this.fireRate);
        this.myCollider.radius = this.trickster.RNG + .5f;
    }

    void Update()
    {
        if (this.target != null && !this.isChanging)
        {
            // calculate angle
            Vector2 direction = this.target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle = angle + 360f;
            }

            // check if it's pointing right
            if (!this.trickster.disabled)
            {
                if ((angle <= 360 && angle >= 315) || (angle >= 0 && angle < 45))
                {
                    this.animator.SetBool("FacingRight", true);
                    this.animator.SetBool("FacingLeft", false);
                    this.animator.SetBool("FacingUp", false);
                    this.animator.SetBool("FacingDown", false);
                    this.firePoint = new Vector3(
                        this.gameObject.transform.position.x + .475f,
                        this.gameObject.transform.position.y + .5f,
                        this.gameObject.transform.position.z);
                }
                else if (angle >= 45 && angle < 135)
                {
                    // check if it's pointing up
                    this.animator.SetBool("FacingRight", false);
                    this.animator.SetBool("FacingLeft", false);
                    this.animator.SetBool("FacingUp", true);
                    this.animator.SetBool("FacingDown", false);
                    this.firePoint = new Vector3(
                        this.gameObject.transform.position.x,
                        this.gameObject.transform.position.y + .6f,
                        this.gameObject.transform.position.z);
                }
                else if (angle >= 135 && angle < 225)
                {
                    // check if it's pointing left
                    this.animator.SetBool("FacingRight", false);
                    this.animator.SetBool("FacingLeft", true);
                    this.animator.SetBool("FacingUp", false);
                    this.animator.SetBool("FacingDown", false);
                    this.firePoint = new Vector3(
                        this.gameObject.transform.position.x - .475f,
                        this.gameObject.transform.position.y + .5f,
                        this.gameObject.transform.position.z);
                }
                else if (angle >= 225 && angle < 315)
                {
                    // check if it's pointing down
                    this.animator.SetBool("FacingRight", false);
                    this.animator.SetBool("FacingLeft", false);
                    this.animator.SetBool("FacingUp", false);
                    this.animator.SetBool("FacingDown", true);
                    this.firePoint = new Vector3(
                        this.gameObject.transform.position.x - .05f,
                        this.gameObject.transform.position.y - .05f,
                        this.gameObject.transform.position.z);
                }

                // Debug.Log(angle);
            }
        }

        if (!this.isChanging && !this.trickster.disabled)
        {
            this.fireCount();
        }

        if (this.trickster.disabled)
        {
            this.myCollider.radius = 0;
            this.target = null;
        }

        if (!this.animator.GetBool("Transition"))
        {
            this.isChanging = false;
        }
    }

    // update the closest target in range
    void updateTarget()
    {
        if (this.attackMode == "default") this.defaultMode();
    }
}