﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeployBar : MonoBehaviour
{
    public Image deployBar;
    public Image background;
    public characterAttributes character;
    private double max;

    void Start()
    {
        
        this.deployBar = this.GetComponent<Image>();
        this.background = this.transform.parent.gameObject.GetComponent<Image>();
        
        this.character = this.transform.parent.parent.parent.gameObject.GetComponent<characterAttributes>();
        Reset();
    }

    public void Reset()
    {
        deployBar.color = new Color(deployBar.color.r, deployBar.color.g, deployBar.color.b, 1f);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 1f);
        max = Time.time + (2 / (character.MOB * .5));
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time <= max)
        {
            this.deployBar.fillAmount = (float)max - (float)Time.time;
        }
        else
        {
            deployBar.color = new Color(deployBar.color.r, deployBar.color.g, deployBar.color.b, 0f);
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        }
        
    }
}