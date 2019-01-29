namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    public class CharacterAttack : MonoBehaviour
    {
        public Animator animator;

        public string enemyTag = "Enemy";

        public float fireRate = 1f;

        public float range = 15f;

        public Transform rangeAttackFirepoint;

        public GameObject rangeAttackPrefab;

        private string attackMode = "default";

        private float fireCountdown;

        private Transform target;

        private List<GameObject> targetedEnemies;

        // draw the attack range of the character selected
        private void Awake()
        {
            this.targetedEnemies = new List<GameObject>();
        }

        private void defaultMode()
        {
            var enemies = GameObject.FindGameObjectsWithTag(this.enemyTag);
            var shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (var enemy in enemies)
            {
                var distanceToEnemy = Vector2.Distance(this.transform.position, enemy.transform.position);

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
        }

        private void fireCount()
        {
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

        private void OnMouseOver()
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
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                this.targetedEnemies.Add(other.gameObject);
                this.updateTarget();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                this.targetedEnemies.Remove(other.gameObject);
                this.updateTarget();
            }
        }

        private void Update()
        {
            this.fireCount();

            if (this.target != null)
            {
                // calculate angle
                Vector2 direction = this.target.transform.position - this.transform.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (angle < 0)
                {
                    angle = angle + 360f;
                }

                // check if it's pointing right
                if (angle <= 360 && angle >= 315 || angle >= 0 && angle < 45)
                {
                    this.animator.SetBool("FacingRight", true);
                    this.animator.SetBool("FacingLeft", false);
                    this.animator.SetBool("FacingUp", false);
                    this.animator.SetBool("FacingDown", false);
                }
                else if (angle >= 45 && angle < 135)
                {
                    // check if it's pointing up
                    this.animator.SetBool("FacingRight", false);
                    this.animator.SetBool("FacingLeft", false);
                    this.animator.SetBool("FacingUp", true);
                    this.animator.SetBool("FacingDown", false);
                }
                else if (angle >= 135 && angle < 225)
                {
                    // check if it's pointing left
                    this.animator.SetBool("FacingRight", false);
                    this.animator.SetBool("FacingLeft", true);
                    this.animator.SetBool("FacingUp", false);
                    this.animator.SetBool("FacingDown", false);
                }
                else if (angle >= 225 && angle < 315)
                {
                    // check if it's pointing down
                    this.animator.SetBool("FacingRight", false);
                    this.animator.SetBool("FacingLeft", false);
                    this.animator.SetBool("FacingUp", false);
                    this.animator.SetBool("FacingDown", true);
                }

                // Debug.Log(angle);
            }
        }

        // update the closest target in range
        private void updateTarget()
        {
            if (this.attackMode == "default")
            {
                this.defaultMode();
            }
        }

        // Instantiate and and chase the target
        private void shoot()
        {
            GameObject rangeAttack_object = (GameObject)Instantiate(
                this.rangeAttackPrefab,
                this.rangeAttackFirepoint.position,
                this.rangeAttackFirepoint.rotation);
            RangeAttack rangeAttack = rangeAttack_object.GetComponent<RangeAttack>();

            if (rangeAttack != null)
            {
                rangeAttack.Chase(this.target);
            }
        }
    }
}