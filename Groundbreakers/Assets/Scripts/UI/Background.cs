using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public RawImage backgroundRawImage;
    public Texture background1;
    public Texture background2;
    public Texture background3;

    public void UpdateBackground()
    {
        var region = GameObject.Find("LevelManager").GetComponent<LevelManager>().Region;

        if (region == 1)
            backgroundRawImage.texture = this.background1;
        else if (region == 2)
            backgroundRawImage.texture = this.background2;
        else
            backgroundRawImage.texture = this.background3;
    }
}
