namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using UnityEngine;

    using Random = UnityEngine.Random;

    [RequireComponent(typeof(SpriteRenderer))]
    public class TileBlock : MonoBehaviour
    {
        private static readonly int TotalBlocks = TerrainGenerator.Dimension * TerrainGenerator.Dimension;

        private static uint blocksReady;

        #region Inspector Variables

        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float enterDuration = 2.5f;

        [SerializeField]
        public Sprite CanDeployIcon;

        [SerializeField]
        public Sprite CanNotDeployIcon;

        [SerializeField]
        public Sprite OccupiedIcon;

        #endregion

        #region Internal Variables

        /// <summary>
        /// This is the sprite renderer that of this object
        /// </summary>
        private SpriteRenderer sprite;

        /// <summary>
        /// This contains the reference to the SpriteRenderer of the CHILD object.
        /// Used for Icon sprite when you hover your mouse over. 
        /// </summary>
        private SpriteRenderer hoverIconSprite;

        #endregion

        #region Unity Callbacks

        public void OnEnable()
        {
            var components = this.GetComponentsInChildren<SpriteRenderer>();
            this.sprite = components[0];
            this.hoverIconSprite = components[1];
        }

        public void FixedUpdate()
        {
            //if (!this.stabled && this.HasReachDestination())
            //{
            //    this.stabled = true;
            //    blocksReady++;

            //    // Check if all tiles are ready and emit event.
            //    if (blocksReady == TotalBlocks)
            //    {
            //        Debug.Log("All block ready");
            //        BattleManager.TriggerEvent("block ready");
            //    }
            //}
        }

        #endregion

        #region Public functions

        /// <summary>
        /// The the sorting order, typically called by GameMap when instantiating the tiles.
        /// </summary>
        /// <param name="z">
        /// The z.
        /// </param>
        public void SetSortingOrder(int z)
        {
            this.sprite.sortingOrder = z;
            this.hoverIconSprite.sortingOrder = z + 1;
        }
        #endregion
    }
}