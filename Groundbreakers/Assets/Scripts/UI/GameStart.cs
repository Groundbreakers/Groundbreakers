using System.Collections;
using System.Collections.Generic;

using Core;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public GameObject explorer1;
    public GameObject explorer2;
    public GameObject explorer3;
    public GameObject explorer4;
    public GameObject explorer5;

    public GameObject scholar;
    public GameObject trickster;
    public GameObject gladiator;
    public GameObject scavenger;
    public GameObject agent;

    public Image[] images;
    public Sprite[] icons;

    private GameObject characterList;

    void Awake()
    {
        this.characterList = GameObject.Find("Player Party");
    }

    public void InitializeCharacterList()
    {
        this.InitializeCharacter(explorer1, 0);
        this.InitializeCharacter(explorer2, 1);
        this.InitializeCharacter(explorer3, 2);
        this.InitializeCharacter(explorer4, 3);
        this.InitializeCharacter(explorer5, 4);

        // By Ivan: now the characters are spawned, we should Update Player Party's script here
        var pm = FindObjectOfType<PartyManager>();

        Assert.IsNotNull(pm);

        pm.LoadCharacters();
    }

    private void InitializeCharacter(GameObject explorer, int i)
    {
        switch (explorer.GetComponent<Explorer>().profession)
        {
            case 0:
                Instantiate(this.scholar, this.characterList.transform);
                this.images[i].sprite = this.icons[0];
                break;
            case 1:
                Instantiate(this.trickster, this.characterList.transform);
                this.images[i].sprite = this.icons[1];
                break;
            case 2:
                Instantiate(this.gladiator, this.characterList.transform);
                this.images[i].sprite = this.icons[2];
                break;
            case 3:
                Instantiate(this.scavenger, this.characterList.transform);
                this.images[i].sprite = this.icons[3];
                break;
        }
    }
}
