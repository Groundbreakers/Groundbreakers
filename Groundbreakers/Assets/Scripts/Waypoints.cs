using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void Awake() {
        points = new Transform[this.transform.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = this.transform.GetChild(i);
        }
    }
}
