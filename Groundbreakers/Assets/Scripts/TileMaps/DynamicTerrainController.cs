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
        private GameObject highGroundPrefab;

        [Required]
        [SerializeField]
        private GameObject mushroomPrefab;

        [Required]
        [SerializeField]
        private GameObject grassPrefab;

        private Tilemap tilemap;

        private TerrainGenerator generator;

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
                if (pos.y < 1 && pos.y > Tilemap.Dimension - 1)
                {
                    return;
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

        private void UpdateTileIfNecessary(Vector3 pos, Tiles newType)
        {
            var currentStatus = this.tilemap.GetTileStatusAt(pos);

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

            // Clear all children
            foreach (Transform child in block.transform)
            {
                Destroy(child.gameObject);
            }

            if (newType == Tiles.HighGround)
            {
                Instantiate(this.highGroundPrefab, block.transform);
            }

            if (newType == Tiles.Grass)
            {
                Instantiate(this.grassPrefab, block.transform);
            }

            this.tilemap.ChangeTileAt(pos, newType);
        }

        private void OnEnable()
        {
            this.tilemap = this.GetComponent<Tilemap>();
            this.generator = this.GetComponent<TerrainGenerator>();
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
                var block = this.tilemap.GetTileBlockAt(x, row);

                block.transform.DOShakePosition(0.2f, 0.2f);

                // block.transform.DOMoveY(row, 0.1f).SetEase(Ease.InBounce);
            }
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