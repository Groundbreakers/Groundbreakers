using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class MultiShot : MonoBehaviour
{
    private BulletLauncher bulletLauncher;
    private DamageHandler damageHandler;

    // Subject to change
    public void multiShot(Transform target)
    {
        this.bulletLauncher = this.GetComponentInChildren<BulletLauncher>();
        this.damageHandler = GameObject.Find("RangedWeapon").GetComponent<DamageHandler>();
        
        //this.bulletLauncher.SetHandlerAttributeIfNot();

        var bulletA = bulletLauncher.InstantiateBullet();
        var bulletB = bulletLauncher.InstantiateBullet();
        var bulletC = bulletLauncher.InstantiateBullet();

        // Subject to change
        this.bulletLauncher.transform.LookAt(target.position);
        var directionA = this.bulletLauncher.transform.forward;
        var directionB = Quaternion.AngleAxis(-45, Vector3.up) * directionA;
        var directionC = Quaternion.AngleAxis(45, Vector3.up) * directionA;

        bulletA.Launch(target, this.damageHandler);
        bulletB.Launch(directionB, this.damageHandler);
        bulletC.Launch(directionC, this.damageHandler);
    }
}
