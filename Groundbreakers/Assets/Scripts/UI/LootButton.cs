﻿using System;
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
        this.description.text = "Test";
        this.loot = module;
    }

    public void HandletoolTipButton()
    {
        this.Toggle();
    }

    public void HandleconfirmButton()
    {
        Debug.Log(title.text);
    }

    public void Toggle()
    {
        toolTip.SetActive(!toolTip.activeSelf);
    }
}