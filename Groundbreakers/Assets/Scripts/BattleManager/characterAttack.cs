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

    private float fireCountdown = 0f;

    private Transform target;

    private string attackMode = "default";

    private List<GameObject> targetedEnemies;
    // draw the attack range of the character selected

    void Awake() { targetedEnemies = new List<GameObject>(); }
    
    void Update() {
        if (target != null)
        {
            //calculate angle
            Vector2 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle = angle + 360f;
            }

            //check if it's pointing right
            if ((angle <= 360 && angle >= 315) || (angle >= 0 && angle < 45))
            {
                animator.SetBool("FacingRight", true);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingDown", false);
            }
            else if (angle >= 45 && angle < 135) //check if it's pointing up
            {
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingUp", true);
                animator.SetBool("FacingDown", false);
            }
            else if (angle >= 135 && angle < 225) //check if it's pointing left
            {
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingLeft", true);
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingDown", false);
            }
            else if (angle >= 225 && angle < 315) //check if it's pointing down
            {
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingDown", true);
            }
            //Debug.Log(angle);
        }
        this.fireCount();

        
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
            this.rangeAttackFirepoint.position,
            this.rangeAttackFirepoint.rotation);
        rangeattack rangeattack = rangeAttack_object.GetComponent<rangeattack>();

        if (rangeattack != null)
        {
            rangeattack.chase(this.target);
        }
    }

    void defaultMode() {
        if(targetedEnemies.Count != 0)
        {
            target = targetedEnemies[0].transform;
        }
        
    }
}
