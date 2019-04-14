namespace Characters
{
    using System.Collections.Generic;
    using System.Linq;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    ///     Should attached to "Player Party" game object.
    /// </summary>
    public class SpawnCharacters : MonoBehaviour
    {
        [ShowInInspector]
        private List<Transform> availableBlocks = new List<Transform>();

        public void Initialize()
        {
            var tilemap = GameObject.Find("Tilemap");
            var map = tilemap.GetComponent<CustomTerrain>();

            var slots = map.GetSpawnLocations();

            var tm = tilemap.GetComponent<Tilemap>();

            this.availableBlocks = slots.Select(x => tm.GetTileBlockAt(x).transform).ToList();

            this.StartDeployCharacters();
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
            // var sequence = DOTween.Sequence();
            foreach (Transform child in this.transform)
            {
                var tile = this.GetNextTransform();
                var target = tile.position;
                var offset = new Vector3(0.0f, 10f);

                child.SetPositionAndRotation(target + offset, Quaternion.identity);

                child.DOMove(target, 4.0f).SetEase(Ease.OutCubic).OnComplete(
                    () => { child.GetComponent<FollowTile>().AffiliateTile(tile); });
            }

            // sequence.OnComplete(() => Debug.Log("Done Deploying"));
        }
    }
}