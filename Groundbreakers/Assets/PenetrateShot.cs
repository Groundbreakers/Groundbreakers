using Assets.Scripts;
using UnityEngine;

    public class PenetrateShot : MonoBehaviour
    {
        private BulletLauncher bulletLauncher;

        private DamageHandler damageHandler;

        public void penetrateShot(Transform target)
        {

            this.bulletLauncher = this.GetComponentInChildren<BulletLauncher>();

            this.damageHandler = GameObject.Find("RangedWeapon").GetComponent<DamageHandler>();

            var bullet = this.bulletLauncher.InstantiateBullet();

            bullet.Launch(target, this.damageHandler);

        }

    }
