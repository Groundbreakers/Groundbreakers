namespace Core
{
    using System.Collections.Generic;
    using System.Linq;

    using Characters;

    using Sirenix.OdinInspector;

    using TileMaps;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Provide API to manipulate Characters on the battle field. 
    /// </summary>
    public class PartyManager : MonoBehaviour
    {
        private const int Unselected = -1;

        /// <summary>
        ///     Select Which tile type can you deploy the characters on.
        /// </summary>
        [SerializeField]
        [EnumToggleButtons]
        private Tiles canDeployType = Tiles.HighGround;

        [ShowInInspector]
        [ReadOnly]
        private List<GameObject> characters = new List<GameObject>();

        private Dictionary<GameObject, int> greyedout = new Dictionary<GameObject, int>();

        private int currentSelectedIndex = Unselected;

        /// <summary>
        ///     This Must be called when a battle starts.
        ///     And you want to have the character game objects already instantiated in under the
        ///     Player Party' game object.
        /// </summary>
        public void LoadCharacters()
        {
            this.characters.Clear();
            this.greyedout.Clear();

            foreach (Transform child in this.transform)
            {
                // Cache the character
                this.characters.Add(child.gameObject);
            }

            this.RetrieveAllCharacters();
        }

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

        public int GetCurrentlySelectedIndex()
        {
            return this.currentSelectedIndex;
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
                x => Vector3.Distance(x.transform.position, pos) > Mathf.Epsilon);
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
            this.OnDeployComplete(character, tile);

            // Lastly, reset
            this.currentSelectedIndex = Unselected;
        }

        public void RetrieveCharacter(GameObject character)
        {
            character.transform.position = Vector3.zero;
            character.SetActive(false);

            if (this.greyedout.ContainsKey(character))
            {
                var index = this.greyedout[character];
                this.greyedout.Remove(character);
                this.GreyOutUiButton(index, false);
            }
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
                // character.gameObject.SetActive(false);
                this.RetrieveCharacter(character.gameObject);
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

        private bool IsAvailable(int index)
        {
            return !this.characters[index].activeSelf;
        }

        private void OnDeployComplete(GameObject character, Transform tile)
        {
            // Grey out the UI
            this.greyedout.Add(character, this.currentSelectedIndex);
            this.GreyOutUiButton(this.currentSelectedIndex, true);

            // Others
            character.GetComponent<FollowTile>().AffiliateTile(tile);

            this.DeselectCharacter();

            var tc = FindObjectOfType<TileController>();
            tc.BeginInactive();

            character.transform.position = tile.position;
        }

        private void GreyOutUiButton(int index, bool grey)
        {
            var obj = GameObject.Find("CommandPanel");
            var portrait = obj.transform.GetChild(index);

            portrait.GetComponent<Button>().interactable = !grey;
        }
    }
}