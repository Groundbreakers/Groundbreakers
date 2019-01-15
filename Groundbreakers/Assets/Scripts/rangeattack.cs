using UnityEngine;

public class rangeattack : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;

    public void chase(Transform _target) {
        this.target = _target;
    }

    void hitTarget() {
        Destroy(this.gameObject);
    }

    void Update() {
        if (this.target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 direction = this.target.position - this.transform.position;
        float distancePerFrame = this.speed * Time.deltaTime;

        if (direction.magnitude <= distancePerFrame)
        {
            this.hitTarget();
        }

        this.transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }
}
