namespace TileMaps
{
    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    ///     This Component can be attached to GameObjects. When this GO is created, instantly
    ///     set the grid where this GO stands to Not passable. And restore its original state when
    ///     this destroy.
    ///     The GameObject mush have a Parent Object, and it must have a Tile Tag.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Blockade : MonoBehaviour
    {
        /// <summary>
        ///     The Weight on this blockade. Used by A* search algorithm.
        /// </summary>
        [InfoBox("weight 0 is equivalent to not passable.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float weight;

        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float hitPoint = 4.0f;

        [SerializeField]
        private GameObject blockadeDeath;

        private SpriteRenderer sprite;

        private Tilemap gameMap;

        private float previousWeight;

        public float GetHitPoint()
        {
            return this.hitPoint;
        }

        public void DeliverDamage()
        {
            this.hitPoint -= 1.0f;

            this.transform.DOShakePosition(0.5f, 0.1f);
            this.sprite.DOColor(Color.cyan, 1.0f);

            if (this.hitPoint <= 0.0f)
            {
                var rubble = Instantiate(
                    this.blockadeDeath, 
                    this.transform.position, 
                    Quaternion.identity);

                GameObject.Destroy(rubble, 1.0f);
                GameObject.Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
            // Inefficient but acceptable here.
            this.gameMap = GameObject.FindObjectOfType<Tilemap>();

            this.sprite = this.GetComponent<SpriteRenderer>();

            this.hitPoint = 4.0f;
        }

        private void Start()
        {
            var parent = this.transform.parent;

            Assert.IsNotNull(parent);
            Assert.IsTrue(parent.CompareTag("Tile"));

            var status = parent.GetComponent<TileStatus>();

            // Store original status and update new.
            this.previousWeight = status.Weight;

            status.SetCanPass(false);
            status.Weight = this.weight;
        }

        private void OnDestroy()
        {
            var parent = this.transform.parent;

            Assert.IsNotNull(parent);
            Assert.IsTrue(parent.CompareTag("Tile"));

            var status = parent.GetComponent<TileStatus>();

            status.SetCanPass(true);
            status.Weight = this.previousWeight;

            this.gameMap.ChangeTileAt(parent.position, Tiles.Stone);
        }
    }
}