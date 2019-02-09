namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelManager : MonoBehaviour
    {
        #region Internal References

        /// <summary>
        /// This stores the HUD GameObject (bad solution)
        /// </summary>
        private GameObject uiCanvas;

        /// <summary>
        /// The player party, currently we use the GameObject predefined in the scene to store
        /// party information (Subject to change).
        /// </summary>
        private GameObject playerParty;

        /// <summary>
        /// The audio manager (BGM Manager).
        /// </summary>
        private Manager audioManager;

        #endregion 

        #region Internal Fields

        /// <summary>
        /// The current area. 
        /// </summary>
        [Obsolete]
        private int currentRegion = 1;

        /// <summary>
        /// The current level in the game;
        /// </summary>
        private int currentLevel = 1;

        #endregion

        #region Public Properties



        #endregion

        #region Public Functions



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
        }

        public void Update()
        {

        }

        #endregion

        #region Internal Functions



        #endregion
    }
}