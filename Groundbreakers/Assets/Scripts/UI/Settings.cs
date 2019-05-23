﻿using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject ui;
    public float timeScale;

    public void Toggle ()
    {
        ui.SetActive(!ui.activeSelf);
    }

    public void NewGame()
    {
        // Get a new BGM
        GameObject bgm = GameObject.Find("BGM Manager");
        Manager manager = bgm.GetComponent<Manager>();
        manager.UpdateBGM();

        // Gnenerate Sector Map here
        GameObject.Find("SectorMap").GetComponent<SectorGenerator>().Initialize();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        BattleManager.TriggerEvent("reset");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void TimeScale1x()
    {
        this.timeScale = 1.0F;
    }

    public void TimeScale2x()
    {
        this.timeScale = 2.0F;
    }

    public void TimeScale4x()
    {
        this.timeScale = 4.0F;
    }
}
