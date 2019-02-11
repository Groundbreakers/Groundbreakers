using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;

using static Assets.Scripts.BattleManager;

public class Manager : MonoBehaviour
{
    private AudioSource peaceTheme;
    private AudioSource battleTheme;
    //private bool isBattle;
    private float speed = 0.01F;
    public int region;

    // Update is called once per frame
    void Update()
    {
        // Press Spacebar to switch between themes
        //if (Input.GetKeyDown("space"))
        //{
        //    if (isBattle)
        //        isBattle = false;
        //    else
        //        isBattle = true;
        //}

        // Fade in & out effects
        if (BattleManager.GameState != Stages.Null)
        {
            peaceTheme.volume -= speed;
            battleTheme.volume += speed;
        }
        else
        {
            battleTheme.volume -= speed;
            peaceTheme.volume += speed;
        }
    }

    public void UpdateBGM()
    {
        //isBattle = false;
        if (this.peaceTheme != null && this.battleTheme != null)
        {
            peaceTheme.Stop();
            battleTheme.Stop();
        }

        // Get to know what is the current region
        GameObject canvas = GameObject.Find("Canvas");
        CurrentLevel currentLevel = canvas.GetComponent<CurrentLevel>();
        this.region = currentLevel.GetRegion();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (region == 1)
        {
            peaceTheme = audioSources[0];
            battleTheme = audioSources[1];
        }
        else if (region == 2)
        {
            peaceTheme = audioSources[2];
            battleTheme = audioSources[3];
        }
        else if (region == 3)
        {
            peaceTheme = audioSources[4];
            battleTheme = audioSources[5];
        }
        peaceTheme.Play();
        battleTheme.Play();
        //isBattle = false;
    }
}
