namespace Assets.Scripts
{
    using Assets.Scripts.Enemies;

    using UnityEngine;

    public class RangeAttack : MonoBehaviour
    {
        public float speed = 70f;

        public Transform target;

        public int damage = 10;

        public int armorpen = 2;

        public bool hit;

        public void Chase(Transform _target)
        {
            this.target = _target;
        }

        public void Update()
        {
            if (this.target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            Vector3 direction = this.target.position - this.transform.position;
            float distancePerFrame = this.speed * Time.deltaTime;

            transform.Translate(direction.normalized * distancePerFrame, Space.World);
        }

        // Deals damage to the enemies
        public void OnTriggerEnter2D(Collider2D hitTarget)
        {
            if (hitTarget.gameObject.tag == "Enemy")
            {
                if (!hit)
                {
                    hitTarget.gameObject.GetComponent<EnemyGeneric>().DamageEnemy(this.damage, this.armorpen, 1, false);
                    //hitTarget.gameObject.GetComponent<Enemy_Generic>().StunEnemy((float)0.2);
                }

                this.hit = true;
                Destroy(this.gameObject);
            }
        }
    }
}