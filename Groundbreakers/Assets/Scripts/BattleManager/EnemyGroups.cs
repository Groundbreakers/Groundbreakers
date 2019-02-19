namespace Asset.Script
{
    using System;

#if UNITY_EDITOR

    using UnityEditor;

    using Sirenix.Utilities;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;

#endif

    using UnityEngine;

    public class EnemyGroups : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        [Title("Region A Groups")]
        private Group[] regionAGroup;

        #endregion

        #region Internal Fields


        #endregion

        #region Properties

        public enum Enemies
        {
            DireRat,
            FireBat,
            Manis,
        }

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Elite
        }

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

        #region Public Functions



        #endregion

        #region Unity Callbacks



        #endregion

        #region Internal Functions


        #endregion
    }

#if UNITY_EDITOR

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