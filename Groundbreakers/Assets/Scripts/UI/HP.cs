using System;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Sprite[] hpFrames;

    public Image image;

    public GameObject ui;

    public int healthPoint = 20;

    public Text text;

    public void UpdateHealth(int amount)
    {
        // some effects
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("PlayerHpLoss");

        Camera.main.DOShakePosition(1.0f, amount / 4.0f);

        var sprite = GameObject.Find("RedScreen").GetComponent<SpriteRenderer>();

        var sequence = DOTween.Sequence();

        var value = Mathf.Clamp(amount / 4.0f, 0.0f, 1.0f);
        sequence.Append(sprite.DOFade(value, 0.5f).SetEase(Ease.OutBounce));
        sequence.Append(sprite.DOFade(0.0f, 0.5f).SetEase(Ease.OutBounce));

        // Blow are the old code
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
        this.text.text = this.healthPoint.ToString();

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