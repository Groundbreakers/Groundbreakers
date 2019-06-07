namespace Assets.Scripts
{
    using Assets.Enemies.Scripts;

    using DG.Tweening;

    using UnityEngine;

    /// <inheritdoc cref="IBullet" />
    /// <summary>
    ///     The most basic bullet movement pattern: move toward one direction in Linear Motion.
    /// </summary>
    [RequireComponent(typeof(OffScreenHandler))]
    public class BulletLinearMovement : MonoBehaviour, IBullet
    {
        private DamageHandler damageHandler;

        private float explosionRadius = 1.5f;

        /// <summary>
        ///     The linear direction of the movement of the bullet.
        /// </summary>
        private Vector3 linearDirection;

        private bool _break;

        private bool purge;

        private bool mark;

        private bool burn;

        private bool blight;

        private bool slow;

        private bool stun;

        #region IBullet

        public void Launch(Transform target, DamageHandler handler)
        {
            var direction = target.position - this.transform.position;
            this.Launch(direction, handler);
        }

        public void Launch(Vector3 direction, DamageHandler handler)
        {
            this.linearDirection = direction.normalized;
            this.damageHandler = handler;
        }

        #endregion

        private void FixedUpdate()
        {
            const float Speed = 0.5f;

            this.transform.Translate(this.linearDirection * Speed);
        }

        /// <summary>
        ///     This method is triggered only if the collider is checked with "isTriggered"
        /// </summary>
        /// <param name="other">
        ///     The other.
        /// </param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            var go = other.gameObject;

            if (go.CompareTag("Enemy"))
            {
                if (GameObject.Find("RangedWeapon").GetComponent<BulletLauncher>().type
                    == BulletLauncher.Type.Explosive)
                {
                    if (explosionRadius >= 0f)
                    {
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);

                        foreach (Collider2D collider in colliders)
                        {
                            if (collider.tag == "Enemy")
                            {
                                this.damageHandler.DeliverDamageTo(collider.gameObject);
                            }
                        }
                    }
                    else
                    {
                        this.damageHandler.DeliverDamageTo(go);
                    }
                }
                else
                {
                    this.damageHandler.DeliverDamageTo(go);
                }

                if (GameObject.Find("RangedWeapon").GetComponent<BulletLauncher>().type != BulletLauncher.Type.Penetrate)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }

            return;

            //if (go.CompareTag("Player"))
            //{
            //    var character = this.damageHandler.Source;

            //    if (ReferenceEquals(go, character))
            //    {
            //        return;
            //    }

            //    // Need to further check the distance (sorry run out of time to refactor this)
            //    var delta = Vector2.Distance(go.transform.position, character.transform.position);
            //    Debug.Log(delta);
            //    if (delta > 1.0f)
            //    {
            //        return;
            //    }

            //    // Stun the target character
            //    go.GetComponent<characterAttack>().stun(3);
            //    go.transform.DOShakePosition(2.0f, 0.1f);
            //    GameObject.Find("Canvas").GetComponent<DamagePopup>().ProduceText(-1, go.transform);
            // }
        }

        //public void statusEffectHandler(GameObject go)
        //{
        //    // prioritize armor breaking and purge 
        //    if (this._break)
        //    {
        //        go.GetComponent<Enemy_Generic>().breakEnemyArmor();
        //    }

        //    if (this.purge && go.GetComponent<Enemy_Generic>().getIsPurged() == false)
        //    {
        //        go.GetComponent<Enemy_Generic>().purgeEnemy();
        //    }

        //    if (this.mark != true)
        //    {
        //        // go.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false, false);
        //        this.damageHandler.DeliverDamageTo(go);
        //    }
        //    else
        //    {
        //        // go.GetComponent<Enemy_Generic>().DamageEnemy(this.damage, this.armorpen, 1, false, true);
        //        this.damageHandler.DeliverDamageTo(go);
        //    }

        //    if (this.burn && go.GetComponent<Enemy_Generic>().getIsBurned() == false)
        //    {
        //        go.GetComponent<Enemy_Generic>().BurnEnemy();
        //    }

        //    if (this.blight && go.GetComponent<Enemy_Generic>().getIsBlighted() == false)
        //    {
        //        go.GetComponent<Enemy_Generic>().BlightEnemy();
        //    }

        //    if (this.slow && go.GetComponent<Enemy_Generic>().getIsSlowed() == false)
        //    {
        //        go.GetComponent<Enemy_Generic>().SlowEnemy(0.5f);
        //    }

        //    if (this.stun && go.GetComponent<Enemy_Generic>().getIsStunned() == false)
        //    {
        //        go.GetComponent<Enemy_Generic>().StunEnemy(0.5f);
        //    }
        //}
    }
}