using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public GameObject ui;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0.0F;
        }
        else
        {
            Time.timeScale = 1.0F;
        }
    }

    public void Transform()
    {

    }

    public void Retreat()
    {

    }
}
