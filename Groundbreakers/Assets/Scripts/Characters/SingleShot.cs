using Assets.Scripts;
using UnityEngine;

public class SingleShot : MonoBehaviour
{
    private BulletLauncher bulletLauncher;

    private DamageHandler damageHandler;

    public void singleShot(Transform target) {

       this.bulletLauncher = this.GetComponentInChildren<BulletLauncher>();

       this.damageHandler = GameObject.Find("RangedWeapon").GetComponent<DamageHandler>();
           
       var bullet = this.bulletLauncher.InstantiateBullet();

       bullet.Launch(target, this.damageHandler);
          
    }
}
