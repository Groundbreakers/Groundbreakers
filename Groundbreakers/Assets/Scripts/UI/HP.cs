using System;

using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Sprite[] hpFrames;

    public Image image;

    public GameObject ui;

    public int healthPoint = 20;

    public void UpdateHealth(int amount)
    {
        this.healthPoint += amount;
        if (this.healthPoint < 0)
        {
            this.healthPoint = 0;
        }
        else if (this.healthPoint > 20)
        {
            this.healthPoint = 20;
        }

        this.image.sprite = this.hpFrames[this.healthPoint];
        if (this.healthPoint <= 0)
        {
            // Game Over
            this.ui.SetActive(true);
            Time.timeScale = 0.0F;

            // Switch the bgm
            var bgmManager = GameObject.Find("BGM Manager");
            var manager = bgmManager.GetComponent<Manager>();
            manager.GameOver();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.image.sprite = this.hpFrames[this.healthPoint];
    }

    private void Update()
    {
        // For debug only - developer cheat
        if (Input.GetKeyDown("p"))
        {
            this.UpdateHealth(20);
        }
    }
}