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

    public Image tooltipImage;
    public Text tooltipTitle;
    public Text tooltipDescription;

    public Sprite common;
    public Sprite modified;
    public Sprite ideal;
    public Sprite groundbreaking;

    public Sprite pow1;
    public Sprite pow2;
    public Sprite pow3;
    public Sprite rof1;
    public Sprite rof2;
    public Sprite rof3;
    public Sprite rng1;
    public Sprite rng2;
    public Sprite rng3;
    public Sprite mob1;
    public Sprite mob2;
    public Sprite mob3;
    public Sprite amp1;
    public Sprite amp2;
    public Sprite amp3;

    public Sprite burstAE;
    public Sprite ricochetAE;
    public Sprite laserAE;
    public Sprite splashAE;
    public Sprite pierceAE;
    public Sprite traceAE;
    public Sprite whirlwindAE;
    public Sprite reachAE;

    public Sprite slowSE;
    public Sprite stunSE;
    public Sprite burnSE;
    public Sprite markSE;
    public Sprite purgeSE;
    public Sprite breakSE;
    public Sprite blightSE;
    public Sprite netSE;

    public Sprite slot;

    private String baseAttribute;
    private int cost;

    void Start()
    {
        this.baseAttribute = "none";
        this.cost = 0;
    }

    void Update()
    {
        if (this.crystalCounter.GetComponent<CrystalCounter>().GetCrystals() < this.cost)
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

    public void Select(String text)
    {
        this.baseAttribute = text;
        this.tooltipTitle.text = this.baseAttribute;
        switch (this.baseAttribute)
        {
            case "POW I":
                this.cost = 500;
                this.tooltipImage.sprite = this.pow1;
                this.tooltipDescription.text = "POW (Power) affects the damage you do to enemies.";
                break;
            case "POW II":
                this.cost = 1000;
                this.tooltipImage.sprite = this.pow2;
                this.tooltipDescription.text = "POW (Power) affects the damage you do to enemies.";
                break;
            case "POW III":
                this.cost = 1500;
                this.tooltipImage.sprite = this.pow3;
                this.tooltipDescription.text = "POW (Power) affects the damage you do to enemies.";
                break;
            case "ROF I":
                this.cost = 500;
                this.tooltipImage.sprite = this.rof1;
                this.tooltipDescription.text = "ROF (Rate of Fire affects) affects how often you attack.";
                break;
            case "ROF II":
                this.cost = 1000;
                this.tooltipImage.sprite = this.rof2;
                this.tooltipDescription.text = "ROF (Rate of Fire affects) affects how often you attack.";
                break;
            case "ROF III":
                this.cost = 1500;
                this.tooltipImage.sprite = this.rof3;
                this.tooltipDescription.text = "ROF (Rate of Fire affects) affects how often you attack.";
                break;
            case "RNG I":
                this.cost = 500;
                this.tooltipImage.sprite = this.rng1;
                this.tooltipDescription.text = "RNG (Range) affects how far your attacks can go.";
                break;
            case "RNG II":
                this.cost = 1000;
                this.tooltipImage.sprite = this.rng2;
                this.tooltipDescription.text = "RNG (Range) affects how far your attacks can go.";
                break;
            case "RNG III":
                this.cost = 1500;
                this.tooltipImage.sprite = this.rng3;
                this.tooltipDescription.text = "RNG (Range) affects how far your attacks can go.";
                break;
            case "MOB I":
                this.cost = 500;
                this.tooltipImage.sprite = this.mob1;
                this.tooltipDescription.text = "MOB (Mobility) affects how fast you can deploy.";
                break;
            case "MOB II":
                this.cost = 1000;
                this.tooltipImage.sprite = this.mob2;
                this.tooltipDescription.text = "MOB (Mobility) affects how fast you can deploy.";
                break;
            case "MOB III":
                this.cost = 1500;
                this.tooltipImage.sprite = this.mob3;
                this.tooltipDescription.text = "MOB (Mobility) affects how fast you can deploy.";
                break;
            case "AMP I":
                this.cost = 500;
                this.tooltipImage.sprite = this.amp1;
                this.tooltipDescription.text = "AMP (Armor Penetration) affects the damage you do to armored enemies.";
                break;
            case "AMP II":
                this.cost = 1000;
                this.tooltipImage.sprite = this.amp2;
                this.tooltipDescription.text = "AMP (Armor Penetration) affects the damage you do to armored enemies.";
                break;
            case "AMP III":
                this.cost = 1500;
                this.tooltipImage.sprite = this.amp3;
                this.tooltipDescription.text = "AMP (Armor Penetration) affects the damage you do to armored enemies.";
                break;
            case "BURST":
                this.cost = 2500;
                this.tooltipImage.sprite = this.burstAE;
                this.tooltipDescription.text = "BURST allows you to shoot 3 bullet at a time.";
                break;
            case "RICOCHET":
                this.cost = 2500;
                this.tooltipImage.sprite = this.ricochetAE;
                this.tooltipDescription.text = "RICOCHET allows your bullets to bounce to nearby enemies.";
                break;
            case "LASER":
                this.cost = 2500;
                this.tooltipImage.sprite = this.laserAE;
                this.tooltipDescription.text = "LASER multiplies your rate of fire but decrease your damage.";
                break;
            case "SPLASH":
                this.cost = 2500;
                this.tooltipImage.sprite = this.splashAE;
                this.tooltipDescription.text = "SPLASH allows your attacks to do damage to all nearby enemies.";
                break;
            case "PIERCE":
                this.cost = 2500;
                this.tooltipImage.sprite = this.pierceAE;
                this.tooltipDescription.text = "PIERCE allows your attacks to do damage to all nearby enemies.";
                break;
            case "TRACE":
                this.cost = 2500;
                this.tooltipImage.sprite = this.traceAE;
                this.tooltipDescription.text = "TRACE allows your bullets to chase after enemies.";
                break;
            case "WHIRLWIND":
                this.cost = 3000;
                this.tooltipImage.sprite = this.whirlwindAE;
                this.tooltipDescription.text = "WHIRLWIND allows you to do spin attacks (melee only).";
                break;
            case "REACH":
                this.cost = 3000;
                this.tooltipImage.sprite = this.reachAE;
                this.tooltipDescription.text = "REACH allows you to do range attacks in melee mode (melee only).";
                break;
            case "SLOW":
                this.cost = 1500;
                this.tooltipImage.sprite = this.slowSE;
                this.tooltipDescription.text = "Attacks decrease enemies movement speed.";
                break;
            case "STUN":
                this.cost = 1500;
                this.tooltipImage.sprite = this.stunSE;
                this.tooltipDescription.text = "Attacks stop enemies for a short period of time.";
                break;
            case "BURN":
                this.cost = 1500;
                this.tooltipImage.sprite = this.burnSE;
                this.tooltipDescription.text = "Enemies take damage over time.";
                break;
            case "MARK":
                this.cost = 1500;
                this.tooltipImage.sprite = this.markSE;
                this.tooltipDescription.text = "Enemies take extra damage from attacks.";
                break;
            case "PURGE":
                this.cost = 1500;
                this.tooltipImage.sprite = this.purgeSE;
                this.tooltipDescription.text = "Disable enemy's Aura and Revenge.";
                break;
            case "BREAK":
                this.cost = 1500;
                this.tooltipImage.sprite = this.breakSE;
                this.tooltipDescription.text = "Disable enemy's Armor.";
                break;
            case "BLIGHT":
                this.cost = 1500;
                this.tooltipImage.sprite = this.blightSE;
                this.tooltipDescription.text = "Enemies take damage over time. Stacks.";
                break;
            case "NET":
                this.cost = 1500;
                this.tooltipImage.sprite = this.netSE;
                this.tooltipDescription.text = "Enemies cannot evade.";
                break;
            default:
                this.cost = 0;
                this.tooltipImage.sprite = this.slot;
                this.tooltipTitle.text = "module";
                this.tooltipDescription.text = "description";
                break;
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

    public void Craft()
    {
        this.crystalCounter.GetComponent<CrystalCounter>().SetCrystals(-this.cost);

        GameObject masterModule = GameObject.Instantiate(this.ModulePrefab);
        masterModule.transform.localScale = Vector3.zero;
        ModuleGeneric moduleGeneric = masterModule.GetComponent<ModuleGeneric>();

        switch (this.baseAttribute)
        {
            case "POW I":
                moduleGeneric.POW = 1;
                moduleGeneric.icon.sprite = this.pow1;
                break;
            case "POW II":
                moduleGeneric.POW = 2;
                moduleGeneric.icon.sprite = this.pow2;
                break;
            case "POW III":
                moduleGeneric.POW = 3;
                moduleGeneric.icon.sprite = this.pow3;
                break;
            case "ROF I":
                moduleGeneric.ROF = 1;
                moduleGeneric.icon.sprite = this.rof1;
                break;
            case "ROF II":
                moduleGeneric.ROF = 2;
                moduleGeneric.icon.sprite = this.rof2;
                break;
            case "ROF III":
                moduleGeneric.ROF = 3;
                moduleGeneric.icon.sprite = this.rof3;
                break;
            case "RNG I":
                moduleGeneric.RNG = 1;
                moduleGeneric.icon.sprite = this.rng1;
                break;
            case "RNG II":
                moduleGeneric.RNG = 2;
                moduleGeneric.icon.sprite = this.rng2;
                break;
            case "RNG III":
                moduleGeneric.RNG = 3;
                moduleGeneric.icon.sprite = this.rng3;
                break;
            case "MOB I":
                moduleGeneric.MOB = 1;
                moduleGeneric.icon.sprite = this.mob1;
                break;
            case "MOB II":
                moduleGeneric.MOB = 2;
                moduleGeneric.icon.sprite = this.mob2;
                break;
            case "MOB III":
                moduleGeneric.MOB = 3;
                moduleGeneric.icon.sprite = this.mob3;
                break;
            case "AMP I":
                moduleGeneric.AMP = 1;
                moduleGeneric.icon.sprite = this.amp1;
                break;
            case "AMP II":
                moduleGeneric.AMP = 2;
                moduleGeneric.icon.sprite = this.amp2;
                break;
            case "AMP III":
                moduleGeneric.AMP = 3;
                moduleGeneric.icon.sprite = this.amp3;
                break;
            case "BURST":
                moduleGeneric.burstAE = true;
                moduleGeneric.icon.sprite = this.burstAE;
                break;
            case "RICOCHET":
                moduleGeneric.ricochetAE = true;
                moduleGeneric.icon.sprite = this.ricochetAE;
                break;
            case "LASER":
                moduleGeneric.laserAE = true;
                moduleGeneric.icon.sprite = this.laserAE;
                break;
            case "SPLASH":
                moduleGeneric.splashAE = true;
                moduleGeneric.icon.sprite = this.splashAE;
                break;
            case "PIERCE":
                moduleGeneric.pierceAE = true;
                moduleGeneric.icon.sprite = this.pierceAE;
                break;
            case "TRACE":
                moduleGeneric.traceAE = true;
                moduleGeneric.icon.sprite = this.traceAE;
                break;
            case "WHIRLWIND":
                moduleGeneric.whirlwindAE = true;
                moduleGeneric.icon.sprite = this.whirlwindAE;
                break;
            case "REACH":
                moduleGeneric.reachAE = true;
                moduleGeneric.icon.sprite = this.reachAE;
                break;
            case "SLOW":
                moduleGeneric.slowSE = true;
                moduleGeneric.icon.sprite = this.slowSE;
                break;
            case "STUN":
                moduleGeneric.stunSE = true;
                moduleGeneric.icon.sprite = this.stunSE;
                break;
            case "BURN":
                moduleGeneric.burnSE = true;
                moduleGeneric.icon.sprite = this.burnSE;
                break;
            case "MARK":
                moduleGeneric.markSE = true;
                moduleGeneric.icon.sprite = this.markSE;
                break;
            case "PURGE":
                moduleGeneric.purgeSE = true;
                moduleGeneric.icon.sprite = this.purgeSE;
                break;
            case "BREAK":
                moduleGeneric.breakSE = true;
                moduleGeneric.icon.sprite = this.breakSE;
                break;
            case "BLIGHT":
                moduleGeneric.blightSE = true;
                moduleGeneric.icon.sprite = this.blightSE;
                break;
            case "NET":
                moduleGeneric.netSE = true;
                moduleGeneric.icon.sprite = this.netSE;
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
            moduleGeneric.background.sprite = this.groundbreaking;
        }
        else if (rarityIndex > 79)
        {
            moduleGeneric.rarity = 2;
            moduleGeneric.background.sprite = this.ideal;
        }
        else if (rarityIndex > 49)
        {
            moduleGeneric.rarity = 1;
            moduleGeneric.background.sprite = this.modified;
        }
        else
        {
            moduleGeneric.rarity = 0;
            moduleGeneric.background.sprite = this.common;
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

        moduleGeneric.title = this.baseAttribute;
        this.SetupDescription(moduleGeneric);

        // Create a child clone under the parent Inventory
        this.inventory.GetComponent<Inventory>().addModules(masterModule);
    }

    public void Toggle()
    {
        this.ui.SetActive(!this.ui.activeSelf);
    }
}
