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
    public CurrentLevel currentLevel;

    public void UpdateBackground()
    {
        if (this.currentLevel.GetRegion() == 1)
            backgroundRawImage.texture = this.background1;
        else if (this.currentLevel.GetRegion() == 2)
            backgroundRawImage.texture = this.background2;
        else
            backgroundRawImage.texture = this.background3;
    }
}
