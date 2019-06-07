using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public Text firstLine;
    public Text secondLine;

    // Start is called before the first frame update
    void Start()
    {
        this.firstLine.text = "Risk";
        this.secondLine.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
