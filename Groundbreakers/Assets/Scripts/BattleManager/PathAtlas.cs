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

        #region Unity Callbacks

        public void OnEnable()
        {
            this.sprite = this.GetComponent<SpriteRenderer>();
            this.sprite.sprite = this.spriteAtlas.GetSprite("road_1");
        }

        #endregion
    }
}