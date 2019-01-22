using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Text mytext = null;
    public static int healthPoint = 20;

    // Start is called before the first frame update
    void Start()
    {
        mytext.text = healthPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            healthPoint--;
            mytext.text = healthPoint.ToString();
        }
    }
}
