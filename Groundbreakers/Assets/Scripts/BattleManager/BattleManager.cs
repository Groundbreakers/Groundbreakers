namespace Assets.Scripts
{
    using System.Collections.Generic;

    using DG.Tweening;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

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

        public static GameStates GameState { get; private set; } = GameStates.Null;

        public static LevelManager GameLevel { get; private set; }

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
            if (GameState != GameStates.Null)
            {
                Debug.LogWarning("Attemp To Start a battle when battle is already started.");

                return;
            }

            // Changing state
            GameState = GameStates.Entering;

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
            Debug.Log("Ding");

            GameObject.Find("DeployPanel").GetComponent<Deploy>().InstantRetreatAllCharacter();
        }

        #endregion

        #region IBattlePhaseHandler

        public void OnBattleBegin()
        {
            GameState = GameStates.Combating;

            // Toggle UI
            Resources.FindObjectsOfTypeAll<GameSpeed>()[0].Toggle();
            GameObject.Find("DeployPanel").GetComponent<Animator>().SetBool("Open", true);
        }

        public void OnBattleEnd()
        {
            this.RetreatAllCharacters();

            GameState = GameStates.Exiting;

            // Toggle UI (dirty way)
            GameObject.Find("DeployPanel").GetComponent<Deploy>().Clear();
            GameObject.Find("DeployPanel").GetComponent<Animator>().SetBool("Open", false);

            // Reset Timer (dirty way)
            GameObject.Find("1xButton").GetComponent<Button>().onClick.Invoke();
        }

        public void OnBattleVictory()
        {
            this.KillAllEnemies();

            GameLevel.StartLevel();

            this.timer.ResetTimer();

            // Dirty way to not show GameSpeed
            Resources.FindObjectsOfTypeAll<GameSpeed>()[0].Toggle();

            GameState = GameStates.Null;
        }

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            GameState = GameStates.Null;

            Debug.Log("BattleManager.Reset() is called");
        }

        private void OnEnable()
        {
            GameLevel = FindObjectOfType<LevelManager>();
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
            BattleUtility.Initialize();
        }

        #endregion
    }
}