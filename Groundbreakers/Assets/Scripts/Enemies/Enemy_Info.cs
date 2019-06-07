using System.Collections;
using System.Collections.Generic;
using Assets.Enemies.Scripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script creates an enemy information box when an enemy is seleted by the cursor and destroy it when cursor exit.
/// </summary>

public class Enemy_Info : MonoBehaviour
{
    public GameObject enemyInfo_prefab;

    private Text specie;

    private Canvas canvas;

    private GameObject box;

    private Enemy_Generic enemygeneric;
    

    void Awake()
    {
        //canvas = GameObject.Find("EnemyInfo_Canvas").GetComponent<Canvas>();
        //enemygeneric = this.GetComponent<Enemy_Generic>();
    }
    
    //public void createEnemyInfo()
    //{
    //    box = (GameObject) Instantiate(this.enemyInfo_prefab);
    //    box.transform.SetParent(this.canvas.gameObject.transform, false);
    //    this.box.transform.position = this.gameObject.transform.position + new Vector3(1.5f,1.5f);
    //    box.GetComponent<EnemyInfoBox>().enemySpecie.text = "Specie: " + this.enemygeneric.specie;
    //    box.GetComponent<EnemyInfoBox>().enemyHealth.text = "Health: " + this.enemygeneric.health;
    //    box.GetComponent<EnemyInfoBox>().enemySpeed.text = "Speed: " + this.enemygeneric.speed;
    //    box.GetComponent<EnemyInfoBox>().enemyPower.text = "Power: " + this.enemygeneric.power;
    //    box.GetComponent<EnemyInfoBox>().enemyEvasion.text = "Evasion: " + this.enemygeneric.evasion;
    //    box.GetComponent<EnemyInfoBox>().enemyRegan.text = "Regen: " + this.enemygeneric.regen;
    //}

    //void OnMouseEnter()
    //{
    //    createEnemyInfo();
    //    this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    //}

    //void OnMouseExit()
    //{
    //    Destroy(box);
    //    this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    //}
}
