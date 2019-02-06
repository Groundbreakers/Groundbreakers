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

        if (ui.activeSelf)
        {
            Time.timeScale = 0.0F;
        }
        else
        {
            Time.timeScale = 1.0F;
        }
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
            this.character1.SetActive(false);
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

    public void Close()
    {
        ui.SetActive(false);
        Time.timeScale = 1.0F;
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
