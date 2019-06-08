using CombatManager;

using TileMaps;

using UnityEngine;

public class BeginBuild : MonoBehaviour
{
    public void Press()
    {
        if (SetupBattleField.State != SetupBattleField.BattleState.InBattle)
        {
            return;
        }

        GameObject.Find("Tilemap").GetComponent<TileController>().BeginBuild();
    }
}