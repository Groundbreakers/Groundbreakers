namespace TileMaps
{
    using InputSystem;

    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(TileStatus))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class HoverableTile : MonoBehaviour, ITileSelectMessageTarget
    {
        private SpriteRenderer rend;

        private TileStatus status;

        public void OnEnable()
        {
            this.rend = this.GetComponent<SpriteRenderer>();
            this.status = this.GetComponent<TileStatus>();

            this.SetAlpha();
        }

        public void OnMouseExit()
        {
            this.SetAlpha();
        }

        public void OnMouseEnter()
        {
            if (this.status.CanHover())
            {
                this.SetAlpha(0.5f);
            }
        }

        public void Select()
        {
            if (!this.status.CanHover())
            {
                return;
            }

            Debug.Log("Message 1 received");
        }

        public void Deselect()
        {
            Debug.Log("Message 2 received");
        }

        private void SetAlpha(float alpha = 1.0f)
        {
            var color = this.rend.color;
            this.rend.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}