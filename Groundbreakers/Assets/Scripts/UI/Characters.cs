using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public GameObject selectCircle;

    private float rotateSpeed = 10.0F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.selectCircle.transform.Rotate(0.0F, 0.0F, Time.deltaTime * this.rotateSpeed);
    }
}
