namespace Asset.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

#if UNITY_EDITOR
    using UnityEditor;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.OdinInspector;
#endif

    using UnityEngine;

    using Random = System.Random;

    /// <summary>
    /// The Generic EnemyGroup Component. Using OdinInspector (or without) to visualize can help
    /// the designer to create enemy packs.
    /// </summary>
    public class EnemyGroups : MonoBehaviour
    {
        private static Random rng = new Random();

        #region Inspector Fields

        [SerializeField]
        [Title("Region A Groups")]
        private Group[] regionAGroup;

        #endregion

        #region Internal Fields

        /// <summary>
        /// Internal buffers that store the pending enemy mobs.
        /// </summary>
        private List<GameObject> bufferA = new List<GameObject>();
        private List<GameObject> bufferB = new List<GameObject>();

        private Difficulty currentDifficulty = Difficulty.Easy;

        #endregion

        #region Properties

        /// <summary>
        /// The difficulty represent which packs to pick.
        /// </summary>
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Elite,
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Provide an API for outside caller to set the difficulty levels. 
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        public void SetDifficulty(Difficulty level)
        {
            this.currentDifficulty = level;
        }

        /// <summary>
        /// Check if the current wave queue has finished.
        /// </summary>
        /// <param name="pathId">
        /// The path Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> True if the current wave queue is empty.
        /// </returns>
        public bool Done(int pathId)
        {
            return this.GetCount(pathId) == 0;
        }

        public int GetCount(int pathId)
        {
            var buffer = pathId == 1 ? this.bufferA : this.bufferB;

            return buffer.Count;
        }

        /// <summary>
        /// Get the next enemy from this current group.
        /// </summary>
        /// <param name="pathId">
        /// The path Id.
        /// </param>
        /// <returns>
        /// The <see cref="GameObject"/> a random enemy from this group.
        /// </returns>
        public GameObject GetNextMob(int pathId)
        {
            var buffer = pathId == 1 ? this.bufferA : this.bufferB;
            const int Head = 0;

            var go = buffer[Head];
            buffer.RemoveAt(Head);
            return go;
        }

        /// <summary>
        /// Should be called after every battle.
        /// </summary>
        public void ResetPack()
        {
            // Fast and dirty way to do
            var level = GameObject.Find("Canvas").GetComponent<CurrentLevel>().GetDifficulty();

            this.bufferA.Clear();
            this.bufferB.Clear();

            this.bufferA = PickRandomPack(this.regionAGroup, level);
            this.bufferB = PickRandomPack(this.regionAGroup, level);

            Shuffle(this.bufferA);
            Shuffle(this.bufferB);
        }

        #endregion

        #region Static helper functions

        private static void Shuffle(IList<GameObject> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Internal helper function that picks a random pack first, then construct an array of
        /// GameObject based on the described pack.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private static List<GameObject> PickRandomPack(Group[] region, Difficulty level)
        {
            // Pick a random pack, but has to be in the same level
            var bundle = region.Where(group => group.level == level).ToArray();

            if (!bundle.Any())
            {
                Debug.LogError("This should not happen: Empty enemy group");
            }

            var packs = bundle[rng.Next(bundle.Length)].packs;

            // Construct an buffer based on the selected buffer
            List<GameObject> result = new List<GameObject>();
            foreach (var pack in packs)
            {
                for (var i = 0; i < pack.amount; i++)
                {
                    result.Add(pack.prefab);
                }
            }

            return result;
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.ResetPack();
        }

        #endregion

        #region Internal 

        [Serializable]
        public struct Group
        {
            public string title;

            [EnumToggleButtons]
            public Difficulty level;

            [ListDrawerSettings(Expanded = true)]
            public Pack[] packs;
        }

        [Serializable]
        public struct Pack
        {
            public int amount;

            public GameObject prefab;
        }

        #endregion
    }

#if UNITY_EDITOR

    /// <summary>
    /// This is an OdinInspector Specific CustomDrawer implementation. Although it shows that is
    /// never being used. Do not Remove it anyhow.
    /// </summary>
    public class PackDrawer : OdinValueDrawer<EnemyGroups.Pack>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var value = this.ValueEntry.SmartValue;

            var rect = EditorGUILayout.GetControlRect();

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            var prev = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 20;

            value.amount = EditorGUI.IntField(rect.AlignLeft(rect.width * 0.3f), value.amount);
            value.prefab = (GameObject)EditorGUI.ObjectField(
                rect.AlignRight(rect.width * 0.7f), 
                value.prefab, 
                typeof(GameObject), 
                false);

            EditorGUIUtility.labelWidth = prev;

            this.ValueEntry.SmartValue = value;
        }
    }

#endif
}