using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePool : MonoBehaviour
{
    public GameObject ui;
    public GameObject content;
    public GameObject prefab;

    private int count;
    private int characterIndex;

    public void UpdatePool()
    {
        // If there are less clones than we need, create more
        GameObject inventory = GameObject.Find("Inventory");
        while (this.content.transform.childCount < inventory.transform.childCount)
        {
            GameObject newModule = (GameObject)GameObject.Instantiate(this.prefab, this.content.transform);
        }

        // Set all clones to inactive
        foreach (Transform child in this.content.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Set up all clones
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            // Active the clones
            GameObject newModule = this.content.transform.GetChild(i).gameObject;
            newModule.SetActive(true);

            // Setup the component of the clones
            ModuleButton module = newModule.GetComponent<ModuleButton>();
            module.Setup(inventory.transform.GetChild(i).gameObject, this.characterIndex);
        }

        // Calculate how many modules in the inventory and change the height of the content according to that
        this.count = 0;
        foreach (Transform child in this.content.transform)
        {
            if (child.gameObject.activeSelf)
                this.count++;
        }
        RectTransform rt = this.content.GetComponent<RectTransform>(); 
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, ((int)((this.count - 1) / 4) + 1) * 150);
    }

    public void Toggle()
    {
        this.characterIndex = this.GetComponent<Characters>().GetCharacterIndex();
        this.UpdatePool();
        ui.SetActive(!ui.activeSelf);
    }
}
