using TileMaps;

using UnityEngine;

public class BeginBooom : MonoBehaviour
{
    public void Press()
    {
        GameObject.Find("Tilemap").GetComponent<TileController>().BeginBooom();
    }
}