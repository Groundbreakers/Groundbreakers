namespace Assets.Scripts
{
    using System;

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

        #region Public Properties

        public enum Stages
        {
            Null,
            Entering,
            Combating,
            Exiting
        }

        public static Stages GameState { get; private set; } = Stages.Null;

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
                switch (GameState)
                {
                    case Stages.Null:
                        this.OnTilesEntering();
                        GameState = Stages.Entering;
                        break;
                    case Stages.Entering:
                        GameState = Stages.Combating;
                        break;
                    case Stages.Combating:
                        GameState = Stages.Exiting;
                        this.OnTilesExiting();
                        break;
                    case Stages.Exiting:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Debug.Log(GameState);
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
            foreach (Transform child in this.boardHolder)
            {
                var tb = child.GetComponent<TileBlock>();
                tb.OnTilesExiting();
            }
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