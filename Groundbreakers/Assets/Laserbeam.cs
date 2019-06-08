using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;


public class Laserbeam : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public LineRenderer lineRenderer2;

    public LineRenderer lineRenderer3;
    
    private float delay = 0f;

    void Awake()
    {
     this.lineRenderer.enabled = false;
    }


    public void LaserAttack(Transform target)
    {
        
        var offset = new Vector3(0.0f, 0.3f, 0.0f);
        
        this.lineRenderer.SetPosition(0, this.gameObject.transform.position);

        this.lineRenderer.SetPosition(1, target.position);

        if (delay <= 0f)
        {
            GameObject.Find("RangedWeapon").GetComponent<DamageHandler>().DeliverDamageTo(target.gameObject, false);
            delay = 0.3f;
        }
        delay -= Time.deltaTime;
    }

    public void LaserRelection(Transform target)
    {
        var offset = new Vector3(0.0f, 0.3f, 0.0f);
        Vector2[] positions = new Vector2[3];

        this.lineRenderer.numPositions = positions.Length;

        positions[0] = this.gameObject.transform.position + offset;
        positions[1] = target.position;

        if (target.GetComponent<laserReflectionTarget>().target != null)
        {
            positions[2] = target.gameObject.GetComponent<laserReflectionTarget>().target.position;
            for (int i = 0; i < 3; i++)
            {
                this.lineRenderer.SetPosition(i, positions[i]);
            }
        }
        else
        {
            positions[0] = this.gameObject.transform.position +  offset;
            positions[1] = target.position;
            this.lineRenderer.numPositions = positions.Length - 1;
            for (int i = 0; i < 2; i++)
            {
              this.lineRenderer.SetPosition(i, positions[i]);
            }
        }
        
        if (delay <= 0f)
        {
            GameObject.Find("RangedWeapon").GetComponent<DamageHandler>().DeliverDamageTo(target.gameObject, false);

            if (target.GetComponent<laserReflectionTarget>().target != null)
            {
                GameObject.Find("RangedWeapon").GetComponent<DamageHandler>().DeliverDamageTo(target.GetComponent<laserReflectionTarget>().target.gameObject, false);
            }

            delay = 0.3f;
        }
        delay -= Time.deltaTime;
    }


}
