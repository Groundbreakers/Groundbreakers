namespace TileMaps
{
    using System.Collections;
    using System.Linq;

    using AI;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <summary>
    ///     This component provides API for controlling dynamic terrain changes.
    /// </summary>
    public class DynamicTerrainController : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private GameObject mushroomPrefab;

        private Tilemap tilemap;

        private TerrainGenerator generator;

        #region Public APIs

        /// <summary>
        ///     Re-roll the middle 6 layer of the map.
        ///     According to the discussion, we can only.
        /// </summary>
        [Button]
        public void RerollMap()
        {
            // Should generate somewhat a new map
            this.generator.Initialize();

            // Ignore top and bottom row
            for (var i = 0; i < Tilemap.Dimension; i++)
            {
                for (var j = 1; j < Tilemap.Dimension - 1; j++)
                {
                    var newType = this.generator.GetTileTypeAt(i, j);

                    this.UpdateTileIfNecessary(new Vector3(i, j), newType);
                }
            }

            foreach (var pos in this.generator.GetMushroomLocations())
            {
                // Skip bounds
                if (pos.y <= 0 || pos.y >= 7)
                {
                    continue;
                }

                // Skip water
                if (this.generator.GetTileTypeAt(pos.x, pos.y) == Tiles.Water)
                {
                    continue;
                }

                var block = this.tilemap.GetTileBlockAt(pos);

                Instantiate(this.mushroomPrefab, block.transform);
            }
        }

        [Button]
        public void StartEarthQuake()
        {
            this.StartCoroutine(this.BeginEarthQuake());
        }

        #endregion

        private void OnEnable()
        {
            this.tilemap = this.GetComponent<Tilemap>();
            this.generator = this.GetComponent<TerrainGenerator>();
        }

        private void UpdateTileIfNecessary(Vector3 pos, Tiles newType)
        {
            var currentStatus = this.tilemap.GetCachedTileStatusAt(pos);

            // Skip water tiles
            if (currentStatus.GetTileType() == Tiles.Water)
            {
                return;
            }

            // Should skip this too?
            if (newType == Tiles.Water)
            {
                return;
            }

            var block = this.tilemap.GetTileBlockAt(pos);

            if (!block)
            {
                Debug.Log(pos);
            }

            if (newType == Tiles.HighGround)
            {
                this.CreateEnterAnimation(block);
            }

            // this.tilemap.ChangeTileAt(pos, newType);
            block.GetComponent<DynamicTileBlock>().ChangeTileType(newType);
        }

        #region Earth Quake

        private IEnumerator BeginEarthQuake()
        {
            const float Speed = 0.25f;

            for (var i = 0; i < Tilemap.Dimension; i++)
            {
                this.ShakeRow(i);
                this.StunEnemiesAtRow(i);

                yield return new WaitForSeconds(Speed);
            }
        }

        private void ShakeRow(int row)
        {
            for (var x = 0; x < Tilemap.Dimension; x++)
            {
                var block = this.tilemap.GetTileAt(x, row);



                block.transform.DOShakePosition(0.2f, 0.2f);

                // block.transform.DOMoveY(row, 0.1f).SetEase(Ease.InBounce);
            }
        }

        /// <summary>
        ///     This is different from the TileEnter animation, but pretty similar. 
        /// </summary>
        /// <param name="block">
        ///     The block.
        /// </param>
        private void CreateEnterAnimation(GameObject block)
        {
            const float Duration = 1.0f;
            const float MaxDelay = 0.25f;
            var offset = new Vector3(0.0f, 5.0f);

            var sprite = block.GetComponent<SpriteRenderer>();

            // Relocate block
            var position = block.transform.position;
            var ori = position;
            position += offset;
            block.transform.position = position;

            // Generate animation
            var delay = Random.Range(0.0f, MaxDelay);

            block.transform.DOMove(ori, Duration)
                .SetEase(Ease.OutExpo)
                .SetDelay(delay);
        }

        private void StunEnemiesAtRow(int row)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            var stunTime = 4.0f;

            var xs = enemies.Where(enemy => enemy.transform.position.y - row < 0.5f);

            foreach (var e in xs)
            {
                e.GetComponent<DynamicMovement>().StunEnemy(stunTime);
            }
        }

        #endregion
    }
}