using System.Collections.Generic;

using CombatManager;

using UnityEngine;
using UnityEngine.UI;

public class UpdateGroundbreakerPanel : MonoBehaviour
{
    private List<Button> buttons = new List<Button>();

    private void OnEnable()
    {
        this.buttons.Clear();

        for (var i = 0; i < 4; i++)
        {
            this.buttons.Add(this.transform.GetChild(i).GetComponent<Button>());
        }
    }

    private void Update()
    {
        if (SetupBattleField.State != SetupBattleField.BattleState.InBattle)
        {
            foreach (var button in this.buttons)
            {
                button.interactable = false;
            }
        }
        else
        {
            foreach (var button in this.buttons)
            {
                button.interactable = true;
            }
        }
    }
}