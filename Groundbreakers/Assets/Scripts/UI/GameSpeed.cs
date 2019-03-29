using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    public GameObject ui;

    public void Toggle()
    {
        this.ui.SetActive(!this.ui.activeSelf);
    }
}
