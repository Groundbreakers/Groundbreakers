using System.Collections;
using System.Collections.Generic;
using Assets.Enemies.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Enemy_Generic enemy;

    void Start()
    {
        this.healthBar = this.GetComponent<Image>();
        this.enemy = this.transform.parent.parent.parent.gameObject.GetComponent<Enemy_Generic>();
    }

    // Update is called once per frame
    void Update()
    {
        this.healthBar.fillAmount = (float)this.enemy.health / (float)this.enemy.maxHealth;
    }
}
