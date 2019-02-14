//namespace Assets.Scripts
//{
//    using UnityEngine;

//    using TG = TerrainGenerator;

//    [RequireComponent(typeof(SpriteRenderer))]
//    public class TileBlock : MonoBehaviour
//    {
//        #region Inspector Variables



//        #endregion

//        #region Internal Variables

//        /// <summary>
//        /// This is the sprite renderer that of this object
//        /// </summary>
//        private SpriteRenderer sprite;

//        /// <summary>
//        /// This contains the reference to the SpriteRenderer of the CHILD object.
//        /// Used for Icon sprite when you hover your mouse over. 
//        /// </summary>
//        private SpriteRenderer hoverIconSprite;

//        #endregion

//        #region Unity Callbacks

//        public void OnEnable()
//        {
//            var components = this.GetComponentsInChildren<SpriteRenderer>();
//            this.sprite = components[0];
//            this.hoverIconSprite = components[1];
//        }

//        #endregion
//    }
//}