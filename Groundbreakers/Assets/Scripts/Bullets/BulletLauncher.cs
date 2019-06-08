namespace Assets.Scripts
{
    using System;

    using Sirenix.OdinInspector;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Ideally, equip this launcher to character objects when in Ranged attack mode.
    /// Disable this component when switched to Melee Mode.
    /// </summary>
    [RequireComponent(typeof(DamageHandler))]
    public class BulletLauncher : MonoBehaviour
    {
        #region Inspector

        //public LineRenderer lineRenderer;

        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        public Type type;
        
        [SerializeField]
        private Vector3 offSet;
        
        #endregion

        #region Internal Fields

        private DamageHandler damageHandler;

        private Transform potentialTarget;

        // tmp
        private int laserDelay = 10;
        private int counter = 0;

        #endregion

        #region Public Properties
        
        public enum Type
        {
            SingleShot,

            MultiShot,

            Laser,

            Penetrate,

            Explosive,
        }

        #endregion
        
        #region Public Functions    

        [Button("Test Launch All")]
        private void FireAt(Transform target)
        {
            this.potentialTarget = target;

            switch (this.type)
            {
                case Type.SingleShot:
                    this.gameObject.GetComponentInParent<SingleShot>().singleShot(target);
                    break;
                case Type.MultiShot:
                    this.gameObject.GetComponentInParent<MultiShot>().multiShot(target);
                    break;
                case Type.Laser:
                    break;
                case Type.Penetrate:
                    this.PenetrateShot(target);
                    break;
                case Type.Explosive:
                    this.ExplosiveShot(target);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Warning, this is temporary solution, should use a proper external damage handler
        public void Melee(Transform target)
        {
            
            this.damageHandler.DeliverDamageTo(target.gameObject, true);
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            this.damageHandler = this.GetComponent<DamageHandler>();
            //this.laserBeam = this.GetComponent<LineRenderer>();

            //var points = new Vector3[2];
            //var t = Time.time;
            //for (int i = 0; i < 2; i++)
            //{
            //    points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f);
            //}

            //this.laserBeam.SetPositions(points);
        }

        private void Update() {

            MonoBehaviour ability = this.gameObject.GetComponentInParent<characterAttack>().ability1;

            if (ability != null)
            {
                string ability_name = ability.GetType().ToString();
                // DEBUG
                if (ability_name == "SingleShot")
                {
                    this.type = Type.SingleShot;
                   
                }

                if (ability_name == "MultiShot")
                {
                    this.type = Type.MultiShot;
                }

                if (Input.GetKeyDown("3"))
                {
                    this.type = Type.Laser;
                }

                if (Input.GetKeyDown("4"))
                {
                    this.type = Type.Penetrate;
                }

                if (Input.GetKeyDown("5"))
                {
                    this.type = Type.Explosive;
                    Debug.Log(this.type);
                }
            }
            //// For laser effect only
            //if (this.type == Type.Laser)
            //{
            //    if (!this.potentialTarget)
            //    {
            //        return;
            //    }

            //    this.LockOnTarget();

            //    this.counter++;
            //    if (this.counter == this.laserDelay)
            //    {
            //        this.counter = 0;
            //        this.damageHandler.DeliverDamageTo(this.potentialTarget.gameObject, false);
            //    }
            //}
        }

        #endregion

        #region Internal Functions

        // temp solution
        private void LockOnTarget()
        {
            //const float MaxRange = 5;

            //var from = this.transform.position;
            //var to = this.potentialTarget.position;
            //var direction = to - from;

            //this.laserBeam.SetPosition(0, from);
            //this.laserBeam.SetPosition(1, to);
        }

        /// <summary>
        /// Create Instance from bullet prefab.
        /// </summary>
        /// <returns>
        /// The <see cref="IBullet"/>.
        /// </returns>
        public IBullet InstantiateBullet()
        {
            var offset = new Vector3(0.0f, 0.5f, 0.0f);

            // Currently using native Instantiation method. Will switch to Object pool.
            // Should also trigger event
            var pos = this.transform.position;
            var go = Instantiate(this.bulletPrefab, pos + offset, Quaternion.identity);

            var bullet = go.GetComponent<IBullet>();

            // this.buffer.Add(bullet);
            return bullet;
        }

        // Subject to change
    
        private void ExplosiveShot(Transform target)
        {
            
            var bullet = this.InstantiateBullet();

            bullet.Launch(target, this.damageHandler);
        }

        private void PenetrateShot(Transform target)
        {
            
            var bullet = this.InstantiateBullet();

            bullet.Launch(target, this.damageHandler);
        }
        
        #endregion
    }
}