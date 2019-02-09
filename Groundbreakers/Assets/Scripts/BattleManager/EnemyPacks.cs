namespace Asset.Script
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class EnemyPacks : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        private Transform enemyPrefab;

        #endregion

        #region Internal Fields

        /// <summary>
        /// The packs, SUBJECT TO CHANGE
        /// </summary>
        [Obsolete]
        private List<Transform>[] packs = new List<Transform>[2];

        #endregion 

        private void Awake()
        {
            // Well, eventually we ready from an XML/JSON File
            this.InitializePacks();
        }

        private void InitializePacks()
        {
            this.packs[0] = new List<Transform>(new Transform[] { this.enemyPrefab, this.enemyPrefab, this.enemyPrefab, this.enemyPrefab, this.enemyPrefab });
            this.packs[1] = new List<Transform>(new Transform[] { this.enemyPrefab, this.enemyPrefab, this.enemyPrefab });
        }
    }

}