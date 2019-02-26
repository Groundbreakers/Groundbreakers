namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <summary>
    /// Ideally, equip this launcher to character objects when in Ranged attack mode.
    /// Disable this component when switched to Melee Mode.
    /// </summary>
    public class BulletLauncher : MonoBehaviour
    {
        #region Inspector
        
        [SerializeField]
        private GameObject bulletPrefab;

        #endregion

        #region Internal Fields

        private List<BulletMovement> buffer = new List<BulletMovement>();

        #endregion

        #region Public Functions    

        [Button("Test Launch All")]
        public void LaunchAll()
        {
            this.InstantiateBullets();

            this.buffer.ForEach(bullet => bullet.Launch(new Vector3(1, 1, 0)));
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            //this.InstantiateBullets();
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Create Instance from bullet prefab.
        /// </summary>
        private void InstantiateBullets()
        {
            // Currently using native Instantiation method. Will switch to Object pool.
            // Should also trigger event
            var go = GameObject.Instantiate(this.bulletPrefab, this.transform);

            this.buffer.Add(go.GetComponent<BulletMovement>());
        }

        #endregion
    }
}