namespace TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using UnityEngine;

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
        private readonly List<Transform> defendPoints = new List<Transform>();

        /// <summary>
        ///     Caching the list of attacking points (i.e. The enemy spawn points).
        /// </summary>
        private readonly List<Transform> spawnPoints = new List<Transform>();

        /// <summary>
        ///     The get Spawn to Endpoint pair as a tuple.
        /// </summary>
        /// <example>
        ///     foreach (var x in numbers.Zip(words, Tuple.Create))
        ///     {
        ///         Console.WriteLine(x.Item1 + x.Item2);
        ///     }
        /// </example>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<Tuple<Transform, Transform>> GetPairs()
        {
            return this.spawnPoints.Zip(this.defendPoints, Tuple.Create);
        }

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
            RearrangeObjects(this.spawnPoints, TileData.Dimension - 1);
            RearrangeObjects(this.defendPoints, 0);
        }

        /// <summary>
        ///     Re-positioning the list of transforms. Pick one column in the row.
        /// </summary>
        /// <param name="objects">
        ///     The objects: An List of Transforms.
        /// </param>
        /// <param name="row">
        ///     The row: The row on the 8 by 8 tiles.
        /// </param>
        private static void RearrangeObjects(List<Transform> objects, int row)
        {
            var n = objects.Count;
            var xs = Enumerable.Range(0, TileData.Dimension).OrderBy(x => Random.value).Take(n).ToList();

            xs.Sort();

            var i = 0;
            foreach (var trans in objects)
            {
                trans.SetPositionAndRotation(new Vector3(xs[i], row), Quaternion.identity);
                i++;
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