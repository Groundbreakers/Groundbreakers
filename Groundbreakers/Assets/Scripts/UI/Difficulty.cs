using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public Text firstLine;
    public Text secondLine;
    public int Risk = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.firstLine.text = "Risk";
        this.secondLine.text = "" + Risk;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
