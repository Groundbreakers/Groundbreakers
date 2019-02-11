using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;

public class LootButton : MonoBehaviour
{
    public Button toolTipButton;
    public Image icon;
    public GameObject toolTip;
    public Text title;
    public Text description;
    public Button confirmButton;
    public GameObject loot;

    private ModuleTemplate script;

    // Start is called before the first frame update
    void Start()
    {
        toolTipButton.onClick.AddListener(HandletoolTipButton);
        confirmButton.onClick.AddListener(HandleconfirmButton);
    }

    public void Setup(GameObject module)
    {
        this.script = module.GetComponent<ModuleTemplate>();
        this.icon.sprite = this.script.icon;
        this.title.text = this.script.moduleTitle;
        this.description.text = this.script.descirption;
        this.loot = module;
    }

    public void HandletoolTipButton()
    {
        this.Toggle();
    }

    public void HandleconfirmButton()
    {
        // Create a child clone under the parent Inventory
        GameObject inventory = GameObject.Find("Inventory");
        GameObject newLoot = (GameObject)GameObject.Instantiate(this.loot, inventory.transform);

        // Close the tooltip panel
        this.Toggle();

        // Close the loot panel
        GameObject canvas = GameObject.Find("Canvas");
        Loot loot = canvas.GetComponent<Loot>();
        loot.Toggle();

        // I guess now we goto the next choice scene?
    }

    public void Toggle()
    {
        toolTip.SetActive(!toolTip.activeSelf);
    }
}
