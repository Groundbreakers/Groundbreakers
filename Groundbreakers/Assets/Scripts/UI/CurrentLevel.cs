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
        this.ui.text = this.region + "-" + this.level;
    }
}
