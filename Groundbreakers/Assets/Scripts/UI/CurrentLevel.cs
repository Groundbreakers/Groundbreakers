using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.UI;


public class CurrentLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreen;

    [SerializeField]
    private Text ui;

    [SerializeField]
    private Text ui2;

    /// <summary>
    /// Keep a reference to the manager class.
    /// </summary>
    private LevelManager.LevelManager levelManager;

    public void OnRegionChanged()
    {
        // Get a new BGM if the region is changed
        var bgm = GameObject.Find("BGM Manager");
        var manager = bgm.GetComponent<Manager>();
        if (!this.titleScreen.activeSelf)
        {
            manager.UpdateBGM();
        }

        // Get a new background image if the region is changed
        var canvas = GameObject.Find("Canvas");
        var background = canvas.GetComponent<Background>();
        background.UpdateBackground();
    }

    public void UpdateLevelInfo()
    {
        this.ui.text = "Region " + this.levelManager.Region;
        this.ui2.text = "Level " + this.levelManager.Level + "/8";
    }

    #region Unity Callbacks

    private void OnEnable()
    {
        this.levelManager = FindObjectOfType<LevelManager.LevelManager>();

        if (!this.levelManager)
        {
            Debug.LogError("There needs to be one active LevelManager script on a GameObject in your scene.");
        }

        this.UpdateLevelInfo();
        this.OnRegionChanged();
    }

    #endregion
}