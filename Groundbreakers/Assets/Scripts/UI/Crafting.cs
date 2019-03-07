using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;

public class Crafting : MonoBehaviour
{
    public GameObject ModulePrefab;
    private ModuleGeneric moduleGeneric;

    void Start()
    {
        GameObject masterModule = GameObject.Instantiate(this.ModulePrefab, this.transform);
        this.moduleGeneric = masterModule.GetComponent<ModuleGeneric>();
    }

    private void Reset()
    {
        this.moduleGeneric.POW = 0;
        this.moduleGeneric.ROF = 0;
        this.moduleGeneric.RNG = 0;
        this.moduleGeneric.MOB = 0;
        this.moduleGeneric.AMP = 0;

        this.moduleGeneric.burstAE = false;
        this.moduleGeneric.ricochetAE = false;
        this.moduleGeneric.laserAE = false;
        this.moduleGeneric.splashAE = false;
        this.moduleGeneric.pierceAE = false;
        this.moduleGeneric.traceAE = false;
        this.moduleGeneric.whirlwindAE = false;
        this.moduleGeneric.reachAE = false;

        this.moduleGeneric.slowSE = false;
        this.moduleGeneric.stunSE = false;
        this.moduleGeneric.burnSE = false;
        this.moduleGeneric.markSE = false;
        this.moduleGeneric.purgeSE = false;
        this.moduleGeneric.breakSE = false;
        this.moduleGeneric.blightSE = false;
        this.moduleGeneric.netSE = false;

        this.moduleGeneric.rarity = 0;
    }

    private void SetupDescription()
    {
        int index = 0;

        if (this.moduleGeneric.POW != 0)
        {
            this.moduleGeneric.description[index] = "POW +" + this.moduleGeneric.POW;
            index++;
        }

        if (this.moduleGeneric.ROF != 0)
        {
            this.moduleGeneric.description[index] = "ROF +" + this.moduleGeneric.ROF;
            index++;
        }

        if (this.moduleGeneric.RNG != 0)
        {
            this.moduleGeneric.description[index] = "RNG +" + this.moduleGeneric.RNG;
            index++;
        }

        if (this.moduleGeneric.MOB != 0)
        {
            this.moduleGeneric.description[index] = "MOB +" + this.moduleGeneric.MOB;
            index++;
        }

        if (this.moduleGeneric.AMP != 0)
        {
            this.moduleGeneric.description[index] = "AMP +" + this.moduleGeneric.AMP;
            index++;
        }

        if (this.moduleGeneric.burstAE)
        {
            this.moduleGeneric.description[index] = "Burst";
            index++;
        }

        if (this.moduleGeneric.ricochetAE)
        {
            this.moduleGeneric.description[index] = "Ricochet";
            index++;
        }

        if (this.moduleGeneric.laserAE)
        {
            this.moduleGeneric.description[index] = "Laser";
            index++;
        }

        if (this.moduleGeneric.splashAE)
        {
            this.moduleGeneric.description[index] = "Splash";
            index++;
        }

        if (this.moduleGeneric.pierceAE)
        {
            this.moduleGeneric.description[index] = "Pierce";
            index++;
        }

        if (this.moduleGeneric.traceAE)
        {
            this.moduleGeneric.description[index] = "Trace";
            index++;
        }

        if (this.moduleGeneric.whirlwindAE)
        {
            this.moduleGeneric.description[index] = "Whirlwind";
            index++;
        }

        if (this.moduleGeneric.reachAE)
        {
            this.moduleGeneric.description[index] = "Reach";
            index++;
        }

        if (this.moduleGeneric.slowSE)
        {
            this.moduleGeneric.description[index] = "Slow";
            index++;
        }

        if (this.moduleGeneric.stunSE)
        {
            this.moduleGeneric.description[index] = "Stun";
            index++;
        }

        if (this.moduleGeneric.burnSE)
        {
            this.moduleGeneric.description[index] = "Burn";
            index++;
        }

        if (this.moduleGeneric.markSE)
        {
            this.moduleGeneric.description[index] = "Mark";
            index++;
        }

        if (this.moduleGeneric.purgeSE)
        {
            this.moduleGeneric.description[index] = "Purge";
            index++;
        }

        if (this.moduleGeneric.breakSE)
        {
            this.moduleGeneric.description[index] = "Break";
            index++;
        }

        if (this.moduleGeneric.blightSE)
        {
            this.moduleGeneric.description[index] = "Blight";
            index++;
        }

        if (this.moduleGeneric.netSE)
        {
            this.moduleGeneric.description[index] = "Net";
            index++;
        }
    }

