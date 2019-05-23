using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public Sprite UnpressedSprite;
    public Sprite PressedSprite;

    public void Press()
    {
        var sprite = this.GetComponent<Image>().sprite;
        this.GetComponent<Image>().sprite = (sprite == this.UnpressedSprite) ? this.PressedSprite : this.UnpressedSprite;
        Debug.Log(sprite);
    }

    public void Unpress()
    {
        this.GetComponent<Image>().sprite = this.UnpressedSprite;
    }
}
