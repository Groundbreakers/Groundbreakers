namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Ideally, equip this launcher to character objects when in Ranged attack mode.
    /// Disable this component when switched to Melee Mode.
    /// </summary>
    [RequireComponent(typeof(BulletMovement))]
    public class BulletLauncher : MonoBehaviour
    {
        #region Inspector
        
        [SerializeField]
        private GameObject bulletPrefab;

        #endregion

        #region Internal Fields

        private List<BulletMovement> buffer = new List<BulletMovement>();

        private characterAttributes attributes;

        #endregion

        #region Public Functions    

        [Button("Test Launch All")]
        public void LaunchAll()
        {
            this.InstantiateBullets();

            // Subject to change
            var direction = this.transform.forward;

            this.buffer.ForEach(bullet => bullet.Launch(direction));
        }

        public void AimAtTarget(Transform target)
        {
            this.transform.LookAt(target);
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.attributes = this.transform.parent.GetComponent<characterAttributes>();
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
            var pos = this.transform.position;
            var go = GameObject.Instantiate(this.bulletPrefab, pos, Quaternion.identity);

            var bullet = go.GetComponent<BulletMovement>();
            bullet.SetCharacterAttribute(this.attributes);

            this.buffer.Add(bullet);
        }

        #endregion
    }
}