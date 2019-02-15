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
    }

    public void NewGame()
    {
        RoutesGenerator routes = this.GetComponent<RoutesGenerator>();
        routes.Toggle();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void TimeScale1x()
    {
        Time.timeScale = 1.0F;
    }

    public void TimeScale2x()
    {
        Time.timeScale = 2.0F;
    }

    public void TimeScale4x()
    {
        Time.timeScale = 4.0F;
    }
}
