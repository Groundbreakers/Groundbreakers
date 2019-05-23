using System.Collections;
using System.Collections.Generic;

using TileMaps;

using UnityEngine;

public class BeginSwap : MonoBehaviour
{
    public void Press()
    {
        GameObject.Find("Tilemap").GetComponent<TileController>().BeginSwap();
    }
}
