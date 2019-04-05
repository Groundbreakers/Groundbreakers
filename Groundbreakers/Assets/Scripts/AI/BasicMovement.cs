namespace AI
{
    using System;

    using UnityEngine;

    public class BasicMovement : MonoBehaviour
    {
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speed = 1.0f;

        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speedMultiplier = 1.0f;

        private Vector3 target = Vector3.zero;

        private void Start()
        {
            this.target = this.transform.position + new Vector3(1, 6);
        }

        private void FixedUpdate()
        {
            if (this.IsMoving())
            {
                var step = Time.fixedDeltaTime * this.speed * this.speedMultiplier;
                var position = this.transform.position;

                this.transform.position = Vector3.MoveTowards(position, this.target, step);
            }
        }

        private bool IsMoving()
        {
            return Vector3.Distance(this.transform.position, this.target) > Mathf.Epsilon;
        }
    }
}