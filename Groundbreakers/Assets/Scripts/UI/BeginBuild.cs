using TileMaps;

using UnityEngine;

public class BeginBuild : MonoBehaviour
{
    public void Press()
    {
        GameObject.Find("Tilemap").GetComponent<TileController>().BeginBuild();
    }
}