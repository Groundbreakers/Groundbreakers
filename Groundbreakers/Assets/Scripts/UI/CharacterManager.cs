using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject selectCircle;
    public GameObject ui;
    public GameObject[] icons = new GameObject[5];
    public GameObject[] modules = new GameObject[5];
    public GameObject[] inventory = new GameObject[5];
    public GameObject[] attributes = new GameObject[5];
    public Sprite[] bars = new Sprite[11];

    public Sprite frame;
    public GameObject moduleButton;
    private int characterIndex;

    private float rotateSpeed = 10.0F;

    void Start()
    {
        this.UpdatePanel();
    }

    // Update is called once per frame
    void Update()
    {
        this.selectCircle.transform.Rotate(0.0F, 0.0F, -Time.deltaTime * this.rotateSpeed);
    }

    public void Select(int index)
    {
        this.characterIndex = index;
        this.selectCircle.transform.position = this.icons[index].transform.position;
        this.UpdatePanel();
    }

    public void UpdatePanel()
    {
        // Show the attributes
        characterAttributes characterAttributes = GameObject.Find("CharacterList").transform
            .GetChild(this.characterIndex).gameObject.GetComponent<characterAttributes>();
        this.attributes[0].GetComponent<Image>().sprite = this.bars[characterAttributes.POW];
        this.attributes[1].GetComponent<Image>().sprite = this.bars[characterAttributes.ROF];
        this.attributes[2].GetComponent<Image>().sprite = this.bars[characterAttributes.RNG];
        this.attributes[3].GetComponent<Image>().sprite = this.bars[characterAttributes.MOB];
        this.attributes[4].GetComponent<Image>().sprite = this.bars[characterAttributes.AMP];

        // Show the equipped modules
        for (int i = 0; i < 5; i++)
        {
            if (this.inventory[this.characterIndex].transform.GetChild(i).childCount != 0)
            {
                this.modules[i].GetComponent<Image>().sprite = this.inventory[this.characterIndex].transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite;
                this.modules[i].GetComponent<Button>().onClick = this.inventory[this.characterIndex].transform.GetChild(i).GetChild(0).gameObject.GetComponent<Button>().onClick;
            }
            else
            {
                this.modules[i].GetComponent<Image>().sprite = this.frame;
                this.modules[i].GetComponent<Button>().onClick = this.moduleButton.GetComponent<Button>().onClick;
            }
        }
    }

    public int GetCharacterIndex()
    {
        return this.characterIndex;
    }

    public void Toggle()
    {
        this.UpdatePanel();
        this.ui.SetActive(!this.ui.activeSelf);
    }
}
