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

    private ModuleGeneric script;

    // Start is called before the first frame update
    void Start()
    {
        toolTipButton.onClick.AddListener(HandletoolTipButton);
        confirmButton.onClick.AddListener(HandleconfirmButton);
    }

    public void Setup(GameObject module)
    {
        this.icon.sprite = module.GetComponent<Image>().sprite;
        this.script = module.GetComponent<ModuleGeneric>();
        this.title.text = this.script.title;
        this.description.text = this.script.description;
        this.loot = module;
    }

    public void HandletoolTipButton()
    {
        this.Toggle();
    }

    public void HandleconfirmButton()
    {
        // Create a child clone under the parent Inventory
        Inventory inventory = Resources.FindObjectsOfTypeAll<Inventory>()[0];
        inventory.addModules(this.loot);

        // Close the tooltip panel
        this.Toggle();

        // Close the loot panel
        GameObject canvas = GameObject.Find("Canvas");
        LootGenerator loot = canvas.GetComponent<LootGenerator>();
        loot.Toggle();

        // Open the route panel
        RoutesGenerator routes = canvas.GetComponent<RoutesGenerator>();
        routes.Toggle();
    }

    public void Toggle()
    {
        toolTip.SetActive(!toolTip.activeSelf);
    }
}
