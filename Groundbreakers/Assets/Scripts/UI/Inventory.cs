using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IDropHandler
{
    public GameObject ui;
    public GameObject modules;
    public GameObject characterManager;

    public GameObject[] inventory = new GameObject[5];

    private int characterIndex;

    public void UpdateInventory()
    {
        // Disable all character inventories
        for (int i = 0; i < 5; i++)
        {
            this.inventory[i].SetActive(false);
        }

        // Active a specific inventory
        this.inventory[this.characterIndex].SetActive(true);

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

    public void addModules(GameObject module)
    {
        foreach (Transform child in this.modules.transform)
        {
            if (child.childCount == 0)
            {
                GameObject.Instantiate(module, child);
                break;
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

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
            if (eventData.pointerCurrentRaycast.gameObject.tag == "Slot")
            {
                eventData.pointerDrag.GetComponent<ModuleGeneric>()
                    .NewParent(eventData.pointerCurrentRaycast.gameObject.transform);
                Debug.Log("On top of: " + eventData.pointerCurrentRaycast.gameObject);
            }
            else if (eventData.pointerCurrentRaycast.gameObject.tag == "Recycle")
            {
                Destroy(eventData.pointerDrag);
            }
        }
    }
}