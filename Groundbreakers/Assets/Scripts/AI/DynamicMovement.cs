namespace AI
{
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;

    public class DynamicMovement : MonoBehaviour
    {
        private static readonly int Direction = Animator.StringToHash("Direction");

        private static IEnumerable<Transform> targets;

        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speed = 1.0f; // temp

        private Animator animator;

        private NavigationMap navigator;

        private Tilemap map;

        [ShowInInspector]
        private List<Vector3> pathBuffer;

        /// <summary>
        ///     The Destination grid position.
        /// </summary>
        private Vector3 goalGrid;

        private Vector3 lastGoalGrid;

        /// <summary>
        ///     The target grid, usually the target should be adjacent grids of current.
        /// </summary>
        private Vector3 nextGrid;

        // TODO: Fix this.
        private bool mad;

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
            return;

            // Check if we need to re calculate path
            if (this.pathBuffer.Any(vec => vec == first || vec == second))
            {
                this.RecalculatePath();
            }
        }

        public void OnTileChange(Vector3 first)
        {
            return;

            // Check if we need to re calculate path
            if (this.pathBuffer.Any(vec => vec == first))
            {
                this.RecalculatePath();
            }
        }

        #endregion

        #region Unity callbacks

        protected void OnEnable()
        {
            this.animator = this.GetComponent<Animator>();

            var tilemap = GameObject.Find("Tilemap");
            this.navigator = tilemap.GetComponent<NavigationMap>();
            this.map = tilemap.GetComponent<Tilemap>();
        }

        protected void Start()
        {
            // Caching the targets
            targets = GameObject.Find("Indicators").GetComponent<SpawnIndicators>().GetDefendPoints();

            this.goalGrid = this.FindGoal();

            Debug.Log("something");

            if (this.transform.position == this.goalGrid)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            var path = this.navigator.Search(
                    this.transform.position,
                    this.goalGrid,
                    false)
                .ToList();


            if (path.Count == 0)
            {
                return;
            }

            this.MoveToward(path.First());
        }

        protected void FixedUpdate()
        {
            if (TileController.Busy)
            {
                return;
            }

            if (this.IsMoving())
            {
                var step = Time.fixedDeltaTime * this.speed;

                this.transform.position = Vector3.MoveTowards(
                    this.transform.position, 
                    this.nextGrid, 
                    step);
            }
            else
            {
                this.goalGrid = this.FindGoal();

                Debug.Log("something");

                if (this.transform.position == this.goalGrid)
                {
                    GameObject.Destroy(this.gameObject);
                    return;
                }

                var path = this.navigator.Search(
                                   this.transform.position,
                                   this.goalGrid,
                                   false)
                               .ToList();


                if (path.Count == 0)
                {
                    return;
                }

                path.RemoveAt(0);
                this.MoveToward(path.First());
            }
        }

        #endregion

        #region Internal Functions

        private Vector3 GetNextPoint()
        {
            var point = this.pathBuffer[0];
            this.pathBuffer.RemoveAt(0);

            return point;
        }

        private void RecalculatePath()
        {
            this.goalGrid = this.FindGoal();

            this.pathBuffer = this.navigator.Search(
                this.transform.position, 
                this.goalGrid,
                false)
                .ToList();
        }

        private Vector3 FindGoal()
        {
            var end = targets.OrderBy(pos => Vector3.Distance(this.transform.position, pos.position)).First();

            return end.position;
        }

        private void UpdatePathIfGoalChanges()
        {
            this.goalGrid = this.FindGoal();

            if (!this.goalGrid.Equals(this.lastGoalGrid))
            {
                this.lastGoalGrid = this.goalGrid;

                this.RecalculatePath();
            }
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

        #endregion
    }
}