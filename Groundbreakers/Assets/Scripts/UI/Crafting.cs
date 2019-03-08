using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public GameObject ui;
    public GameObject ModulePrefab;
    public GameObject button;
    public Text buttonText;
    public GameObject crystalCounter;
    public GameObject inventory;

    void Update()
    {
        if (this.crystalCounter.GetComponent<CrystalCounter>().GetCrystals() < 1000)
        {
            this.button.GetComponent<Button>().interactable = false;
            this.buttonText.text = "Not enough crystals";
        }
        else if (this.inventory.GetComponent<Inventory>().GetAvailableSlots() < 1)
        {
            this.button.GetComponent<Button>().interactable = false;
            this.buttonText.text = "Not enough slots";
        }
        else
        {
            this.button.GetComponent<Button>().interactable = true;
            this.buttonText.text = "Craft";
        }
    }

    private void SetupDescription(ModuleGeneric moduleGeneric)
    {
        int index = 0;

        if (moduleGeneric.POW != 0)
        {
            moduleGeneric.description[index] = "POW +" + moduleGeneric.POW;
            index++;
        }

        if (moduleGeneric.ROF != 0)
        {
            moduleGeneric.description[index] = "ROF +" + moduleGeneric.ROF;
            index++;
        }

        if (moduleGeneric.RNG != 0)
        {
            moduleGeneric.description[index] = "RNG +" + moduleGeneric.RNG;
            index++;
        }

        if (moduleGeneric.MOB != 0)
        {
            moduleGeneric.description[index] = "MOB +" + moduleGeneric.MOB;
            index++;
        }

        if (moduleGeneric.AMP != 0)
        {
            moduleGeneric.description[index] = "AMP +" + moduleGeneric.AMP;
            index++;
        }

        if (moduleGeneric.burstAE)
        {
            moduleGeneric.description[index] = "Burst";
            index++;
        }

        if (moduleGeneric.ricochetAE)
        {
            moduleGeneric.description[index] = "Ricochet";
            index++;
        }

        if (moduleGeneric.laserAE)
        {
            moduleGeneric.description[index] = "Laser";
            index++;
        }

        if (moduleGeneric.splashAE)
        {
            moduleGeneric.description[index] = "Splash";
            index++;
        }

        if (moduleGeneric.pierceAE)
        {
            moduleGeneric.description[index] = "Pierce";
            index++;
        }

        if (moduleGeneric.traceAE)
        {
            moduleGeneric.description[index] = "Trace";
            index++;
        }

        if (moduleGeneric.whirlwindAE)
        {
            moduleGeneric.description[index] = "Whirlwind";
            index++;
        }

        if (moduleGeneric.reachAE)
        {
            moduleGeneric.description[index] = "Reach";
            index++;
        }

        if (moduleGeneric.slowSE)
        {
            moduleGeneric.description[index] = "Slow";
            index++;
        }

        if (moduleGeneric.stunSE)
        {
            moduleGeneric.description[index] = "Stun";
            index++;
        }

        if (moduleGeneric.burnSE)
        {
            moduleGeneric.description[index] = "Burn";
            index++;
        }

        if (moduleGeneric.markSE)
        {
            moduleGeneric.description[index] = "Mark";
            index++;
        }

        if (moduleGeneric.purgeSE)
        {
            moduleGeneric.description[index] = "Purge";
            index++;
        }

        if (moduleGeneric.breakSE)
        {
            moduleGeneric.description[index] = "Break";
            index++;
        }

        if (moduleGeneric.blightSE)
        {
            moduleGeneric.description[index] = "Blight";
            index++;
        }

        if (moduleGeneric.netSE)
        {
            moduleGeneric.description[index] = "Net";
            index++;
        }
    }

    public void Craft(String baseAttribute)
    {
        this.crystalCounter.GetComponent<CrystalCounter>().SetCrystals(-1000);

        GameObject masterModule = GameObject.Instantiate(this.ModulePrefab);
        masterModule.transform.localScale = Vector3.zero;
        ModuleGeneric moduleGeneric = masterModule.GetComponent<ModuleGeneric>();

        switch (baseAttribute)
        {
            case "POW I":
                moduleGeneric.POW = 1;
                break;
            case "POW II":
                moduleGeneric.POW = 2;
                break;
            case "POW III":
                moduleGeneric.POW = 3;
                break;
            case "ROF I":
                moduleGeneric.ROF = 1;
                break;
            case "ROF II":
                moduleGeneric.ROF = 2;
                break;
            case "ROF III":
                moduleGeneric.ROF = 3;
                break;
            case "RNG I":
                moduleGeneric.RNG = 1;
                break;
            case "RNG II":
                moduleGeneric.RNG = 2;
                break;
            case "RNG III":
                moduleGeneric.RNG = 3;
                break;
            case "MOB I":
                moduleGeneric.MOB = 1;
                break;
            case "MOB II":
                moduleGeneric.MOB = 2;
                break;
            case "MOB III":
                moduleGeneric.MOB = 3;
                break;
            case "AMP I":
                moduleGeneric.AMP = 1;
                break;
            case "AMP II":
                moduleGeneric.AMP = 2;
                break;
            case "AMP III":
                moduleGeneric.AMP = 3;
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
            moduleGeneric.rarity = 3;
        }
        else if (rarityIndex > 79)
        {
            moduleGeneric.rarity = 2;
        }
        else if (rarityIndex > 49)
        {
            moduleGeneric.rarity = 1;
        }
        else
        {
            moduleGeneric.rarity = 0;
        }

        // Generate extra attributes based on rarity
        for (int i = 0; i < moduleGeneric.rarity; i++)
        {
            // Generate a number between 0-100
            int attributeIndex = rnd.Next(101);

            if (attributeIndex > 90)
            {
                moduleGeneric.POW += 1;
            }
            else if (attributeIndex > 80)
            {
                moduleGeneric.ROF += 1;
            }
            else if (attributeIndex > 70)
            {
                moduleGeneric.RNG += 1;
            }
            else if (attributeIndex > 60)
            {
                moduleGeneric.MOB += 1;
            }
            else if (attributeIndex > 50)
            {
                moduleGeneric.AMP += 1;
            }
            else if (attributeIndex > 44)
            {
                moduleGeneric.slowSE = true;
            }
            else if (attributeIndex > 41)
            {
                moduleGeneric.stunSE = true;
            }
            else if (attributeIndex > 38)
            {
                moduleGeneric.burnSE = true;
            }
            else if (attributeIndex > 35)
            {
                moduleGeneric.markSE = true;
            }
            else if (attributeIndex > 32)
            {
                moduleGeneric.purgeSE = true;
            }
            else if (attributeIndex > 29)
            {
                moduleGeneric.breakSE = true;
            }
            else if (attributeIndex > 26)
            {
                moduleGeneric.blightSE = true;
            }
            else if (attributeIndex > 23)
            {
                moduleGeneric.netSE = true;
            }
            else if (attributeIndex > 20)
            {
                moduleGeneric.burstAE = true;
            }
            else if (attributeIndex > 17)
            {
                moduleGeneric.ricochetAE = true;
            }
            else if (attributeIndex > 14)
            {
                moduleGeneric.laserAE = true;
            }
            else if (attributeIndex > 11)
            {
                moduleGeneric.splashAE = true;
            }
            else if (attributeIndex > 8)
            {
                moduleGeneric.pierceAE = true;
            }
            else if (attributeIndex > 5)
            {
                moduleGeneric.traceAE = true;
            }
            else if (attributeIndex > 2)
            {
                moduleGeneric.whirlwindAE = true;
            }
            else
            {
                moduleGeneric.reachAE = true;
            }
        }

        moduleGeneric.title = baseAttribute;
        this.SetupDescription(moduleGeneric);

        // Create a child clone under the parent Inventory
        this.inventory.GetComponent<Inventory>().addModules(masterModule);
    }

    public void Toggle()
    {
        this.ui.SetActive(!this.ui.activeSelf);
    }
}
