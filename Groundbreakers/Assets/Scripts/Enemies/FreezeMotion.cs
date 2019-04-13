namespace Enemies
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class FreezeMotion : MonoBehaviour
    {
        private Rigidbody2D body;

        public static void FreezeAll()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<FreezeMotion>().Freeze();
            }
        }

        public static void ResumeAll()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<FreezeMotion>().Resume();
            }
        }

        private void OnEnable()
        {
            this.body = this.GetComponent<Rigidbody2D>();
        }

        private void Freeze()
        {
            this.body.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Resume()
        {
            this.body.constraints = RigidbodyConstraints2D.None;
        }

        private void Update()
        {
        }
    }
}