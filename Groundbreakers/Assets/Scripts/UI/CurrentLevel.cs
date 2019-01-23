using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentLevel : MonoBehaviour
{
    public Text ui;
    public int region;
    public int level;
    
    // Start is called before the first frame update
    void Start()
    {
        this.region = 1;
        this.level = 1;
        this.ui.text = this.region + "-" + this.level;
    }

    public void UpdateLevel()
    {
        if (this.level == 8)
        {
            this.region += 1;
            this.level = 1;

            // Get a new BGM if the region is changed
            GameObject bgm = GameObject.Find("BGM Manager");
            Manager manager = bgm.GetComponent<Manager>();
            manager.UpdateBGM();

            // Get a new background image if the region is changed
            GameObject canvas = GameObject.Find("Canvas");
            Background background = canvas.GetComponent<Background>();
            background.UpdateBackground();
        }
        else
        {
            this.level += 1;
        }

        this.ui.text = this.region + "-" + this.level;
    }
}
