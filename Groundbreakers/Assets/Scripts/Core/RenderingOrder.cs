namespace Core
{
    using UnityEngine;

    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RenderingOrder : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            this.spriteRenderer.sortingOrder = Mathf.CeilToInt(this.transform.position.z);
        }
    }
}