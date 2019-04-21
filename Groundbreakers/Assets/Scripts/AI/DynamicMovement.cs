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

        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float speed = 1.0f; // temp

        private Animator animator;

        private NavigationMap navigator;

        private Tilemap map;

        [ShowInInspector]
        private List<Vector3> pathBuffer;

        /// <summary>
        ///     The target grid, usually the target should be adjacent grids of current.
        /// </summary>
        private Vector3 nextGrid = Vector3.zero;

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
            // Check if we need to re calculate path
            if (this.pathBuffer.Any(vec => vec == first || vec == second))
            {
                this.RecalculatePath();
            }
        }

        public void OnTileChange(Vector3 first)
        {
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
            this.RecalculatePath();
            var next = this.GetNextPoint();
            this.MoveToward(next);
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
                if (this.pathBuffer.Count == 0)
                {
                    Destroy(this.gameObject);
                }

                var next = this.GetNextPoint();
                this.MoveToward(next);

                this.map.OnTileOccupied(this.transform.position, false);
                this.map.OnTileOccupied(next);
            }
        }

        #endregion

        #region Internal Functions

        private Vector3 GetNextPoint()
        {
            var point = Vector3.zero;

            if (this.pathBuffer.Count <= 0)
            {
                return point;
            }

            point = this.pathBuffer[0];
            this.pathBuffer.RemoveAt(0);

            return point;
        }

        private void RecalculatePath()
        {
            // TODO: Fucking refactor this shit.
            var targets = GameObject.Find("Indicators").GetComponent<SpawnIndicators>().GetDefendPoints();
            var end = targets.OrderBy(pos => Vector3.Distance(this.transform.position, pos.position)).First();

            this.pathBuffer = this.navigator.Search(this.transform.position, end.position).ToList();
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