    public void Craft(String baseAttribute)
    {
        this.Reset();

        switch (baseAttribute)
        {
            case "POW I":
                this.moduleGeneric.POW = 1;
                break;
            case "POW II":
                this.moduleGeneric.POW = 2;
                break;
            case "POW III":
                this.moduleGeneric.POW = 3;
                break;
            case "ROF I":
                this.moduleGeneric.ROF = 1;
                break;
            case "ROF II":
                this.moduleGeneric.ROF = 2;
                break;
            case "ROF III":
                this.moduleGeneric.ROF = 3;
                break;
            case "RNG I":
                this.moduleGeneric.RNG = 1;
                break;
            case "RNG II":
                this.moduleGeneric.RNG = 2;
                break;
            case "RNG III":
                this.moduleGeneric.RNG = 3;
                break;
            case "MOB I":
                this.moduleGeneric.MOB = 1;
                break;
            case "MOB II":
                this.moduleGeneric.MOB = 2;
                break;
            case "MOB III":
                this.moduleGeneric.MOB = 3;
                break;
            case "AMP I":
                this.moduleGeneric.AMP = 1;
                break;
            case "AMP II":
                this.moduleGeneric.AMP = 2;
                break;
            case "AMP III":
                this.moduleGeneric.AMP = 3;
                break;
        }

        // Generate a number between 0-99
        System.Random rnd = new System.Random();
        int rarityIndex = rnd.Next(100);

        // Decide the rarity based on the index
        // 5% chance to get a Groundbreaking
        // 15% chance to get an Ideal
        // 30% chance to get a Modified
        // 50% chance to get a Common
        if (rarityIndex > 94)
        {
            this.moduleGeneric.rarity = 3;
        }
        else if (rarityIndex > 79)
        {
            this.moduleGeneric.rarity = 2;
        }
        else if (rarityIndex > 49)
        {
            this.moduleGeneric.rarity = 1;
        }
        else
        {
            this.moduleGeneric.rarity = 0;
        }

        // Generate extra attributes based on rarity
        for (int i = 0; i < this.moduleGeneric.rarity; i++)
        {
            // Generate a number between 0-100
            int attributeIndex = rnd.Next(101);

            if (attributeIndex > 90)
            {
                this.moduleGeneric.POW += 1;
            }
            else if (attributeIndex > 80)
            {
                this.moduleGeneric.ROF += 1;
            }
            else if (attributeIndex > 70)
            {
                this.moduleGeneric.RNG += 1;
            }
            else if (attributeIndex > 60)
            {
                this.moduleGeneric.MOB += 1;
            }
            else if (attributeIndex > 50)
            {
                this.moduleGeneric.AMP += 1;
            }
            else if (attributeIndex > 44)
            {
                this.moduleGeneric.slowSE = true;
            }
            else if (attributeIndex > 41)
            {
                this.moduleGeneric.stunSE = true;
            }
            else if (attributeIndex > 38)
            {
                this.moduleGeneric.burnSE = true;
            }
            else if (attributeIndex > 35)
            {
                this.moduleGeneric.markSE = true;
            }
            else if (attributeIndex > 32)
            {
                this.moduleGeneric.purgeSE = true;
            }
            else if (attributeIndex > 29)
            {
                this.moduleGeneric.breakSE = true;
            }
            else if (attributeIndex > 26)
            {
                this.moduleGeneric.blightSE = true;
            }
            else if (attributeIndex > 23)
            {
                this.moduleGeneric.netSE = true;
            }
            else if (attributeIndex > 20)
            {
                this.moduleGeneric.burstAE = true;
            }
            else if (attributeIndex > 17)
            {
                this.moduleGeneric.ricochetAE = true;
            }
            else if (attributeIndex > 14)
            {
                this.moduleGeneric.laserAE = true;
            }
            else if (attributeIndex > 11)
            {
                this.moduleGeneric.splashAE = true;
            }
            else if (attributeIndex > 8)
            {
                this.moduleGeneric.pierceAE = true;
            }
            else if (attributeIndex > 5)
            {
                this.moduleGeneric.traceAE = true;
            }
            else if (attributeIndex > 2)
            {
                this.moduleGeneric.whirlwindAE = true;
            }
            else
            {
                this.moduleGeneric.reachAE = true;
            }
        }

        this.SetupDescription();

        // Create a child clone under the parent Inventory
        Inventory inventory = Resources.FindObjectsOfTypeAll<Inventory>()[0];
        inventory.addModules(this.transform.GetChild(0).gameObject);
    }
}
