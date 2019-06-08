using System.Collections.Generic;

using CombatManager;

using Sirenix.OdinInspector;

using TileMaps;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UpdateGroundbreakerPanel : MonoBehaviour
{
    private List<Button> buttons = new List<Button>();

    private DynamicTerrainController dtc;

    [Required]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private List<float> costs = new List<float>();

    private void OnEnable()
    {
        this.dtc = FindObjectOfType<DynamicTerrainController>();

        Assert.IsNotNull(this.dtc);

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
            for (var i = 0; i < 4; i++)
            {
                var cost = this.costs[i];
                var button = this.buttons[i];

                button.interactable = cost < this.dtc.GetRiskLevel();
            }
        }
    }
}