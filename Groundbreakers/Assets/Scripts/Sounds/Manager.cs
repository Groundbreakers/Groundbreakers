using System.Collections;
using System.Collections.Generic;

using Assets.Scripts;

using UnityEngine;

using static Assets.Scripts.BattleManager;

public class Manager : MonoBehaviour
{
    private AudioSource peaceTheme;
    private AudioSource battleTheme;
    private float speed = 0.01F;
    public int region;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        peaceTheme = audioSources[0];
        battleTheme = audioSources[0];
        peaceTheme.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Fade in & out effects
        if (BattleManager.GameState != Stages.Null)
        {
            if (peaceTheme.volume != 0.0F)
            {
                peaceTheme.volume -= speed;
                battleTheme.volume += speed;
            }
        }
        else
        {
            if (battleTheme.volume != 0.0F)
            {
                battleTheme.volume -= speed;
                peaceTheme.volume += speed;
            }
        }
    }

    public void UpdateBGM()
    {
        if (this.peaceTheme != null)
        {
            peaceTheme.Stop();
        }

        if (this.battleTheme != null)
        {
            battleTheme.Stop();
        }

        // Get to know what is the current region
        GameObject canvas = GameObject.Find("Canvas");
        CurrentLevel currentLevel = canvas.GetComponent<CurrentLevel>();
        this.region = currentLevel.GetRegion();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (region == 1)
        {
            peaceTheme = audioSources[1];
            battleTheme = audioSources[2];
        }
        else if (region == 2)
        {
            peaceTheme = audioSources[3];
            battleTheme = audioSources[4];
        }
        else if (region == 3)
        {
            peaceTheme = audioSources[5];
            battleTheme = audioSources[6];
        }
        peaceTheme.Play();
        battleTheme.Play();
    }

    public void GameOver()
    {
        if (this.peaceTheme != null && this.battleTheme != null)
        {
            peaceTheme.Stop();
            battleTheme.Stop();
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();
        battleTheme = audioSources[7];
        battleTheme.Play();
    }
}
