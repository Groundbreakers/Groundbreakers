using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cetus_Charge_Particle : MonoBehaviour
{
    public float acceleration = 0.2f;
    public float speed = 0.5f;
    public float timeAlive = 1f;
    private Transform thisParent;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        this.thisParent = this.transform.parent;
        this.dir = this.thisParent.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(this.dir.normalized * this.speed * Time.deltaTime, Space.World);
        this.speed += this.acceleration;
        // Reduce sprite and trail size by 5%
        this.gameObject.GetComponent<SpriteRenderer>().size = this.gameObject.GetComponent<SpriteRenderer>().size - (this.gameObject.GetComponent<SpriteRenderer>().size / 20);
        this.gameObject.GetComponent<TrailRenderer>().widthMultiplier = this.gameObject.GetComponent<TrailRenderer>().widthMultiplier - (this.gameObject.GetComponent<TrailRenderer>().widthMultiplier / 20);
        this.timeAlive -= Time.deltaTime;
        if (timeAlive <= 0 || Vector2.Distance(this.transform.position, this.thisParent.position) < 0.1)
        {
            Destroy(this.gameObject);
        }
    }
}
