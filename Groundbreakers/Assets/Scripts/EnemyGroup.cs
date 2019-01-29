namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEditor;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public class EnemyGroup : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private EnemyPrefab[] enemies;
#pragma warning restore 649

        private List<GameObject> enemyList = new List<GameObject>();

        /// <summary>
        ///     Check if this queue has finished.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> True if queue is empty.
        /// </returns>
        public bool Done()
        {
            return this.enemyList.Count == 0;
        }

        /// <summary>
        ///     Get the next enemy from this enemy group.
        /// </summary>
        /// <returns>
        ///     The <see cref="GameObject" /> a random enemy from this group.
        /// </returns>
        public GameObject GetNext()
        {
            const int Head = 0;
            var prefab = this.enemyList[Head];
            this.enemyList.RemoveAt(Head);
            return prefab;
        }

        /// <summary>
        ///     Resetting the queue, basically generate a new sequence of enemies.
        /// </summary>
        public void ReSet()
        {
            this.enemyList.Clear();

            foreach (var enemyPrefab in this.enemies)
            {
                var prefab = enemyPrefab.prefab;
                var amount = enemyPrefab.amount;

                for (var i = 0; i < amount; i++)
                {
                    this.enemyList.Add(prefab);
                }
            }

            Shuffle(this.enemyList);
        }

        public void Start()
        {
            this.ReSet();
        }

        private static void Shuffle<T>(IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }

    #region Inspector Editor

    /// <summary>
    ///     This Class basically serves as an interface for EnemyGroup's enemies field.
    /// </summary>
    [Serializable]
    public class EnemyPrefab
    {
        public int amount;

        public GameObject prefab;
    }

    /// <summary>
    ///     This Class must exist because Unity Editor will need to refer to this.
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