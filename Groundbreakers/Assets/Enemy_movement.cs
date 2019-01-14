using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private int wavepoint_index = 0;
    private int ArrayStartIndex = 0;
    private int ArrayEndIndex;
    private const float DISTOWAYPOINT = 0.01f;

    void Start()
    {
        target = Waypoints.points[ArrayStartIndex];
    }

    void Update()
    {
        Vector2 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, target.position) <= DISTOWAYPOINT) {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint() {
        ArrayEndIndex = Waypoints.points.Length - 1;

        if (wavepoint_index >= ArrayEndIndex)
        {
            Destroy(gameObject);
        }
        else
        {
            wavepoint_index++;
            target = Waypoints.points[wavepoint_index];
        }
    }

}
