using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deploy : MonoBehaviour
{
    public GameObject ui;

    private GameObject node;
    private GameObject character1;
    private GameObject character2;
    private GameObject character3;
    private GameObject character4;
    private GameObject character5;
    private GameObject character1Pos;
    private GameObject character2Pos;
    private GameObject character3Pos;
    private GameObject character4Pos;
    private GameObject character5Pos;

    void Start()
    {
        // Reference five characters
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

    public void DeployC1()
    {
        // If character 1 is on the battlefield, retrieve it first
        if (this.character1.activeSelf)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Status status = canvas.GetComponent<Status>();
            status.SetNode(this.character1Pos);
            status.Retreat();
        }

        // Put the character on the node and active it
        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        selectNode.characterOnTop = 1;
        this.character1.transform.position = this.node.transform.position;
        this.character1.transform.rotation = this.node.transform.rotation;
        this.character1.SetActive(true);
        this.character1Pos = this.node;
        this.Close();
    }

    public void DeployC2()
    {
        if (this.character2.activeSelf)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Status status = canvas.GetComponent<Status>();
            status.SetNode(this.character2Pos);
            status.Retreat();
        }
        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        selectNode.characterOnTop = 2;
        this.character2.transform.position = this.node.transform.position;
        this.character2.transform.rotation = this.node.transform.rotation;
        this.character2.SetActive(true);
        this.character2Pos = this.node;
        this.Close();
    }

    public void DeployC3()
    {
        if (this.character3.activeSelf)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Status status = canvas.GetComponent<Status>();
            status.SetNode(this.character3Pos);
            status.Retreat();
        }
        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        selectNode.characterOnTop = 3;
        this.character3.transform.position = this.node.transform.position;
        this.character3.transform.rotation = this.node.transform.rotation;
        this.character3.SetActive(true);
        this.character3Pos = this.node;
        this.Close();
    }

    public void DeployC4()
    {
        if (this.character4.activeSelf)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Status status = canvas.GetComponent<Status>();
            status.SetNode(this.character4Pos);
            status.Retreat();
        }
        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        selectNode.characterOnTop = 4;
        this.character4.transform.position = this.node.transform.position;
        this.character4.transform.rotation = this.node.transform.rotation;
        this.character4.SetActive(true);
        this.character4Pos = this.node;
        this.Close();
    }

    public void DeployC5()
    {
        if (this.character5.activeSelf)
        {
            GameObject canvas = GameObject.Find("Canvas");
            Status status = canvas.GetComponent<Status>();
            status.SetNode(this.character5Pos);
            status.Retreat();
        }
        SelectNode selectNode = this.node.GetComponent<SelectNode>();
        selectNode.characterOnTop = 5;
        this.character5.transform.position = this.node.transform.position;
        this.character5.transform.rotation = this.node.transform.rotation;
        this.character5.SetActive(true);
        this.character5Pos = this.node;
        this.Close();
    }

    public void Close()
    {
        ui.SetActive(false);
        Time.timeScale = 1.0F;
    }
}
