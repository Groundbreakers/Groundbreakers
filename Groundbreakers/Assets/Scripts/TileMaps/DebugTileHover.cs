namespace TileMaps
{
    using UnityEngine;

    public class DebugTileHover : MonoBehaviour
    {
        private SpriteRenderer rend;

        private void OnEnable()
        {
            this.rend = this.GetComponent<SpriteRenderer>();
        }

        private void OnMouseOver()
        {
            var color = this.rend.color;
            this.rend.color = new Color(color.r, color.g, color.b, 0.5f);
        }

        private void OnMouseExit()
        {
            var color = this.rend.color;
            this.rend.color = new Color(color.r, color.g, color.b, 1.0f);
        }
    }
}