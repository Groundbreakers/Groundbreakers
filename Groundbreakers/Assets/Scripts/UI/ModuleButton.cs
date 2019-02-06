using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleButton : MonoBehaviour
{
    public Button toolTipButton;
    public Image icon;
    public String title;
    public String description;

    public int characterIndex;

    private ModuleTemplate script;

    // Start is called before the first frame update
    void Start()
    {
        toolTipButton.onClick.AddListener(HandletoolTipButton);
    }

    public void Setup(GameObject module)
    {
        this.script = module.GetComponent<ModuleTemplate>();
        this.icon.sprite = this.script.icon;
        this.title = this.script.moduleTitle;
        this.description = this.script.descirption;
    }

    public void HandletoolTipButton()
    {
        // Show description on the tooltip panel
        GameObject tooltipTitle = GameObject.Find("MTooltipTitle");
        Text titleText = tooltipTitle.GetComponent<Text>();
        titleText.text = this.title;
        GameObject tooltipDescription = GameObject.Find("MTooltipDescription");
        Text descriptionText = tooltipDescription.GetComponent<Text>();
        descriptionText.text = this.description;
    }
}
