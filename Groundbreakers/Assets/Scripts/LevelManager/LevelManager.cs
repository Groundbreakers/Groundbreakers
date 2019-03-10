namespace Assets.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    using static Asset.Script.EnemyGroups;

    public class LevelManager : MonoBehaviour
    {
        #region Internal References

        /// <summary>
        /// This stores the HUD GameObject (bad solution)
        /// </summary>
        private GameObject uiCanvas;

        /// <summary>
        /// The reference to the CurrentLevel script attached to the UI.
        /// </summary>
        private CurrentLevel currentLevelUi;

        /// <summary>
        /// The player party, currently we use the GameObject predefined in the scene to store
        /// party information (Subject to change).
        /// </summary>
        private GameObject playerParty;

        /// <summary>
        /// The audio manager (BGM Manager).
        /// </summary>
        private Manager audioManager;

        private RoutesGenerator routesGenerator;

        #endregion

        #region Internal Fields

        private Dictionary<int, Difficulty> levelDifficultyMap = new Dictionary<int, Difficulty>
                                                                     {
                                                                         { 1, Difficulty.Easy },
                                                                         { 2, Difficulty.Easy },
                                                                         { 3, Difficulty.Easy },
                                                                         { 4, Difficulty.Medium },
                                                                         { 5, Difficulty.Medium },
                                                                         { 6, Difficulty.Medium },
                                                                         { 7, Difficulty.Hard },
                                                                         { 8, Difficulty.Boss }
                                                                     };

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current game region of the progress.
        /// </summary>
        public int Region { get; private set; } = 1;

        /// <summary>
        /// Gets the current game level of the progress.
        /// </summary>
        public int Level { get; private set; } = 0;

        #endregion

        #region Public Functions

        public void StartLevel()
        {
            this.GotoNextLevel();

            this.routesGenerator.Toggle();
        }

        /// <summary>
        /// I assume this is called by Battle manager when battle ends
        /// </summary>
        public void GotoNextLevel()
        {
            if (this.Level == 8)
            {
                this.Region += 1;
                this.Level = 1;
                this.currentLevelUi.OnRegionChanged();
            }
            else
            {
                this.Level++;
            }

            // Update UI Text
            this.currentLevelUi.UpdateLevelInfo();
        }

        /// <summary>
        ///     Called by BattleManager (or directly called by spawn) I think
        /// </summary>
        /// <returns>
        ///     The current <see cref="Difficulty" /> of the level.
        /// </returns>
        public Difficulty GetDifficulty()
        {
            return this.Level == 0 ? Difficulty.Easy : this.levelDifficultyMap[this.Level];
        }

        #endregion

        #region Unity Callbacks

        public void OnEnable()
        {
            // Finding all the references. 
            this.uiCanvas = GameObject.Find("Canvas");

            if (!this.uiCanvas)
            {
                Debug.LogError("There needs to be one active Canvas GameObject in your scene.");
            }

            this.playerParty = GameObject.Find("CharacterList");

            if (!this.uiCanvas)
            {
                Debug.LogError("There needs to be one active CharacterList GameObject in your scene.");
            }

            this.audioManager = FindObjectOfType(typeof(Manager)) as Manager;

            if (!this.uiCanvas)
            {
                Debug.LogError("There needs to be one active Manager script on a GameObject in your scene.");
            }

            // Other references
            this.currentLevelUi = this.uiCanvas.GetComponent<CurrentLevel>();
            this.routesGenerator = this.uiCanvas.GetComponent<RoutesGenerator>();
        }

        public void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                this.StartLevel();
            }
        }

        #endregion

        #region Internal Functions



        #endregion
    }
}