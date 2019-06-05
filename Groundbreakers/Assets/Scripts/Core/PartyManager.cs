namespace Core
{
    using System.Collections.Generic;

    using Characters;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using UnityEngine;

    public class PartyManager : MonoBehaviour
    {
        private const int Unselected = -1;

        [ShowInInspector]
        private List<GameObject> characters = new List<GameObject>();

        private int currentSelectedIndex = Unselected;

        public void SelectCharacter(int index)
        {
            if (this.currentSelectedIndex == index)
            {
                // Already select, deselect
                this.currentSelectedIndex = Unselected;
            }

            this.currentSelectedIndex = index;

            Debug.Log($"Selected = {this.currentSelectedIndex}");
        }

        [Button]
        public void DeployCurrentCharacterAt(Transform tile)
        {
            if (this.currentSelectedIndex != Unselected)
            {
                var character = this.characters[this.currentSelectedIndex];

                var target = tile.position;
                var offset = new Vector3(0.0f, 10f);

                character.transform.SetPositionAndRotation(target + offset, Quaternion.identity);

                character.transform.DOMove(target, 1.0f).SetEase(Ease.OutCubic).OnComplete(
                    () => { character.GetComponent<FollowTile>().AffiliateTile(tile); });

                this.currentSelectedIndex = Unselected;
            }
        }

        private void OnEnable()
        {
            this.characters.Clear();

            foreach (Transform child in this.transform)
            {
                // Cache the character
                this.characters.Add(child.gameObject);


                // Setup 
            }
        }
        private void Update()
        {

        }
    }
}