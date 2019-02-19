﻿namespace Assets.Scripts
{
    using System.Collections.Generic;

    using DG.Tweening;

    using UnityEngine;
    using UnityEngine.Events;

    public class BattleManager : MonoBehaviour, IBattlePhaseHandler
    {
        #region Singleton

        private static BattleManager battleManager;

        #endregion

        #region Private Fields

        private Dictionary<string, UnityEvent> eventDictionary;

        private GameTimer timer;

        #endregion

        #region Public Properties

        /// <summary>
        /// The stages of the game phases. 
        /// </summary>
        public enum Stages
        {
            /// <summary>
            /// The Initial State
            /// </summary>
            Null,

            /// <summary>
            /// The state when all tiles should enter.
            /// </summary>
            Entering,

            /// <summary>
            /// The state when combat should be resolved.
            /// </summary>
            Combating,

            /// <summary>
            /// The state when all tiles should leave the map.
            /// </summary>
            Exiting,
        }

        /// <summary>
        /// Gets the only battle manager in the scene. Whenever referring the battle manager, please call 'BattleManager.Instance'. 
        /// </summary>
        public static BattleManager Instance
        {
            get
            {
                if (!battleManager)
                {
                    battleManager = FindObjectOfType(typeof(BattleManager)) as BattleManager;

                    if (battleManager != null)
                    {
                        battleManager.Initialize();
                    }
                    else
                    {
                        Debug.LogError("There needs to be one active BattleManager script on a GameObject in your scene.");
                    }
                }

                return battleManager;
            }
        }

        public static Stages GameState { get; private set; } = Stages.Null;

        #endregion

        #region Event System Functions

        /// <summary>
        /// The start listening.
        /// </summary>
        /// <example>
        /// public void OnEnable ()
        /// {
        ///    Battle.StartListening ("test", SomeFunction);
        /// }
        /// </example>
        /// <param name="eventName">
        /// The event name.
        /// </param>
        /// <param name="listener">
        /// The listener.
        /// </param>
        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if (battleManager == null)
            {
                return;
            }

            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Trigger an Event, all the functions registered under the eventName will be called.
        /// </summary>
        /// <example>
        /// BattleManager.TriggerEvent ("test");
        /// </example>
        /// <param name="eventName">
        /// The Name of an event that you wish to trigger/emit in string. 
        /// </param>
        public static void TriggerEvent(string eventName)
        {
            if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        #endregion

        #region Public Utility Functions

        /// <summary>
        /// The main entry function call to start the battle.
        /// </summary>
        public void ShouldStartBattle()
        {
            if (GameState != Stages.Null)
            {
                Debug.LogWarning("Attemp To Start a battle when battle is already started.");

                return;
            }

            // Changing state
            GameState = Stages.Entering;

            this.timer.StartLevel();
        }

        /// <summary>
        /// Instantly destroy all existing enemies on the scene. You might use this for some other
        /// interesting purposes. I am using during resetting the game map.
        /// </summary>
        public void KillAllEnemies()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enemy in enemies)
            {
                GameObject.Destroy(enemy);
            }
        }

        /// <summary>
        /// Instantly destroy all existing enemies on the scene. You might use this for some other
        /// interesting purposes. I am using during resetting the game map.
        /// </summary>
        public void RetreatAllCharacters()
        {
            // TODO: Please nicely Refactor this :)
            var characterList = GameObject.Find("CharacterList").transform;

            Debug.Log("Ding");

            foreach (Transform character in characterList)
            {
                Debug.Log("Bing");
                character.gameObject.SetActive(false);
            }
        }

        #endregion

        #region IBattlePhaseHandler

        public void OnBattleBegin()
        {
            GameState = Stages.Combating;

            // Toggle UI
            Resources.FindObjectsOfTypeAll<Deploy>()[0].ui.SetActive(true);
        }

        public void OnBattleEnd()
        {
            this.RetreatAllCharacters();

            GameState = Stages.Exiting;

            // Toggle UI
            GameObject.Find("DeployPanel").GetComponent<Deploy>().Toggle();
        }

        public void OnBattleVictory()
        {
            // Clear existing mobs
            this.KillAllEnemies();

            // Temp, call the loot 
            var lootUI = FindObjectOfType<LootGenerator>();
            lootUI.Toggle();

            this.timer.ResetTimer();
            GameState = Stages.Null;
        }

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            GameState = Stages.Null;

            Debug.Log("BattleManager.Reset() is called");
        }

        private void OnEnable()
        {
            this.timer = this.GetComponent<GameTimer>();

            StartListening("block ready", this.OnBattleBegin);

            StartListening("end", this.OnBattleEnd);

            StartListening("victory", this.OnBattleVictory);

            StartListening("reset", this.Reset);
        }
        
        #endregion

        #region Internal functions  

        private void Initialize()
        {
            if (this.eventDictionary == null)
            {
                this.eventDictionary = new Dictionary<string, UnityEvent>();
            }

            // Some setting
            DOTween.Init(true, true);
        }

        #endregion
    }
}