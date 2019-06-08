using CombatManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Sector : MonoBehaviour
{
    public GameObject PathL;
    public GameObject PathM;
    public GameObject PathR;
    public GameObject Button;
    public string Type;
    public int Depth;
    public int Row;
    public int Column;

    public void Setup(int row, int column, string type, int depth)
    {
        this.Row = row;
        this.Column = column;
        this.Type = type;
        this.Depth = depth;
        this.Button.GetComponent<Button>().onClick.AddListener(this.HandleButton);
    }

    public void HandleButton()
    {
        // Should disable this if the player is currently in battle. ----- by Ivan
        var battleField = FindObjectOfType<SetupBattleField>();

        Assert.IsNotNull(battleField);

        if (SetupBattleField.State != SetupBattleField.BattleState.NotInBattle)
        {
            return;
        }

        battleField.Setup(Depth, FindObjectOfType<Difficulty>().Risk);

        GameObject.Find("SectorMap").GetComponent<SectorGenerator>().ShowPath(this.Row, this.Column);
    }

    public void ShowButton()
    {
        this.Button.SetActive(true);
    }

    public void ShowPath(Char path)
    {
        this.Button.SetActive(true);
        if (path == 'L')
        {
            this.PathL.SetActive(true);
        }
        if (path == 'M')
        {
            this.PathM.SetActive(true);
        }
        if (path == 'R')
        {
            this.PathR.SetActive(true);
        }
    }
}
