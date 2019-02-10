using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject ui;
    public Text wave;
    public Text timer;

    private float countdown;
    private float waveDelay;
    private int waveCount;
    private Boolean isBattle;

    void Start()
    {
        this.isBattle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            this.Toggle();
        }

        if (this.isBattle)
        {
            if (this.countdown <= 0F)
            {
                NextWave(waveCount);
                this.waveCount += 1;
                this.wave.text = "WAVE " + this.waveCount + "/5";
                this.countdown = this.waveDelay;
            }

            this.countdown -= Time.deltaTime;
            this.timer.text = Mathf.Round(this.countdown).ToString();
        }
    }

    public void Initialize()
    {
        this.isBattle = true;
        this.countdown = 10.0F;
        this.waveDelay = 30.0F;
        this.waveCount = 0;
        this.wave.text = "BEGINS IN";
        this.timer.text = this.countdown.ToString();
    }

    public void NextWave(int count)
    {
        GameObject battleManager = GameObject.Find("BattleManager");
        var waveSpawner = battleManager.GetComponent<MobSpawner>();

        waveSpawner.StartCoroutine(waveSpawner.SpawnWave(count));
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        if (!this.isBattle)
        {
            this.Initialize();
        }
        else
        {
            this.isBattle = false;
        }
    }
}
