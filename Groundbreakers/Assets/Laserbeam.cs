using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;


public class Laserbeam : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private float delay = 0f;

    void Awake()
    {
     this.lineRenderer.enabled = false;
    }


    public void LaserAttack(Transform target)
    {
        
        var offset = new Vector3(0.0f, 0.5f, 0.0f);
        
        this.lineRenderer.SetPosition(0, this.gameObject.transform.position);

        this.lineRenderer.SetPosition(1, target.position);
        
        if (delay <= 0f)
        {
            GameObject.Find("RangedWeapon").GetComponent<DamageHandler>().DeliverDamageTo(target.gameObject, false);
            delay = 0.3f;
        }
        delay -= Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
