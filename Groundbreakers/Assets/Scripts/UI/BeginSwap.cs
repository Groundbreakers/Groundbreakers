using System.Collections;
using System.Collections.Generic;

using CombatManager;

using TileMaps;

using UnityEngine;

public class BeginSwap : MonoBehaviour
{
    public void Press()
    {
        if (SetupBattleField.State != SetupBattleField.BattleState.InBattle)
        {
            return;
        }

        GameObject.Find("Tilemap").GetComponent<TileController>().BeginSwap();
    }
}
