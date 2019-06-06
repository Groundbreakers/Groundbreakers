using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public Sprite UnpressedSprite;
    public Sprite PressedSprite;
    public bool Pressed;

    public void Press()
    {
        var sprite = this.GetComponent<Image>().sprite;
        this.GetComponent<Image>().sprite = (sprite == this.UnpressedSprite) ? this.PressedSprite : this.UnpressedSprite;

        if (this.GetComponent<Image>().sprite == this.PressedSprite)
        {
            this.Pressed = true;
            Debug.Log(Pressed);
        }
        else
        {
            this.Pressed = false;
            Debug.Log(Pressed);
        }

        Debug.Log(sprite);
    }

    public void Unpress()
    {
        this.GetComponent<Image>().sprite = this.UnpressedSprite;
    }
}
