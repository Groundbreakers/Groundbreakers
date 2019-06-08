using System;
using System.Linq;

using CombatManager;

using Sirenix.OdinInspector;

using TileMaps;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ClickPortrait : MonoBehaviour
{
    [Required]
    [SerializeField]
    private GameObject charactersPanel;

    [Required]
    [SerializeField]
    private int cost;

    private CharacterManager manager;

    public void Press(int index)
    {
        Assert.IsTrue(Enumerable.Range(0, 5).Contains(index));

        if (SetupBattleField.State == SetupBattleField.BattleState.NotInBattle)
        {
            // In peace mode. 
            this.manager.Select(index);
            this.manager.Open();
        }
        else if (SetupBattleField.State == SetupBattleField.BattleState.InBattle)
        {
            // In battle mode. 
            FindObjectOfType<TileController>().BeginDeploying(index);
        }
    }

    private void OnEnable()
    {
        this.manager = this.charactersPanel.GetComponent<CharacterManager>();

        Assert.IsNotNull(this.manager);
    }
}