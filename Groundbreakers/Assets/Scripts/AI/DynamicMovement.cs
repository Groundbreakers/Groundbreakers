namespace AI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class DynamicMovement : MonoBehaviour
    {
        private static readonly int Direction = Animator.StringToHash("Direction");

        private static IEnumerable<Transform> targets;

        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speed = 1.0f; // temp

        public Animator animator;

        private NavigationMap navigator;

        private Tilemap map;

        [ShowInInspector]
        private List<Vector3> pathBuffer;

        /// <summary>
        ///     The Destination grid position.
        /// </summary>
        [ShowInInspector]
        private Vector3 goalGrid;

        private Vector3 lastGoalGrid;

        /// <summary>
        ///     The target grid, usually the target should be adjacent grids of current.
        /// </summary>
        private Vector3 nextGrid;

        // TODO: Fix this.
        private bool mad;

        private bool attacking;

        #region Public Functions

        public void MoveToward(Vector3 pos)
        {
            var dir = pos - this.transform.position;

            this.SetDirection(dir);

            // Update the new target grid
            this.nextGrid = pos;
        }

        public void OnTilesChange(Vector3 first, Vector3 second)
        {
        }

        public void OnTileChange(Vector3 first)
        {
        }

        #endregion

        #region Unity callbacks

        protected void OnEnable()
        {
            this.animator = this.GetComponent<Animator>();

            // Naive approach
            var tilemap = GameObject.Find("Tilemap");
            this.navigator = tilemap.GetComponent<NavigationMap>();
            this.map = tilemap.GetComponent<Tilemap>();
        }

        protected void Start()
        {
            // Caching the targets
            targets = GameObject.Find("Indicators").GetComponent<SpawnIndicators>().GetDefendPoints();

            this.nextGrid = this.transform.position;
        }

        protected void FixedUpdate()
        {
            if (this.IsMoving())
            {
                this.UpdateMoving();
            }
            else
            {
                if (this.CheckHasReachedGoal())
                {
                    return;
                }

                if (this.attacking)
                {
                    this.UpdateAttacking();
                }
                else
                {
                    this.UpdateStopping();
                }
            }
        }

        #endregion

        #region Internal Functions

        /// <summary>
        ///     Progress to next grid
        /// </summary>
        private void UpdateMoving()
        {
            var step = Time.fixedDeltaTime * this.speed;

            this.transform.position = Vector3.MoveTowards(this.transform.position, this.nextGrid, step);
        }

        /// <summary>
        ///     The update stopping.
        /// </summary>
        private void UpdateStopping()
        {
            //if (this.mad)
            //{
            //    // fucking check if has valid normal path
            //    var normalPath = this.navigator.Search(this.transform.position, this.goalGrid, false).ToList();

            //    if (normalPath.Count > 0)
            //    {
            //        this.mad = false;
            //    }
            //}

            // TODO: Refactor this shit,
            var path = this.navigator.Search(this.transform.position, this.goalGrid, this.mad).ToList();

            if (path.Count == 0)
            {
                // This happens when no valid path is made.
                this.mad = true;
                return;
            }

            path.RemoveAt(0);
            var next = path.First();
            var blockade = this.map.GetBlockadeAt(next);

            if (blockade)
            {
                // Should damage it
                this.StartAttack(blockade);
                return;
            }

            this.MoveToward(next);
        }

        private void UpdateAttacking()
        {

        }

        /// <summary>
        ///     Check if has reached the destination, if so, destroy self.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        private bool CheckHasReachedGoal()
        {
            this.goalGrid = this.FindGoal();

            if (this.transform.position != this.goalGrid)
            {
                return false;
            }

            Destroy(this.gameObject);
            return true;
        }

        /// <summary>
        ///     The find goal.
        /// </summary>
        /// <returns>
        ///     The <see cref="Vector3" />.
        /// </returns>
        private Vector3 FindGoal()
        {
            Assert.IsTrue(targets.Any());

            var end = targets.OrderBy(pos => Vector3.SqrMagnitude(pos.position - this.transform.position)).First();

            return end.position;
        }

        /// <summary>
        ///     Check if the object is moving.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> If this component is moving.
        /// </returns>
        private bool IsMoving()
        {
            return Vector3.Distance(this.transform.position, this.nextGrid) > Mathf.Epsilon;
        }

        /// <summary>
        ///     Update animator's direction.
        /// </summary>
        /// <param name="dir">
        ///     The direction vector
        /// </param>
        private void SetDirection(Vector3 dir)
        {
            var yAbs = Mathf.Abs(dir.y);
            var xAbs = Mathf.Abs(dir.x);

            if (yAbs > xAbs && dir.y > 0)
            {
                this.animator.SetInteger(Direction, 0); // Up
            }
            else if (yAbs < xAbs && dir.x > 0)
            {
                this.animator.SetInteger(Direction, 1); // Right
            }
            else if (yAbs > xAbs && dir.y < 0)
            {
                this.animator.SetInteger(Direction, 2); // Down
            }
            else if (yAbs < xAbs && dir.x < 0)
            {
                this.animator.SetInteger(Direction, 3); // Left
            }
        }

        private void StartAttack(GameObject blockade)
        {
            if (this.attacking)
            {
                return;
            }

            var dir = blockade.transform.position - this.transform.position;
            Debug.Log(dir);
            this.SetDirection(dir);

            var b = blockade.GetComponent<Blockade>();

            Assert.IsNotNull(b);

            this.StartCoroutine(this.Attack(b));
        }

        private IEnumerator Attack(Blockade blockade)
        {
            animator.SetBool("Attacking", true);
            this.attacking = true;

            yield return new WaitForSeconds(1.0f);

            if (blockade)
            {
                blockade.DeliverDamage();
            }

            this.attacking = false;
            animator.SetBool("Attacking", false);
        }

        #endregion
    }
}