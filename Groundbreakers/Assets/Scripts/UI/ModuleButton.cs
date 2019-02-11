using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{
    public Button button;
    public Image icon;
    public String title;
    public String description;
    public int slot;
    public GameObject selectedModule;

    public int characterIndex;

    private ModuleTemplate script;
    private GameObject tooltip;
    private GameObject characterList;

    // Start is called before the first frame update
    void Start()
    {
        this.tooltip = GameObject.Find("ModuleTooltip");
        this.characterList = GameObject.Find("CharacterList");
        this.button.onClick.AddListener(this.HandleTooltip);
    }

    public void Setup(GameObject module, int index)
    {
        this.script = module.GetComponent<ModuleTemplate>();
        this.icon.sprite = this.script.icon;
        this.title = this.script.moduleTitle;
        this.description = this.script.descirption;
        this.slot = this.script.slot;
        this.selectedModule = module;
        this.characterIndex = index;
    }

    public void HandleTooltip()
    {
        // Show description on the tooltip panel
        GameObject tooltipTitle = this.tooltip.transform.GetChild(0).gameObject;
        Text titleText = tooltipTitle.GetComponent<Text>();
        titleText.text = this.title;
        GameObject tooltipDescription = this.tooltip.transform.GetChild(1).gameObject;
        Text descriptionText = tooltipDescription.GetComponent<Text>();
        descriptionText.text = this.description;
        GameObject notEnoughSlot = this.tooltip.transform.GetChild(3).gameObject;
        notEnoughSlot.SetActive(false);

        // Check if the character has enough module slots
        int availableSlots = 999;
            // = characterList.transform.GetChild(this.characterIndex).GetComponent<characterAttributes>().availableSlots;
        if (this.slot <= availableSlots)
        {
            this.UpdatetooltipButton();
        }
        else
        {
            notEnoughSlot.SetActive(true);
        }
    }

    public void UpdatetooltipButton()
    {
        // Find the tooltip button
        GameObject tooltipButton = this.tooltip.transform.GetChild(2).gameObject;
        Button tooltipButtonComponent = tooltipButton.GetComponent<Button>();
        tooltipButtonComponent.onClick.RemoveAllListeners();
        tooltipButtonComponent.onClick.AddListener(this.EquipModule);
        GameObject tooltipButtonText = tooltipButton.transform.GetChild(0).gameObject;
        Text buttonText = tooltipButtonText.GetComponent<Text>();
        buttonText.text = "Equip";
        tooltipButton.SetActive(true);
    }

    public void EquipModule()
    {
        this.selectedModule.transform.SetParent(this.characterList.transform.GetChild(this.characterIndex));
        GameObject canvas = GameObject.Find("Canvas");
        ModulePool modulePool = canvas.GetComponent<ModulePool>();
        modulePool.UpdatePool();
    }
}
