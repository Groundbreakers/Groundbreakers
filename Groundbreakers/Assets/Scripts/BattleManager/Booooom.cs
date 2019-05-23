using System;
using System.Linq;

using Assets.Enemies.Scripts;

using UnityEngine;

public class Booooom : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float boooomRadius = 2.0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnDisable()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        var pos = this.transform.position;

        var targets = enemies.Where(
            e => Vector3.Distance(pos, e.transform.position) <= this.boooomRadius);

        foreach (var target in targets)
        {
            target.GetComponent<Enemy_Generic>().DamageEnemy(999, 999, 1.0f, false, false);
        }
    }
}