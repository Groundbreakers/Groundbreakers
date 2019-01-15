// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BattleManager.cs" company="UCSC">
//   MIT
// </copyright>
// <summary>
//   The battle manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using UnityEngine;

    public class BattleManager : MonoBehaviour
    {
        private const int Columns = 8;

        private const int Rows = 8;

        private Transform boardHolder;

        /// <summary>
        ///     Temporary use this sprite as the tile on the tile map.
        /// </summary>
        [SerializeField]
        private Sprite sprite = null;

        public void Start()
        {
            this.SetupBoard();
        }

        public void Update()
        {
        }

        private void SetupBoard()
        {
            this.boardHolder = new GameObject("Map").transform;

            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    var instance = new GameObject();
                    var spriteRenderer = instance.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = this.sprite;

                    instance.transform.SetPositionAndRotation(new Vector3(i, j, 0f), Quaternion.identity);
                    instance.transform.SetParent(this.boardHolder);
                }
            }

            this.boardHolder.SetPositionAndRotation(new Vector3(-Columns / 2f, -Rows / 2f), Quaternion.identity);
        }
    }
}