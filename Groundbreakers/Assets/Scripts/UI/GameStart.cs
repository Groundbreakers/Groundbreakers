using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private GameObject characterList;

    void Awake()
    {
        this.characterList = GameObject.Find("CharacterList");
    }

    public void InitializeCharacterList()
    {
        this.InitializeCharacter(explorer1);
        this.InitializeCharacter(explorer2);
        this.InitializeCharacter(explorer3);
        this.InitializeCharacter(explorer4);
        this.InitializeCharacter(explorer5);
    }

    private void InitializeCharacter(GameObject explorer)
    {
        switch (explorer.GetComponent<Explorer>().profession)
        {
            case 0:
                Instantiate(this.scholar, this.characterList.transform);
                break;
            case 1:
                Instantiate(this.trickster, this.characterList.transform);
                break;
            case 2:
                Instantiate(this.gladiator, this.characterList.transform);
                break;
            case 3:
                Instantiate(this.scavenger, this.characterList.transform);
                break;
        }
    }
}
