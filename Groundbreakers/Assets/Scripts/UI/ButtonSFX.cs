using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void Clicked()
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("MenuClick");
    }
}
