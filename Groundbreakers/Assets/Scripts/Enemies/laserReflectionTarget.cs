using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserReflectionTarget : MonoBehaviour
{
    private List<GameObject> targetedEnemies;

    public Transform target;

    private CircleCollider2D myCollider;

    void Awake()
    {
        targetedEnemies = new List<GameObject>();
        myCollider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            targetedEnemies.Add(other.gameObject);
            updateTarget();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            targetedEnemies.Remove(other.gameObject);
            updateTarget();
        }
    }

    void updateTarget()
    {
        if (targetedEnemies.Count != 0)
        {
            target = targetedEnemies[0].transform;
        }
        else
        {
            target = null;
            Debug.Log("null");
        }

    }
}
