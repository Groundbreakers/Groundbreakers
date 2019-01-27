using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject ui;
    public Text wave;
    public Text timer;
    public float countdown;
    public float waveDelay;
    public int waveCount;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            this.Initialize();
            this.Toggle();
        }
        if (this.countdown <= 0F)
        {
            this.waveCount += 1;
            this.wave.text = "WAVE " + this.waveCount;
            this.countdown = this.waveDelay;
            NextWave(waveCount);
        }
        this.countdown -= Time.deltaTime;
        this.timer.text = Mathf.Round(this.countdown).ToString();
    }

    public void Initialize()
    {
        this.countdown = 10.0F;
        this.waveDelay = 30.0F;
        this.waveCount = 0;
        this.wave.text = "BEGINS IN";
        this.timer.text = this.countdown.ToString();
    }

    public void NextWave(int count)
    {

    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
    }
}
