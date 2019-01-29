namespace Assets.Scripts
{
    using UnityEngine;

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

        public void OnEnable()
        {
            this.sprite = this.GetComponent<SpriteRenderer>();

            this.originalColor = this.sprite.color;
        }

        public void OnMouseOver()
        {
            if (BattleManager.GameState != BattleManager.Stages.Combating)
            {
                return;
            }

            if (!this.enabled)
            {
                return;
            }

            this.sprite.color = this.mouseOverColor;

            BattleManager.Instance.SetCurrentSelectedTile(this.transform);
        }

        public void OnMouseExit()
        {
            if (BattleManager.GameState != BattleManager.Stages.Combating)
            {
                return;
            }

            if (!this.enabled)
            {
                return;
            }

            this.sprite.color = this.originalColor;

            BattleManager.Instance.SetCurrentSelectedTile(null);
        }
    }
}