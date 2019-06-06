namespace Core
{
    using System.Collections.Generic;
    using System.Linq;

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

        /// <summary>
        ///     Select Which tile type can you deploy the characters on.
        /// </summary>
        [Required]
        [SerializeField]
        [EnumToggleButtons]
        private Tiles canDeployType = Tiles.HighGround;

        [ShowInInspector]
        [ReadOnly]
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
        ///     Use this to check if you can deploy the character on this tile. Particularly useful
        ///     to render option area.
        /// </summary>
        /// <param name="tile">
        ///     The tile.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool CanDeployAt(Transform tile)
        {
            // First, check tile type
            var type = tile.GetComponent<TileStatus>().GetTileType();

            if (type != this.canDeployType)
            {
                return false;
            }

            var pos = tile.position;

            return this.characters.All(
                x => Vector3.Distance(
                         x.transform.position, pos) > Mathf.Epsilon);
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
                Debug.LogWarning("Already deployed. Should not happen.");
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

            // Trigger the UI button
            var i = character.transform.GetSiblingIndex();
            this.TurnOnOff(i, false);
        }

        public void RetrieveCharacter(GameObject character)
        {
            character.SetActive(false);

            var i = character.transform.GetSiblingIndex();
            this.TurnOnOff(i, true);
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
                this.RetrieveCharacter(character);
            }
        }

        /// <summary>
        ///     Use this to help determine which character is on specific tile.
        /// </summary>
        /// <returns>
        ///     The <see cref="GameObject" />.
        /// </returns>
        public IEnumerable<GameObject> GetDeployedCharacters()
        {
            return this.characters.Where(x => x.activeSelf);
        }

        private void OnEnable()
        {
            this.characters.Clear();

            foreach (Transform child in this.transform)
            {
                // Cache the character
                this.characters.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }

            // this.RetrieveAllCharacters();
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

        private void TurnOnOff(int index, bool on)
        {
            // Un-Trigger the UI button
            var bp = GameObject.Find("CommandPanel")
                .transform
                .GetChild(index)
                .gameObject
                .GetComponent<ButtonPressed>();

            Debug.Log(index);

            if (on)
            {
                bp.Press();
            }
            else
            {
                bp.Unpress();
            }
        }
    }
}