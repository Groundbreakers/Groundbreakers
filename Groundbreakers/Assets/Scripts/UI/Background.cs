using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public RawImage backgroundRawImage;
    public Texture background1;
    public Texture background2;
    public Texture background3;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        CurrentLevel currentLevel = canvas.GetComponent<CurrentLevel>();
        if (currentLevel.region == 1)
            backgroundRawImage.texture = this.background1;
        else if (currentLevel.region == 2)
            backgroundRawImage.texture = this.background2;
        else
            backgroundRawImage.texture = this.background3;
    }

    public void UpdateBackground()
    {
        GameObject canvas = GameObject.Find("Canvas");
        CurrentLevel currentLevel = canvas.GetComponent<CurrentLevel>();
        if (currentLevel.region == 1)
            backgroundRawImage.texture = this.background1;
        else if (currentLevel.region == 2)
            backgroundRawImage.texture = this.background2;
        else
            backgroundRawImage.texture = this.background3;
    }
}
