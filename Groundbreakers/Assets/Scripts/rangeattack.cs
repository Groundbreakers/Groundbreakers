using Assets.Enemies.Scripts;

using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public int armorpen = 2;

    public int damage = 1;

    public float speed = 10f;

    public Transform target;

    public void chase(Transform _target)
    {
        this.target = _target;
    }

    // Deals damage to the enemies
    private void OnTriggerEnter2D(Collider2D hitTarget)
    {
        if (hitTarget.gameObject.tag == "Enemy")
        {
            hitTarget.gameObject.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false);
            hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy((float)0.2);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        var direction = this.target.position - this.transform.position;
        var distancePerFrame = this.speed * Time.deltaTime;

        this.transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }
}