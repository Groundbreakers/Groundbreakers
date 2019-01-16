﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;

public class ChoicesButton : MonoBehaviour
{
    public Button button;
    public Image icon;
    public Text title;
    public Text description;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(HandleClick);
    }

    public void Setup(Sprite choiceIcon, String choiceTitle, String choiceDescription)
    {
        this.icon.sprite = choiceIcon;
        this.title.text = choiceTitle;
        this.description.text = choiceDescription;
    }

    public void HandleClick()
    {
        Debug.Log(title.text);
    }
}