using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

using UnityEngine;
using UnityEngine.UI;

using Difficulty = Asset.Script.EnemyGroups.Difficulty;

public class CurrentLevel : MonoBehaviour
{
    #region Inspector Properties

    public GameObject titleScreen;

    public Text ui;

    public Text ui2;

    #endregion

    #region Internal Fields

    private int level;

    private int region;

    private Dictionary<int, Difficulty> levelDifficultyMap =
        new Dictionary<int, Difficulty>
            {
                { 1, Difficulty.Easy },
                { 2, Difficulty.Easy },
                { 3, Difficulty.Easy },
                { 4, Difficulty.Medium },
                { 5, Difficulty.Medium },
                { 6, Difficulty.Hard },
                { 7, Difficulty.Hard },
                { 8, Difficulty.Hard },
            };

    #endregion

    #region Public Functions

    public void ChangeRegion()
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

    public int GetLevel()
    {
        return this.level;
    }

    public int GetRegion()
    {
        return this.region;
    }

    /// <summary>
    /// Called by BattleManager (or directly called by spawn) I think
    /// </summary>
    /// <returns>
    /// The <see cref="Difficulty"/>.
    /// </returns>
    public Difficulty GetDifficulty()
    {
        return this.level == 0 ? Difficulty.Easy : this.levelDifficultyMap[this.level];
    }

    public void UpdateLevel()
    {
        if (this.level == 8)
        {
            this.region += 1;
            this.level = 1;
            this.ChangeRegion();
        }
        else
        {
            this.level += 1;
        }

        this.ui.text = "Region " + this.region;
        this.ui2.text = "Level " + this.level + "/8";
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        this.region = 1;
        this.level = 1;
        this.ui.text = "Region " + this.region;
        this.ui2.text = "Level " + this.level + "/8";
        this.ChangeRegion();
    }

    #endregion
}