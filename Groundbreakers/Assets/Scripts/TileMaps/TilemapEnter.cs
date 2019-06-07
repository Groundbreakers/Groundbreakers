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
        ///     Using default sprite material first, then change this back to outline material when
        ///     the entering animation is completed. 
        /// </summary>
        [SerializeField]
        private Material outlineMaterial;

        [SerializeField]
        private Material defaultMaterial;

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

        private void CreateEnterAnimation(GameObject block)
        {
            var sprite = block.GetComponent<SpriteRenderer>();

            var delay = Random.Range(0.0f, this.maxDelay);

            // Relocate block
            var position = block.transform.position;
            var ori = position;
            position += this.offset;
            block.transform.position = position;

            block.transform.DOMove(ori, this.duration)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);

            sprite.DOFade(1.0f, this.duration)
                .SetEase(Ease.OutExpo)
                .SetDelay(delay)
                .OnComplete(() => { sprite.material = this.outlineMaterial; });

            // Handle for high ground
            var status = block.GetComponent<TileStatus>();
            if (status)
            {
                var type = status.GetTileType();
                if (type == Tiles.HighGround)
                {
                    var item = block.transform.GetChild(0).GetComponent<SpriteRenderer>();

                    item.DOFade(1.0f, this.duration)
                        .SetEase(Ease.OutExpo)
                        .SetDelay(delay)
                        .OnComplete(() => { item.material = this.outlineMaterial; });
                }
            }
        }

        private void CreateExistAnimation(GameObject block)
        {
            var sprite = block.GetComponent<SpriteRenderer>();

            sprite.material = this.defaultMaterial;

            var targetPos = block.transform.position - this.offset;

            var delay = Random.Range(0.0f, this.maxDelay);

            block.transform.DOMove(targetPos, this.duration)
                .SetEase(Ease.InBack)
                .SetDelay(delay);

            sprite.DOFade(0.0f, this.duration)
                .SetEase(Ease.InExpo)
                .SetDelay(delay)
                .OnComplete(() => Destroy(block));

            // Handle for high ground
            var status = block.GetComponent<TileStatus>();
            if (status)
            {
                var type = status.GetTileType();
                if (type == Tiles.HighGround)
                {
                    var item = block.transform.GetChild(0).GetComponent<SpriteRenderer>();

                    item.material = this.defaultMaterial;

                    item.DOFade(0.0f, this.duration)
                        .SetEase(Ease.InExpo)
                        .SetDelay(delay);
                }
            }
        }
    }
}