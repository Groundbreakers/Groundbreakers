using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject ui;
    public GameObject[] attributes = new GameObject[4];

    //Basic Information
    public Image image;
    public Text name;
    public Text profession;

    //Abilities and Level Ups
    public Text ability1;
    public Text ability2;
    public Text levelup2;
    public Text levelup4;

    //Enabled or Disabled
    public Image level2;
    public Image level3;
    public Image level3ability;
    public Image level4;
    public Image level5;
    public Image level5ability;

    //Frame sprites
    public Sprite enabledSmall;
    public Sprite disabledSmall;
    public Sprite enabledMedium;
    public Sprite disabledMedium;

    private int characterIndex;

    private float rotateSpeed = 10.0F;

    void Start()
    {
        this.UpdatePanel();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Tooltip"))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Select(int index)
    {
        this.characterIndex = index;
        this.UpdatePanel();
    }

    public void UpdatePanel()
    {
        //return;

        // Show the attributes
        characterAttributes characterAttributes = GameObject.Find("Player Party").transform
            .GetChild(this.characterIndex).gameObject.GetComponent<characterAttributes>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                this.attributes[i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < characterAttributes.POW; i++)
        {
            this.attributes[0].transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < characterAttributes.ROF; i++)
        {
            this.attributes[1].transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < characterAttributes.RNG; i++)
        {
            this.attributes[2].transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < characterAttributes.AMP; i++)
        {
            this.attributes[3].transform.GetChild(i).gameObject.SetActive(true);
        }

        //Basic Info
        this.name.text = characterAttributes.name;
        this.profession.text = characterAttributes.profession;

        if (this.profession.text == "Scholar")
        {
            this.UpdateScholar();
        }
        else if (this.profession.text == "Trickster")
        {
            this.UpdateTrickster();
        }
        else if (this.profession.text == "Gladiator")
        {
            this.UpdateGladiator();
        }
        else if (this.profession.text == "Scavenger")
        {
            this.UpdateScavenger();
        }
        else
        {
            this.UpdateAgent();
        }
    }

    public int GetCharacterIndex()
    {
        return this.characterIndex;
    }

    public void Open()
    {
        this.UpdatePanel();
        this.ui.SetActive(true);
    }

    public void Close()
    {
        this.ui.SetActive(false);
    }

    public void UpdateScholar()
    {
        this.levelup2.text = "AMP+1 RNG+1";
        this.levelup4.text = "POW+1 RNG+1";
        this.ability1.text = "Ability 1: Slow targeting enemy";
        this.ability2.text = "Ability 2: Laser Reflection";
    }

    public void UpdateTrickster()
    {
        this.levelup2.text = "ROF+1 AMP+1";
        this.levelup4.text = "POW+1 RNG+1";
        this.ability1.text = "Ability 1: Transform Whirlwind";
        this.ability2.text = "Ability 2: Slow all nearby enemies";
    }

    public void UpdateGladiator()
    {
        this.levelup2.text = "POW+1 AMP+1";
        this.levelup4.text = "ROF+1 AMP+1";
        this.ability1.text = "Ability 1: Attacks stop enemies for 0.5 second";
        this.ability2.text = "Ability 2: Melee Splash";
    }

    public void UpdateScavenger()
    {
        this.levelup2.text = "POW+1 AMP+1";
        this.levelup4.text = "ROF+1 RNG+1";
        this.ability1.text = "Ability 1: Transform Multi-shot";
        this.ability2.text = "Ability 2: Attacks burn enemies for 1 second";
    }

    public void UpdateAgent()
    {
        this.levelup2.text = "ROF+1 RNG+1";
        this.levelup4.text = "POW+1 ROF+1";
        this.ability1.text = "Ability 1: True Strike";
        this.ability2.text = "Ability 2: Bullets Pierce";
    }
}
