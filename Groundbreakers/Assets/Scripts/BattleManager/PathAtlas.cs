namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.U2D;

    [RequireComponent(typeof(SpriteRenderer))]
    public class PathAtlas : MonoBehaviour
    {
        #region Inspector Values

        [SerializeField]
        private SpriteAtlas spriteAtlas;

        #endregion

        #region Internal Variables

        private SpriteRenderer sprite;

        #endregion

        #region Public Functions

        /// <summary>
        /// I am currently using string as the function argument, which is not ideal but work.
        /// </summary>
        /// <param name="filename">
        /// The name.
        /// </param>
        public void SetDirection(string filename = "road_1")
        {
            this.sprite.sprite = this.spriteAtlas.GetSprite(filename);
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.sprite = this.GetComponent<SpriteRenderer>();
            this.SetDirection();
        }

        #endregion
    }
}