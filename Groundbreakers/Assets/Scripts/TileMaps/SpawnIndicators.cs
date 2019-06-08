namespace TileMaps
{
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using UnityEngine;
    using UnityEngine.Assertions;

    using Random = UnityEngine.Random;

    /// <inheritdoc />
    /// <summary>
    ///     A behavior class that should be attached to the "Indicators" Prefab Only. Randomly
    ///     rearrange indicators when created. 
    /// </summary>
    public class SpawnIndicators : MonoBehaviour
    {
        /// <summary>
        ///     Caching the list of defending points.
        /// </summary>
        private List<Transform> defendPoints = new List<Transform>();

        /// <summary>
        ///     Caching the list of attacking points (i.e. The enemy spawn points).
        /// </summary>
        private List<Transform> spawnPoints = new List<Transform>();

        /// <summary>
        ///     Should be used by enemies to do some check.
        /// </summary>
        /// <returns>
        ///     The <see cref="Transform"/>.
        /// </returns>
        public IEnumerable<Transform> GetDefendPoints()
        {
            return this.defendPoints;
        }

        /// <summary>
        ///     Should be called by the CombatManager's setup script.
        /// </summary>
        [Button]
        public void Initialize()
        {
            this.spawnPoints.Clear();
            this.defendPoints.Clear();

            this.InitializeIndicators();

            RearrangeObjectsBruteForce(this.spawnPoints, Tilemap.Dimension - 1);
            RearrangeObjectsBruteForce(this.defendPoints, 0);

            // Set the tile where the spawner locates not swap-able
            foreach (var go in this.spawnPoints.Concat(this.defendPoints))
            {
                var map = GameObject.Find("Tilemap").transform;

                var pos = go.transform.position;
                var index = (int)(pos.x * Tilemap.Dimension + pos.y);

                var block = map.GetChild(index);

                block.GetComponent<TileStatus>().SetAsSpawn();
            }
        }

        /// <summary>
        ///     Show all the Indicators, which was originally hidden.
        /// </summary>
        [Button]
        public void RevealIndicators()
        {
            foreach (var go in this.spawnPoints.Concat(this.defendPoints))
            {
                go.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        public void HideIndicators()
        {
            foreach (var go in this.spawnPoints.Concat(this.defendPoints))
            {
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        /// <summary>
        ///     Do a search to check if there is any valid path from start points to end point.
        ///     Should be checked each time map is generated. And if no valid path, you should
        ///     consider re-roll or break it.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool"/>.
        /// </returns>
        public bool HasValidPath()
        {
            var navMesh = FindObjectOfType<NavigationMap>();

            Assert.IsNotNull(navMesh);

            foreach (var spawnPoint in this.spawnPoints)
            {
                var goal = this.defendPoints.OrderBy(pos => Vector3.Distance(spawnPoint.position, pos.position)).First();

                var path = navMesh.Search(spawnPoint.position, goal.position);

                // Empty path => No valid path => Return False. 
                if (!path.Any())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Re-positioning the list of transforms on to passable tiles.
        ///     Pick one column in the row.
        /// </summary>
        /// <param name="objects">
        ///     The objects: An List of Transforms.
        /// </param>
        /// <param name="row">
        ///     The row: The row on the 8 by 8 tiles.
        /// </param>
        private static void RearrangeObjectsBruteForce(
            IReadOnlyCollection<Transform> objects, 
            int row)
        {
            var tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
            var n = objects.Count;

            var xs = Enumerable.Range(0, 8).OrderBy(x => Random.value).Take(n).ToList();

            xs.Sort();

            var j = 0;
            foreach (var trans in objects)
            {
                var newPos = new Vector3(xs[j], row);

                trans.SetPositionAndRotation(newPos, Quaternion.identity);

                tilemap.ChangeTileAt(newPos, Tiles.Stone);

                j++;
            }
        }

        /// <summary>
        ///     Preprocess the list of indicators.
        /// </summary>
        private void InitializeIndicators()
        {
            var atkNames = new[] { "Spawn Point A", "Spawn Point B" };
            var defNames = new[] { "Defend Point A", "Defend Point B" };

            foreach (var str in atkNames)
            {
                var go = this.transform.Find(str);

                this.spawnPoints.Add(go);
            }

            foreach (var str in defNames)
            {
                var go = this.transform.Find(str);

                this.defendPoints.Add(go);
            }
        }
    }
}