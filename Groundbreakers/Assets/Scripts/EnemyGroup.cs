namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEditor;
    using UnityEngine;

    public class EnemyGroup : MonoBehaviour
    {
        [SerializeField]
        private EnemyPrefab[] enemies;

        #region Internal field

        private List<GameObject> enemyList = new List<GameObject>();

        #endregion

        #region Public Functions

        public GameObject GetNext()
        {
            const int Head = 0;
            var prefab = this.enemyList[Head];
            this.enemyList.RemoveAt(Head);
            return prefab;
        }

        public bool Done()
        {
            return this.enemyList.Count == 0;
        }

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            foreach (var enemyPrefab in this.enemies)
            {
                var prefab = enemyPrefab.prefab;
                var amount = enemyPrefab.amount;

                for (int i = 0; i < amount; i++)
                {
                    this.enemyList.Add(prefab);
                }
            }

            Shuffle(this.enemyList);
        }

        #endregion

        #region Internal Functions

        public static void Shuffle<T>(IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        #endregion
    }

    #region Inspector Editor

    /// <summary>
    /// This Class basically serves as an interface for EnemyGroup's enemies field. 
    /// </summary>
    [Serializable]
    public class EnemyPrefab
    {
        public GameObject prefab;

        public int amount;
    }

    /// <summary>
    /// This Class must exist because Unity Editor will need to refer to this.
    /// </summary>
    [CustomPropertyDrawer(typeof(EnemyPrefab))]
    public class EnemyGroupDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, 30, position.height);
            var unitRect = new Rect(position.x + 35, position.y, 150, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("prefab"), GUIContent.none);
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    #endregion
}