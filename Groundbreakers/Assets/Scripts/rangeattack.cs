namespace Assets.Scripts
{
    using Assets.Scripts.Enemies;

    using UnityEngine;

    public class Rangeattack : MonoBehaviour
    {
        public int armorpen = 2;

        public int damage = 10;

        public float speed = 70f;

        public Transform target;

        public void Chase(Transform _target)
        {
            this.target = _target;
        }

        // Deals damage to the enemies
        public void OnTriggerEnter2D(Collider2D hitTarget)
        {
            if (hitTarget.gameObject.tag == "Enemy")
            {
                hitTarget.gameObject.GetComponent<EnemyGeneric>().DamageEnemy(this.damage, this.armorpen, 1, false);

                // hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy((float)0.2);
                Destroy(this.gameObject);
            }
        }

        public void Update()
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
}