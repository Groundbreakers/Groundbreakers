using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public GameObject ui;

    private GameObject node;

    private GameObject character1;

    private GameObject character2;

    private GameObject character3;

    private GameObject character4;

    private GameObject character5;

    private characterAttributes character;
    private DeployBar bar;
    void Start()
    {
        GameObject characterList = GameObject.Find("CharacterList");
        this.character1 = characterList.transform.GetChild(0).gameObject;
        this.character2 = characterList.transform.GetChild(1).gameObject;
        this.character3 = characterList.transform.GetChild(2).gameObject;
        this.character4 = characterList.transform.GetChild(3).gameObject;
        this.character5 = characterList.transform.GetChild(4).gameObject;
    }

    public void Toggle(GameObject selectedNode)
    {
        this.node = selectedNode;
        ui.SetActive(!ui.activeSelf);
    }

    public void Transform()
    {
        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        if (selectNode.characterOnTop == 0)
        {
            return;
        }
        else if (selectNode.characterOnTop == 1)
        {
            this.character1.GetComponent<characterAttack>().change();
        }
        else if (selectNode.characterOnTop == 2)
        {
            this.character2.GetComponent<characterAttack>().change();
        }
        else if (selectNode.characterOnTop == 3)
        {
            this.character3.GetComponent<characterAttack>().change();
        }
        else if (selectNode.characterOnTop == 4)
        {
            this.character4.GetComponent<characterAttack>().change();
        }
        else if (selectNode.characterOnTop == 5)
        {
            this.character5.GetComponent<characterAttack>().change();
        }
        this.Close();
    }

    public void Retreat()
    {
        

        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        if (selectNode.characterOnTop == 0)
        {
            return;
        }
        else if (selectNode.characterOnTop == 1)
        {
            
            this.character1.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            character = character1.GetComponent<characterAttributes>();
            GameObject temp = this.character1.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            bar = temp.GetComponent<DeployBar>();
            bar.Reset();
            character.disable();
            Invoke("RetreatC1", 2 / ((character.MOB * .5f)));
            selectNode.characterOnTop = 0;
        }
        else if (selectNode.characterOnTop == 2)
        {
            this.character2.SetActive(false);
            selectNode.characterOnTop = 0;
        }
        else if (selectNode.characterOnTop == 3)
        {
            this.character3.SetActive(false);
            selectNode.characterOnTop = 0;
        }
        else if (selectNode.characterOnTop == 4)
        {
            this.character4.SetActive(false);
            selectNode.characterOnTop = 0;
        }
        else if (selectNode.characterOnTop == 5)
        {
            this.character5.SetActive(false);
            selectNode.characterOnTop = 0;
        }
        this.Close();
    }

    public void RetreatC1()
    {
        this.character1.SetActive(false);
    }
    public void Close()
    {
        ui.SetActive(false);
    }

    public GameObject GetNode()
    {
        return this.node;
    }

    public void SetNode(GameObject newNode)
    {
        this.node = newNode;
    }
}
