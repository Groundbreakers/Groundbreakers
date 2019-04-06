namespace AI
{
    using System.Collections.Generic;

    using UnityEngine;

    public class BasicMovement : MonoBehaviour
    {
        private static readonly int Direction = Animator.StringToHash("Direction");

        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speed = 1.0f; // temp

        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speedMultiplier = 1.0f;

        private Animator animator;

        /// <summary>
        /// The target grid, usually the target should be adjacent grids of current.
        /// </summary>
        private Vector3 target = Vector3.zero;

        /// <summary>
        /// The current direction of the object, this is different from unity direction.
        /// </summary>
        private Vector3 direction = Vector3.down;

        #region Public Functions

        public void MoveStraight(Vector3 dir)
        {
            this.SetDirection(dir);

            // Update the new target grid
            this.target = this.transform.position + dir.normalized;
        }

        #endregion

        #region Unity callbacks

        private void Awake()
        {
            this.animator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            this.target = this.transform.position + new Vector3(1, -1);
        }

        private void FixedUpdate()
        {
            if (this.IsMoving())
            {
                var step = Time.fixedDeltaTime * this.speed * this.speedMultiplier;
                var position = this.transform.position;

                this.transform.position = Vector3.MoveTowards(position, this.target, step);
            }
            else
            {
                // this.MoveStraight(new Vector3(Random.Range(-1.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0));
            }
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Check if the object is moving. 
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/> If this component is moving. 
        /// </returns>
        private bool IsMoving()
        {
            return Vector3.Distance(this.transform.position, this.target) > Mathf.Epsilon;
        }

        private void SetDirection(Vector3 dir)
        {
            this.direction = dir;

            // Update animator
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && dir.y > 0)
            {
                this.animator.SetInteger(Direction, 0); // Up
            }
            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x) && dir.x > 0)
            {
                this.animator.SetInteger(Direction, 1); // Right
            }
            else if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x) && dir.y < 0)
            {
                this.animator.SetInteger(Direction, 2); // Down
            }
            else if (Mathf.Abs(dir.y) < Mathf.Abs(dir.x) && dir.x < 0)
            {
                this.animator.SetInteger(Direction, 3); // Left
            }
        }

        #endregion
    }
}