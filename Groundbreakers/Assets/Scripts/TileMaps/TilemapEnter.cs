namespace TileMaps
{
    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     Should attach this to Tile map GameObject.
    /// </summary>
    [RequireComponent(typeof(Tilemap))]
    public class TilemapEnter : MonoBehaviour
    {
        /// <summary>
        ///     The offset: it is the position of where you would like the object to travel from.
        /// </summary>
        [SerializeField]
        private Vector3 offset = new Vector3(0.0f, -4.0f);

        /// <summary>
        ///     The duration of object traveling in seconds.
        /// </summary>
        [SerializeField]
        [Range(0.5f, 5.0f)]
        private float duration = 1.75f;

        /// <summary>
        ///     The maximum random delay allowed in seconds.
        /// </summary>
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float maxDelay = 2.0f;

        private Tilemap tilemap;

        [Button]
        public void Begin()
        {
            foreach (Transform child in this.transform)
            {
                this.CreateEnterAnimation(child.gameObject);
            }
        }

        [Button]
        public void Terminate()
        {
            foreach (Transform child in this.transform)
            {
                this.CreateExistAnimation(child.gameObject);
            }
        }

        protected void OnEnable()
        {
            this.tilemap = this.GetComponent<Tilemap>();
        }

        private void CreateEnterAnimation(GameObject block)
        {
            var sprite = block.GetComponent<SpriteRenderer>();

            // Relocate block
            var position = block.transform.position;
            var ori = position;
            position += this.offset;
            block.transform.position = position;

            // Generate animation
            var delay = Random.Range(0.0f, this.maxDelay);

            block.transform.DOMove(ori, this.duration)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);

            sprite.DOFade(1.0f, this.duration)
                .SetEase(Ease.OutExpo)
                .SetDelay(delay);
        }

        private void CreateExistAnimation(GameObject block)
        {
            var sprite = block.GetComponent<SpriteRenderer>();

            var targetPos = block.transform.position - this.offset;

            var delay = Random.Range(0.0f, this.maxDelay);

            block.transform.DOMove(targetPos, this.duration)
                .SetEase(Ease.InBack)
                .SetDelay(delay);

            sprite.DOFade(0.0f, this.duration)
                .SetEase(Ease.InExpo)
                .SetDelay(delay)
                .OnComplete(() => Destroy(block));
        }
    }
}