using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Image image;
    private int healthPoint = 20;
    public GameObject ui;
    public Sprite[] hpFrames;

    // Start is called before the first frame update
    void Start()
    {
        this.image.sprite = this.hpFrames[this.healthPoint];
    }

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
            GameObject bgmManager = GameObject.Find("BGM Manager");
            Manager manager = bgmManager.GetComponent<Manager>();
            manager.GameOver();
        }
    }
}
