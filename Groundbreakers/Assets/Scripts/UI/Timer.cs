﻿using System;
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
    public float countdown;
    public float waveDelay;
    public int waveCount;
    public Boolean isBattle;

    public void OnEnable()
    {
        BattleManager.StartListening("block ready", this.Toggle);
    }

    void Start()
    {
        this.isBattle = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    this.Toggle();
        //}
        
        //if (this.isBattle)
        if (BattleManager.GameState == BattleManager.Stages.Combating)
        {
            if (this.countdown <= 0F)
            {
                //NextWave(waveCount);
                this.waveCount += 1;
                this.wave.text = "WAVE " + this.waveCount;
                this.countdown = this.waveDelay;
            }

            this.countdown -= Time.deltaTime;
            this.timer.text = Mathf.Round(this.countdown).ToString();
        }
    }

    public void UpdateWave(int wave)
    {
        this.waveCount = wave;
        this.wave.text = "WAVE " + this.waveCount;
        this.countdown = this.waveDelay;
        this.timer.text = Mathf.Round(this.countdown).ToString();
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
        //GameObject battleManager = GameObject.Find("BattleManager");
        //WaveSpawner waveSpawner = battleManager.GetComponent<WaveSpawner>();
        //waveSpawner.StartCoroutine(waveSpawner.SpawnWave(count));
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        if (BattleManager.GameState != BattleManager.Stages.Combating)
        {
            this.Initialize();
        }
        else
        {
            this.isBattle = false;
        }
    }
}
