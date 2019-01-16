using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    
    public int damage = 1;
    private Enemy_movement enemy;
    Enemy_movement bullet;


    public void chase(Transform _target) {
        this.target = _target;
    }

   
    void hitTarget() {
        Destroy(this.gameObject);
    }

    void Update() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 direction = this.target.position - this.transform.position;
        float distancePerFrame = this.speed * Time.deltaTime;

        if (direction.magnitude <= distancePerFrame)
        {
            this.hitTarget();

        }

        this.transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }

    // Deals damage to the enemies, but sometimes this.enemy.health is not updated ?
    void OnTriggerEnter2D(Collider2D target)
    {
        
        if (target.gameObject.tag == "Enemy")
        {
            enemy = GameObject.FindObjectOfType<Enemy_movement>();
            if (enemy != null && this.enemy.health >= 0)
            {
                
                this.enemy.health -= damage;
                Debug.Log(enemy.health);
            }
            

        }
    }


}
