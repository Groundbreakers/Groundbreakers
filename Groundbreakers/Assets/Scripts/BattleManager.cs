using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class BattleManager : MonoBehaviour
    {
        #region Unity Inspector Fields

        [SerializeField]
        private Tilemap tileMap;

        /// <summary>
        /// Define the size of the battlefield map.
        /// </summary>
        [SerializeField]
        [Range(1f, 8f)]
        private int dimension = 8;

        /// <summary>
        ///     Temporary use this sprite as the tile on the tile map.
        /// </summary>5
        [SerializeField]
        private Sprite sprite = null;

        #endregion

        #region Private Fields

        private Transform boardHolder;

        private TileType[,] tileData;

        /// <summary>
        /// The tile types.
        /// </summary>
        private enum TileType
        {
            Grass,

            Sand,

            Stone
        }

        #endregion

        #region Unity Callbacks

        public void Start()
        {
            this.SetupBoard();
        }

        public void Update()
        {
        }

        #endregion

        #region Internal Functions

        private void SetupBoard()
        {
            this.boardHolder = new GameObject("Map").transform;

            for (var i = 0; i < this.dimension; i++)
            {
                for (var j = 0; j < this.dimension; j++)
                {
                    var instance = new GameObject();
                    var spriteRenderer = instance.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = this.sprite;

                    instance.transform.SetPositionAndRotation(new Vector3(i, j, 0f), Quaternion.identity);
                    instance.transform.SetParent(this.boardHolder);
                }
            }

            this.boardHolder.SetPositionAndRotation(
                new Vector3(-this.dimension / 2f, -this.dimension / 2f),
                Quaternion.identity);
        }

        private void ClearBoard()
        {
        }

        #endregion
    }
}