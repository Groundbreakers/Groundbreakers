namespace Characters
{
    using System.Collections;
    using System.Collections.Generic;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc cref="MonoBehaviour" />
    /// <summary>
    ///     Should attached to "Player Party" game object.
    ///     Call "Initialize" method to spawn all characters on to the map.
    /// </summary>
    public class SpawnCharacters : MonoBehaviour, IEnumerable<GameObject>
    {
        /// <summary>
        ///     Store the TileBlock GameObjects that are candidates for spawn in to a list.
        /// </summary>
        [ShowInInspector]
        private List<Transform> availableBlocks = new List<Transform>();

        /// <summary>
        ///     Ask available character spawn slots, then deploy characters.
        /// </summary>
        public void Initialize()
        {
            var tilemap = GameObject.Find("Tilemap");

            Assert.IsNotNull(tilemap, "Need to have a Tilemap GameObject active in the scene.");

            var tm = tilemap.GetComponent<Tilemap>();

            this.availableBlocks = FindSpawnLocations(tm);

            this.StartDeployCharacters();
        }

        /// <summary>
        ///     Should be called when the battle has terminated. Retrieve the characters away from
        ///     The battle fields.
        /// </summary>
        public void RetrieveAllCharacters()
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            foreach (Transform child in this.transform)
            {
                yield return child.gameObject;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static List<Transform> FindSpawnLocations(Tilemap tilemap)
        {
            var list = new List<Transform>();

            for (var i = 0; i < Tilemap.Dimension; i++)
            {
                for (var j = 0; j < Tilemap.Dimension; j++)
                {
                    var block = tilemap.GetCachedTileStatusAt(i, j);

                    if (block && block.GetTileType() == Tiles.HighGround)
                    {
                        list.Add(block.transform);
                    }
                }
            }

            if (list.Count < 5)
            {
                Debug.LogError("Should have at least 5 HighGround block");
            }

            return list;
        }

        private Transform GetNextTransform()
        {
            if (this.availableBlocks.Count == 0)
            {
                Debug.LogError("Should not happen");
            }

            var pos = this.availableBlocks[0];
            this.availableBlocks.RemoveAt(0);
            return pos;
        }

        private void StartDeployCharacters()
        {
            var sequence = DOTween.Sequence();
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);

                var tile = this.GetNextTransform();
                var target = tile.position;
                var offset = new Vector3(0.0f, 10f);

                child.SetPositionAndRotation(target + offset, Quaternion.identity);

                var tween = child.DOMove(target, 4.0f).SetEase(Ease.OutCubic).OnComplete(
                    () => { child.GetComponent<FollowTile>().AffiliateTile(tile); });

                sequence.Join(tween);
            }

            sequence.OnComplete(() => Debug.Log("Done Deploying"));
        }
    }
}