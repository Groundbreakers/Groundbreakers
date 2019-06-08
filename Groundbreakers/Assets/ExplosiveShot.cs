using Assets.Scripts;
using UnityEngine;

public class ExplosiveShot : MonoBehaviour
{
    private BulletLauncher bulletLauncher;

    private DamageHandler damageHandler;

    public void explosiveShot(Transform target)
    {

        this.bulletLauncher = this.GetComponentInChildren<BulletLauncher>();

        this.damageHandler = GameObject.Find("RangedWeapon").GetComponent<DamageHandler>();

        var bullet = this.bulletLauncher.InstantiateBullet();

        bullet.Launch(target, this.damageHandler);

    }
}
