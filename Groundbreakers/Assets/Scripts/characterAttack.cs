using UnityEngine;

public class characterAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";

    public float fireRate = 1f;

    public float range = 15f;

    public Transform rangeAttackFirepoint;

    public GameObject rangeAttackPrefab;

    public Animator animator;

    private bool delay = false;

    private float fireCountdown = 0f;

    private Transform target;

    // draw the attack range of the character selected
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.range);
    }

    // Instantiate and and chase the target
    void shoot() {
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

    void Start() {
        this.InvokeRepeating("updateTarget", 0f, 0.1f);
    }

    void Update() {
        
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

    // update the closest target in range
    void updateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(this.enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(this.transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDistance <= this.range)
            {
                this.target = nearestEnemy.transform;
            }
            else
            {
                this.target = null;
            }
        }

        //update player position
        if(nearestEnemy != null)
        {
            //calculate angle
            Vector2 direction = nearestEnemy.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            if(angle < 0)
            {
                angle = angle + 360f; 
            }

            //check if it's pointing right
            if( (angle <= 360 && angle >= 315) || (angle >= 0 && angle < 45))
            {
                animator.SetBool("FacingRight", true);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingDown", false);
            }
            else if(angle >= 45  && angle < 135) //check if it's pointing up
            {
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingUp", true);
                animator.SetBool("FacingDown", false);
            }
            else if(angle >= 135 && angle < 225) //check if it's pointing left
            {
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingLeft", true);
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingDown", false);
            }
            else if(angle >= 225 && angle < 315) //check if it's pointing down
            {
                animator.SetBool("FacingRight", false);
                animator.SetBool("FacingLeft", false);
                animator.SetBool("FacingUp", false);
                animator.SetBool("FacingDown", true);
            }
            //Debug.Log(angle);
        }
       
    }
}
