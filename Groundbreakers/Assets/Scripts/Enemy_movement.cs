using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    public float speed = 10f;

    private int ArrayEndIndex;

    private int ArrayStartIndex = 0;

    private float DisToWaypoint = 0.01f;

    private Transform target;

    private int wavepoint_index = 0;

    public int health = 2;  // might move to another script for enemy attribute later



    void GetNextWaypoint() {
        this.ArrayEndIndex = Waypoints.points.Length - 1;

        if (this.wavepoint_index >= this.ArrayEndIndex)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.wavepoint_index++;
            this.target = Waypoints.points[this.wavepoint_index];
        }
    }

    void Start() {
        this.target = Waypoints.points[this.ArrayStartIndex];
        
    }

    void Update() {
        Vector2 direction = this.target.position - this.transform.position;
        this.transform.Translate(direction.normalized * this.speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(this.transform.position, this.target.position) <= this.DisToWaypoint)
        {
            this.GetNextWaypoint();
        }

        // might move to another script for enemy attribute later
        if (this.health <= 0)
        {
            Destroy(this.gameObject);
            Debug.Log("---------------------------------------------");
        }

    }


    
}
