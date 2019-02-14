using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleGeneric : MonoBehaviour
{
    // Title and description
    public Button button;
    public string title;
    public int rarity;
    public int slot;
    public String description;

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

    private GameObject tooltip;
    private Inventory inventory;

    private Boolean isEquipped;

    // Start is called before the first frame update
    void Start()
    {
        this.inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        this.tooltip = GameObject.Find("InventoryTooltip");
        this.button.onClick.AddListener(this.HandleTooltip);
    }

    public void HandleTooltip()
    {
        // Reference the tooltip
        GameObject tooltipTitle = this.tooltip.transform.GetChild(0).gameObject;
        Text titleText = tooltipTitle.GetComponent<Text>();
        titleText.text = this.title;

        // Rarity affects text color
        switch (this.rarity)
        {
            case 0:
                titleText.color = Color.black;
                break;
            case 1:
                titleText.color = new Color(0.0f, 0.4f, 1.0f);
                break;
            case 2:
                titleText.color = new Color(1.0f, 0.5f, 0.0f);
                break;
            default:
                titleText.color = Color.black;
                break;
        }

        // Update the descriptionText
        GameObject tooltipDescription = this.tooltip.transform.GetChild(1).gameObject;
        Text descriptionText = tooltipDescription.GetComponent<Text>();
        descriptionText.text = this.description;

        // Update the tooltipButton
        this.UpdatetooltipButton(this.isEquipped);
    }

    public void UpdatetooltipButton(Boolean b)
    {
        // Find the tooltip button and text
        GameObject tooltipButton = this.tooltip.transform.GetChild(2).gameObject;
        Button tooltipButtonComponent = tooltipButton.GetComponent<Button>();
        GameObject tooltipButtonText = tooltipButton.transform.GetChild(0).gameObject;
        Text buttonText = tooltipButtonText.GetComponent<Text>();

        // Reset the button
        tooltipButton.SetActive(false);
        tooltipButtonComponent.onClick.RemoveAllListeners();

        // Reference the notEnoughSlot text and disable it for now
        GameObject notEnoughSlot = this.tooltip.transform.GetChild(3).gameObject;
        notEnoughSlot.SetActive(false);

        // Check if this module is equipped
        if (b)
        {
            tooltipButtonComponent.onClick.AddListener(this.RemoveModule);
            buttonText.text = "Remove";
            tooltipButton.SetActive(true);
        }
        else
        {
            // If the module is not equipped and the character has enough module slots
            int availableSlots = this.inventory.GetAvailableSlots();

            if (this.slot <= availableSlots)
            {
                tooltipButtonComponent.onClick.AddListener(this.EquipModule);
                buttonText.text = "Equip";
                tooltipButton.SetActive(true);
            }
            else
            {
                notEnoughSlot.SetActive(true);
            }
        }
    }

    public void EquipModule()
    {
        if (!this.isEquipped)
        {
            int availableSlots = this.inventory.GetAvailableSlots();
            this.transform.SetParent(this.inventory.transform.GetChild(this.inventory.GetCharacterIndex()).GetChild(5 - availableSlots));
            this.transform.localPosition = Vector3.zero;
            this.inventory.UpdateInventory();
            this.isEquipped = true;
            this.inventory.SetAvailableSlots(-this.slot);
        }
    }

    public void RemoveModule()
    {
        if (this.isEquipped)
        {
            this.transform.SetParent(this.inventory.content.transform);
            this.inventory.UpdateInventory();
            this.isEquipped = false;
            this.inventory.SetAvailableSlots(this.slot);
        }
    }
}
