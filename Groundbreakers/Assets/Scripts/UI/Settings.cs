using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject ui;
    public Button button;

    public void Toggle ()
    {
        ui.SetActive(!ui.activeSelf);
        if (this.button.gameObject.activeSelf)
        {
            this.button.interactable = !ui.activeSelf;
        }

        if (ui.activeSelf)
        {
            Time.timeScale = 0.0F;
        }
        else
        {
            Time.timeScale = 1.0F;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit ()
    {
        Application.Quit();
    }
}
