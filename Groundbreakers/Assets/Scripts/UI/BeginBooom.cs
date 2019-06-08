using CombatManager;

using TileMaps;

using UnityEngine;

public class BeginBooom : MonoBehaviour
{
    public void Press()
    {
        if (SetupBattleField.State != SetupBattleField.BattleState.InBattle)
        {
            return;
        }

        GameObject.Find("Tilemap").GetComponent<TileController>().BeginBooom();
    }
}