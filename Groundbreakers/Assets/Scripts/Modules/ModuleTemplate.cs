using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleTemplate : MonoBehaviour
{
    public string moduleTitle;
    public int rarity;
    public int slot;
    public Sprite icon;
    public String descirption;

    // Basic Attributes
    public int POW;
    public int ROF;
    public int RNG;
    public int MOB;
    public int AMP;

    // Attack Effects
    public Boolean multiShotAE;
    public Boolean ricochetAE;
    public Boolean burstAE;
    public Boolean laserAE;
    public Boolean whirwindAE;
    public Boolean reachAE;
    public Boolean splashAE;
    public Boolean chargeAE;
    public Boolean trueStrikeAE;
    public Boolean antiAirAE;
    public Boolean nullifyAE;
    public Boolean chaosAE;

    // Status Effects
    public Boolean slowSE;
    public Boolean stunSE;
    public Boolean burnSE;
    public Boolean markSE;
    public Boolean purgeSE;
    public Boolean breakSE;
    public Boolean blightSE;
    public Boolean netSE;
}
