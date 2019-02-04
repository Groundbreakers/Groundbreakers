using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentLevel : MonoBehaviour
{
    public Text ui;
    public Text ui2;
    private int region;
    private int level;
    
    // Start is called before the first frame update
    void Start()
    {
        this.region = 1;
        this.level = 1;
        this.ui.text = "Region " + this.region;
        this.ui2.text = "Level " + this.level + "/8";
        this.ChangeRegion();
    }

    public void ChangeRegion()
    {
        // Get a new BGM if the region is changed
        GameObject bgm = GameObject.Find("BGM Manager");
        Manager manager = bgm.GetComponent<Manager>();
        manager.UpdateBGM();

        // Get a new background image if the region is changed
        GameObject canvas = GameObject.Find("Canvas");
        Background background = canvas.GetComponent<Background>();
        background.UpdateBackground();
    }

    public void UpdateLevel()
    {
        if (this.level == 8)
        {
            this.region += 1;
            this.level = 1;
            this.ChangeRegion();
        }
        else
        {
            this.level += 1;
        }

        this.ui.text = "Region " + this.region;
        this.ui2.text = "Level " + this.level + "/8";
    }

    public int GetRegion()
    {
        return this.region;
    }

    public int GetLevel()
    {
        return this.level;
    }
}
