namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Events;

    using Random = UnityEngine.Random;

    [RequireComponent(typeof(GameMap))]
    public class BattleManager : MonoBehaviour
    {
        #region Singleton

        private static BattleManager battleManager;

        #endregion

        #region Inspector Variables

        [SerializeField]
        private GameObject character;

        #endregion

        #region Private Fields

        private Dictionary<string, UnityEvent> eventDictionary;

        private List<GameObject> characterPoll = new List<GameObject>();

        private Transform currentSelectedTile = null;

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
            UnityEvent thisEvent = null;
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

        #region Unity Callbacks

        public void OnEnable()
        {
            StartListening(
                "block ready",
                () =>
                    {
                        GameState = Stages.Combating;
                    });

            StartListening(
                "update wave",
                () =>
                    {
                        GameState = Stages.Exiting;
                    });
        }

        public void Start()
        {
            // Initialize Character poll
        }

        public void Update()
        {
            // This is for debugging purpose
            if (Input.GetKeyDown("q"))
            {
                TriggerEvent("test");
            }

            // These are actual part Input Update *NOT FINAL*
            if (this.currentSelectedTile && Input.GetMouseButtonDown(0))
            {
                var newPos = this.currentSelectedTile.position;

                Debug.Log(newPos);

                // Todo: encapsulate in methods
                if (this.characterPoll.Count < 5 && this.CanDeployAt(newPos)) 
                {
                    var instance = Instantiate(this.character, newPos, Quaternion.identity);
                    this.characterPoll.Add(instance);
                }
            }

            if (this.currentSelectedTile && Input.GetMouseButtonDown(1))
            {
                var pos = this.currentSelectedTile.position;

                var selected = this.characterPoll.Find(x => Math.Abs(Vector3.Distance(pos, x.transform.position)) < Mathf.Epsilon);

                if (selected)
                {
                    // *Not final*
                    this.characterPoll.Remove(selected);
                    Destroy(selected);
                }
                else
                {
                    // Play error sound?
                }
            }
        }

        #endregion

        #region Public Functions

        public void OnWaveUpdate(int currentWave)
        {
            Debug.Log("Current wave " + currentWave);

            // Todo: make it more generic
            if (currentWave == 5 - 1)
            {
                Debug.Log("Bing");
                GameState = Stages.Exiting;
                TriggerEvent("battle finished");
            }

            // Should update the UI.
        }

        public void SetCurrentSelectedTile(Transform currentGrid)
        {
            this.currentSelectedTile = currentGrid;
        }

        #endregion

        #region Internal functions  

        private void Initialize()
        {
            if (this.eventDictionary == null)
            {
                this.eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        private bool CanDeployAt(Vector3 pos)
        {
            if (!this.GetComponent<GameMap>().CanDeployAt(pos))
            {
                return false;
            }

            // Then check all player
            var selected = this.characterPoll.Find(chara => Math.Abs(Vector3.Distance(pos, chara.transform.position)) < Mathf.Epsilon);

            return !selected;
        }

        #endregion
    }
}