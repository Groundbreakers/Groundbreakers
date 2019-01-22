using System;
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
        // Call Battle function here
        GameObject canvas = GameObject.Find("Canvas");
        Choices choices = canvas.GetComponent<Choices>();
        choices.Toggle();

        // Update Current Level
        CurrentLevel currentLevel = canvas.GetComponent<CurrentLevel>();
        if (currentLevel.level == 8)
        {
            currentLevel.region += 1;
            currentLevel.level = 1;
        }
        else
            currentLevel.level += 1;
        currentLevel.UpdateLevel();

        // Get a new background image if the region is changed
        Background background = canvas.GetComponent<Background>();
        background.UpdateBackground();
        Debug.Log(title.text);
    }
}
