namespace TileMaps
{
    using System.Linq;

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

        private SpriteRenderer sprite;

        private Tilemap gameMap;

        private bool previousCanPass;

        private void OnEnable()
        {
            // Inefficient but acceptable here.
            this.gameMap = GameObject.FindObjectOfType<Tilemap>();

            this.sprite = this.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            var parent = this.transform.parent;

            Assert.IsNotNull(parent);
            Assert.IsTrue(parent.CompareTag("Tile"));

            var status = parent.GetComponent<TileStatus>();

            // Store original status and update new.
            this.previousCanPass = status.CanPass();
            status.SetCanPass(false);
            status.Weight = this.weight;

            Tilemap.OnTileChanges(parent.position);
        }

        private void OnDestroy()
        {
            var parent = this.transform.parent;

            Assert.IsNotNull(parent);
            Assert.IsTrue(parent.CompareTag("Tile"));

            var status = parent.GetComponent<TileStatus>();

            status.SetCanPass(this.previousCanPass);

            Tilemap.OnTileChanges(parent.position);
        }

        // TMP
        // TODO: do it properly
        private void FixedUpdate()
        {
            // TMP Solution, very inefficent
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Any(go => Vector3.Distance(go.transform.position, this.transform.position) < Mathf.Epsilon))
            {
                Destroy(this.gameObject);
            }
        }
    }
}