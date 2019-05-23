using System.Collections;
using System.Collections.Generic;

using TileMaps;

using UnityEngine;

public class BeginBuild : MonoBehaviour
{
    public void Press()
    {
        GameObject.Find("Tilemap").GetComponent<TileController>().BeginBuild();
    }
}
