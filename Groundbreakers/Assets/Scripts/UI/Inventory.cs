using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject ui;
    public GameObject content;
    public GameObject characterManager;
    public GameObject tooltip;

    public GameObject[] inventory = new GameObject[5];

    private int count;
    private int characterIndex;
    private int[] availableSlots = {5, 5, 5, 5, 5};

    public void UpdateInventory()
    {
        // Reset tooltip
        this.ClearTooltip();

        // Rearrange the character's module
        this.RearrangeModules();

        // Disable all character inventories
        for (int i = 0; i < 5; i++)
        {
            this.inventory[i].SetActive(false);
        }

        // Active a specific inventory
        this.inventory[this.characterIndex].SetActive(true);

        // Calculate how many modules in the inventory and change the height of the content according to that
        this.count = 0;
        foreach (Transform child in this.content.transform)
        {
            if (child.gameObject.activeSelf)
            {
                this.count++;
            }
        }

        RectTransform rt = this.content.GetComponent<RectTransform>(); 
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, ((int)((this.count - 1) / 4) + 1) * 150);

        // Update character attributes
        GameObject.Find("CharacterList").transform.GetChild(this.characterIndex).GetComponent<characterAttributes>().updateAttributes(this.GetCharacterInventory(this.characterIndex));
    }

    public void Toggle()
    {
        this.characterIndex = this.characterManager.GetComponent<CharacterManager>().GetCharacterIndex();
        this.UpdateInventory();
        this.ui.SetActive(!this.ui.activeSelf);
    }

    public int GetCharacterIndex()
    {
        return this.characterIndex;
    }

    public int GetAvailableSlots()
    {
        return this.availableSlots[this.characterIndex];
    }

    public void SetAvailableSlots(int count)
    {
        this.availableSlots[this.characterIndex] += count;
    }

    public void ClearTooltip()
    {
        // Reset the title
        GameObject tooltipTitle = this.tooltip.transform.GetChild(0).gameObject;
        Text titleText = tooltipTitle.GetComponent<Text>();
        titleText.text = string.Empty;

        // Reset the description
        GameObject tooltipDescription = this.tooltip.transform.GetChild(1).gameObject;
        Text descriptionText = tooltipDescription.GetComponent<Text>();
        descriptionText.text = string.Empty;

        // Disable the tooltip button
        GameObject tooltipButton = this.tooltip.transform.GetChild(2).gameObject;
        tooltipButton.SetActive(false);

        // Disable the notEnoughSlot text
        GameObject notEnoughSlot = this.tooltip.transform.GetChild(3).gameObject;
        notEnoughSlot.SetActive(false);
    }

    public void RearrangeModules()
    {
        // If the inventory is not empty
        if (this.GetAvailableSlots() != 5)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // If a certain module slot is empty && the next slot is not empty
                    if (this.transform.GetChild(this.GetCharacterIndex()).GetChild(j).childCount == 0 &&
                        this.transform.GetChild(this.GetCharacterIndex()).GetChild(j + 1).childCount != 0)
                    {
                        // Move the module from the next slot to this slot
                        this.transform.GetChild(this.GetCharacterIndex()).GetChild(j + 1).GetChild(0).SetParent(
                            this.transform.GetChild(this.GetCharacterIndex()).GetChild(j));

                        // Reset its position
                        this.transform.GetChild(this.GetCharacterIndex()).GetChild(j).GetChild(0).localPosition = Vector3.zero;
                    }
                }
            }
        }
    }

    public int[] GetCharacterInventory(int index)
    {
        int[] sumAttributes = new int[22];
        for (int i = 0; i < 5; i++)
        {
            if (this.transform.GetChild(index).GetChild(i).childCount != 0)
            {
                ModuleGeneric script = this.transform.GetChild(index).GetChild(i).GetChild(0).gameObject
                    .GetComponent<ModuleGeneric>();
                int[] moduleAttributes = script.GetModuleAttributes();
                if (sumAttributes.Length == moduleAttributes.Length)
                {
                    for (int j = 0; j < 22; j++)
                    {
                        sumAttributes[j] = sumAttributes[j] + moduleAttributes[j];
                    }
                }
            }
        }

        return sumAttributes;
    }
}