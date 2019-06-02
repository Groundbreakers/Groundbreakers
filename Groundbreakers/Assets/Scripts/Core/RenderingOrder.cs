namespace Core
{
    using TileMaps;

    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RenderingOrder : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private TileStatus status;

        private void OnEnable()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.status = this.GetComponent<TileStatus>();
        }

        private void Update()
        {
            var y = 8 - this.transform.position.y;

            if (this.status.IsMoving)
            {
                this.transform.localScale = new Vector3(2.0f, 2.0f);
                this.spriteRenderer.sortingLayerName = "HUD";
                this.UpdateSortingOrderInChildren(1, "HUD");
            }
            else
            {
                this.transform.localScale = new Vector3(1.0f, 1.0f);
                this.spriteRenderer.sortingLayerName = "GroundTiles";
                this.UpdateSortingOrderInChildren(1, "GroundTiles");
            }
        }

        private void UpdateSortingOrderInChildren(int order, string name)
        {
            foreach (Transform child in this.transform)
            {
                var render = child.GetComponent<SpriteRenderer>();
                render.sortingLayerName = name;
                render.sortingOrder = 1;
            }
        }
    }
}