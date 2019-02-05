using Assets.Enemies.Scripts;
using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public int armorpen = 2;

    public int damage = 10;

    public bool hit;

    public float speed = 70f;

    public Transform target;

    private float angle;

    private Vector2 direction;

    private ModuleTemplate module;

    private GameObject moduleObject;

    private Vector2 rotatedDirection;

    // set this bullet's target 
    public void chase(Transform _target) {
        this.target = _target;
    }

    // set this bullet's angle 
    public void damageReduction(int _damageReduction) {
        this.damage -= _damageReduction;
    }

    // set this bullet's angle 
    public void setAngle(float _angle) {
        this.angle = _angle;
    }

    // Deals damage to the enemies
    void OnTriggerEnter2D(Collider2D hitTarget) {
        if (hitTarget.gameObject.tag == "Enemy")
        {
            if (!this.hit)
            {
                hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false);

                // hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy((float)0.2);
            }

            this.hit = true;
            Destroy(this.gameObject);
        }
    }

    void Start() {
        this.moduleObject = GameObject.Find("BattleManager");
        this.module = this.moduleObject.GetComponent<ModuleTemplate>();
        this.direction = this.target.position - this.transform.position;
        this.rotatedDirection = this.vectorRotation(this.angle) * (this.target.position - this.transform.position);
    }

    void Update() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (this.module.multiShotAE != true)
        {
            float distancePerFrame = this.speed * Time.deltaTime;

            // transform.Translate(direction.normalized * distancePerFrame, Space.World); // if bullet tracking
            this.transform.Translate(Time.deltaTime * this.direction.normalized * distancePerFrame, Space.World);
        }
        else
        {
            float distancePerFrame = this.speed * Time.deltaTime;
            this.transform.Translate(Time.deltaTime * this.rotatedDirection.normalized * distancePerFrame, Space.World);
        }
    }

    Quaternion vectorRotation(float angle) {
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
