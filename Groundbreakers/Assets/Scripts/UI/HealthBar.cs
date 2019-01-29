using System.Collections;
using System.Collections.Generic;
using Assets.Enemies.Scripts;
using Assets.Scripts.Enemies;

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public EnemyGeneric enemy;

    void Start()
    {
        this.healthBar = this.GetComponent<Image>();
        this.enemy = this.transform.parent.parent.parent.gameObject.GetComponent<EnemyGeneric>();
    }

    // Update is called once per frame
    void Update()
    {
        this.healthBar.fillAmount = (float)this.enemy.health / (float)this.enemy.maxHealth;
    }
}
