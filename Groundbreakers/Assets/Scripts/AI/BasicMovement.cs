namespace AI
{
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using TileMaps;

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

        private NavigationMap navigator;

        private Tilemap map;

        [ShowInInspector]
        private List<Vector3> pathBuffer;

        private Vector3 goal;

        /// <summary>
        ///     The target grid, usually the target should be adjacent grids of current.
        /// </summary>
        private Vector3 nextGrid = Vector3.zero;

        /// <summary>
        ///     The current direction of the object, this is different from unity direction.
        /// </summary>
        private Vector3 direction = Vector3.down;

        private bool freezing;

        #region Public Functions

        public void MoveToward(Vector3 pos)
        {
            var dir = pos - this.transform.position;

            this.SetDirection(dir);

            // Update the new target grid
            this.nextGrid = pos;
        }

        public void OnTileChange(Vector3 pos)
        {
            // Check if we need to re calculate path
            if (this.pathBuffer.Any(vec => vec == pos))
            {
                this.RecalculatePath();
                var next = this.GetNextPoint();
                this.MoveToward(next);
            }
        }

        public void Freeze()
        {
            this.freezing = true;
        }

        public void Unfreeze()
        {
            this.freezing = false;
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
            if (this.freezing)
            {
                return;
            }

            if (this.IsMoving())
            {
                var step = Time.fixedDeltaTime * this.speed * this.speedMultiplier;
                var position = this.transform.position;

                this.transform.position = Vector3.MoveTowards(
                    position, 
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

        private void SetDirection(Vector3 dir)
        {
            this.direction = dir;

            // Update animator direction
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