namespace Assets.Scripts
{
    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// The Tiles that can deploy behavior.
    /// Attach this component to any object if you wish to deploy the character on.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Deployable : MonoBehaviour
    {
        [SerializeField]
        private Color mouseOverColor = Color.white;

        private SpriteRenderer sprite;

        private Color originalColor;

        #region Unity Callbacks

        public void OnEnable()
        {
            this.sprite = this.GetComponent<SpriteRenderer>();

            this.originalColor = this.sprite.color;
        }

        public void OnDisable()
        {
            this.sprite.color = this.originalColor;
        }

        public void OnMouseOver()
        {
            if (BattleManager.GameState != BattleManager.Stages.Combating)
            {
                return;
            }

            BattleManager.Instance.SetCurrentSelectedTile(this.transform);

            if (!this.enabled)
            {
                return;
            }

            this.sprite.color = this.mouseOverColor;
        }

        public void OnMouseExit()
        {
            if (BattleManager.GameState != BattleManager.Stages.Combating)
            {
                return;
            }

            BattleManager.Instance.SetCurrentSelectedTile(null);

            if (!this.enabled)
            {
                return;
            }

            this.OnDisable();
        }

        #endregion
    }
}