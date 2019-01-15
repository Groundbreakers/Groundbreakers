using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeattack : MonoBehaviour
{

    private Transform target;
    public float speed = 10f;

    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distancePerFrame) {
            hitTarget();
            return;
        }

        transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }


    public void chase(Transform _target) {
        target = _target;
    }

    void hitTarget() {
        Destroy(gameObject);
    }
}
