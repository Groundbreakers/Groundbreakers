namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class BattleManager : MonoBehaviour, IBattlePhaseHandler
    {
        #region Inspector Variables

        [SerializeField]
        private GameObject tilePrefab;

        #endregion

        #region Internal Variables

        private const uint Dimension = 8;

        private const float CellSize = 0.5f;

        /// <summary>
        /// The parent transform that hold all sub sprite for tiles.
        /// </summary>
        private Transform boardHolder;

        #endregion

        #region Unity Callbacks

        public void Start()
        {
            this.CreateGameBoard();
        }

        public void Update()
        {
            // Debug only
            if (Input.GetKeyDown("space"))
            {
                this.OnTilesEntering();
            }
        }

        #endregion

        #region IBattlePhaseHandler

        public void OnTilesEntering()
        {
            foreach (Transform child in this.boardHolder)
            {
                var tb = child.GetComponent<TileBlock>();
                tb.OnTilesEntering();
            }
        }

        public void OnBattling()
        {
            throw new System.NotImplementedException();
        }

        public void OnTilesExiting()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Internal functions

        private void CreateGameBoard()
        {
            this.boardHolder = new GameObject("Map").transform;

            // Spawning each individual tiles
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var instance = Instantiate(
                        this.tilePrefab, 
                        new Vector3(i * CellSize, j * CellSize, 0f), 
                        Quaternion.identity);

                    instance.transform.SetParent(this.boardHolder);
                }
            }

            // Locating the mother board
            float newX = -Dimension * CellSize / 2f; 
            float newY = -Dimension * CellSize / 2f;

            this.boardHolder.SetPositionAndRotation(
                new Vector3(newX, newY, 0f),
                Quaternion.identity);
        }

        private void CreateWayPoints()
        {
        }

        #endregion
    }
}