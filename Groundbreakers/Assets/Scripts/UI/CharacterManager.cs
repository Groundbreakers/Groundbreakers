using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject ui;
    public GameObject[] slots = new GameObject[5];
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
                if (this.slots[i].transform.childCount == 0)
                {
                    GameObject.Instantiate(
                        this.inventory[this.characterIndex].transform.GetChild(i).GetChild(0),
                        this.slots[i].transform);
                    this.slots[i].transform.GetChild(0).GetComponent<CanvasGroup>().blocksRaycasts = true;
                    this.slots[i].transform.GetChild(0).GetComponent<ModuleGeneric>().isDragable = false;
                }
            }
            else
            {
                if (this.slots[i].transform.childCount != 0)
                {
                    Destroy(this.slots[i].transform.GetChild(0).gameObject);
                }
            }
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
}
