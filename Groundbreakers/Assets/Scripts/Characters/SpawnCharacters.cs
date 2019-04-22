namespace Characters
{
    using System.Collections.Generic;
    using System.Linq;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    ///     Should attached to "Player Party" game object.
    ///     Call "Initialize" method to spawn all characters on to the map.
    /// </summary>
    public class SpawnCharacters : MonoBehaviour
    {
        [ShowInInspector]
        private List<Transform> availableBlocks = new List<Transform>();

        /// <summary>
        ///     Ask available character spawn slots, then deploy characters.
        /// </summary>
        public void Initialize()
        {
            var tilemap = GameObject.Find("Tilemap");

            Assert.IsNotNull(tilemap, "Need to have a Tilemap GameObject active in the scene.");

            var map = tilemap.GetComponent<CustomTerrain>();
            var tm = tilemap.GetComponent<Tilemap>();

            var slots = map.GetSpawnLocations();
            this.availableBlocks = slots.Select(x => tm.GetTileBlockAt(x).transform).ToList();

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