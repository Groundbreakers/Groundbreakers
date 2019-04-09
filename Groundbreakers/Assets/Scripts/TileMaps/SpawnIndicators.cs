namespace TileMaps
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     A behavior class that should be attached to the "Indicators" Prefab Only. Randomly
    ///     rearrange indicators when created.
    /// </summary>
    public class SpawnIndicators : MonoBehaviour
    {
        private readonly List<Transform> defendPoints = new List<Transform>();

        private readonly List<Transform> spawnPoints = new List<Transform>();

        /// <summary>
        ///     Re-positioning the list of transforms. Pick one column in the row.
        /// </summary>
        /// <param name="objects">
        ///     The objects: An List of Transforms.
        /// </param>
        /// <param name="row">
        ///     The row: The row on the 8 by 8 tiles.
        /// </param>
        private static void RearrangeObjects(IEnumerable<Transform> objects, int row)
        {
            var xs = Enumerable.Range(0, TileData.Dimension).OrderBy(x => Random.value).ToArray();

            var i = 0;
            foreach (var trans in objects)
            {
                trans.SetPositionAndRotation(new Vector3(xs[i], row), Quaternion.identity);
                i++;
            }
        }

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

        private void OnEnable()
        {
            this.spawnPoints.Clear();
            this.spawnPoints.Clear();

            this.InitializeIndicators();
            RearrangeObjects(this.spawnPoints, TileData.Dimension - 1);
            RearrangeObjects(this.defendPoints, 0);
        }
    }
}