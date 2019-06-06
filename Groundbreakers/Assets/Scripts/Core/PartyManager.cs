namespace Core
{
    using System.Collections.Generic;

    using Characters;

    using DG.Tweening;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;

    /// <summary>
    ///     Provide API to manipulate Characters on the battle field. 
    /// </summary>
    public class PartyManager : MonoBehaviour
    {
        private const int Unselected = -1;

        [ShowInInspector]
        private List<GameObject> characters = new List<GameObject>();

        private int currentSelectedIndex = Unselected;

        public void SelectCharacter(int index)
        {
            this.currentSelectedIndex = index;

            Debug.Log($"Selected = {this.currentSelectedIndex}");
        }

        public void DeselectCharacter()
        {
            this.currentSelectedIndex = Unselected;

            Debug.Log($"Selected = {this.currentSelectedIndex}");
        }

        /// <summary>
        ///     Should only be called by InputSystem (well, ideally command module)
        /// </summary>
        /// <param name="tile">
        ///     The tile to deploy on.
        /// </param>
        public void DeployCurrentCharacterAt(Transform tile)
        {
            if (this.currentSelectedIndex == Unselected)
            {
                Debug.Log("Did not select a character");

                return;
            }

            var character = this.characters[this.currentSelectedIndex];

            // Enable it first
            if (!this.IsAvailable(this.currentSelectedIndex))
            {
                Debug.LogError("Already deployed. Should not happen.");
                return;
            }

            character.SetActive(true);

            // Now create tween animation
            var target = tile.position;
            var offset = new Vector3(0.0f, 10f);

            character.transform.position = target + offset;

            character.transform.DOMove(target, 1.0f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => this.OnDeployComplete(character, tile))
                .SetUpdate(true);

            // Lastly, reset
            this.currentSelectedIndex = Unselected;
        }

        /// <summary>
        ///     Should be called when the battle has terminated. Retrieve the characters away from
        ///     The battle fields.
        /// </summary>
        [Button]
        public void RetrieveAllCharacters()
        {
            foreach (var character in this.characters)
            {
                character.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            this.characters.Clear();

            foreach (Transform child in this.transform)
            {
                // Cache the character
                this.characters.Add(child.gameObject);
            }

            this.RetrieveAllCharacters();
        }

        private bool IsAvailable(int index)
        {
            return !this.characters[index].activeSelf;
        }

        private void OnDeployComplete(GameObject character, Transform tile)
        {
            character.GetComponent<FollowTile>().AffiliateTile(tile);

            this.DeselectCharacter();

            var tc = FindObjectOfType<TileController>();
            tc.BeginInactive();
        }
    }
}