using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Text mytext = null;
    public int healthPoint = 20;

    // Start is called before the first frame update
    void Start()
    {
        this.mytext.text = this.healthPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        this.mytext.text = this.healthPoint.ToString();
        if (this.healthPoint <= 0)
        {
            // Game Over
        }
    }
}
