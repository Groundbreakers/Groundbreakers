using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deploy : MonoBehaviour
{
    public GameObject ui;

    private GameObject node;
    private GameObject[] character = new GameObject[5];
    private GameObject[] characterPos = new GameObject[5];
    private characterAttributes[] characterAttributes = new characterAttributes[5];
    private IEnumerator[] coroutine = new IEnumerator[5];

    void Start()
    {
        // Reference five characters
        GameObject characterList = GameObject.Find("CharacterList");
        this.character[0] = characterList.transform.GetChild(0).gameObject;
        this.character[1] = characterList.transform.GetChild(1).gameObject;
        this.character[2] = characterList.transform.GetChild(2).gameObject;
        this.character[3] = characterList.transform.GetChild(3).gameObject;
        this.character[4] = characterList.transform.GetChild(4).gameObject;
        this.characterAttributes[0] = this.character[0].GetComponent<characterAttributes>();
        this.characterAttributes[1] = this.character[1].GetComponent<characterAttributes>();
        this.characterAttributes[2] = this.character[2].GetComponent<characterAttributes>();
        this.characterAttributes[3] = this.character[3].GetComponent<characterAttributes>();
        this.characterAttributes[4] = this.character[4].GetComponent<characterAttributes>();
    }

    public void Toggle()
    {
        this.node = null;
        this.ui.SetActive(!this.ui.activeSelf);
    }

    public void DeployCharacter(int index)
    {
        // If the character is on the battlefield, retrieve it first
        if (this.character[index].activeSelf)
        {
            Debug.Log("The character has been deployed already");
            this.Retreat(index);
            //this.DeployCharacter(index);
        }
        else
        {
            // Put the character on the node and active it
            this.character[index].transform.position = this.node.transform.position;
            this.character[index].transform.rotation = this.node.transform.rotation;
            this.characterAttributes[index].disable();
            this.character[index].SetActive(true);
            GameObject temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            DeployBar bar = temp.GetComponent<DeployBar>();
            bar.Reset();
            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            this.coroutine[index] = this.Spawn(index, .0f / (float)this.characterAttributes[index].MOB * .5f);
            this.StartCoroutine(this.coroutine[index]);
        }
    }

    public IEnumerator Spawn(int index, float time)
    {
        Debug.Log("Deploying Character " + index + " ...");
        yield return new WaitForSeconds(time);
        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        this.characterAttributes[index].enabled();
        this.characterPos[index] = this.node;
        SelectNode selectNode = this.characterPos[index].GetComponent<SelectNode>();
        selectNode.characterOnTop = index;
    }

    public void Retreat(int index)
    {
        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
        GameObject temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        DeployBar bar = temp.GetComponent<DeployBar>();
        bar.Reset();
        this.characterAttributes[index].disable();
        this.coroutine[index] = this.Retreating(index, .0f / (float)this.characterAttributes[index].MOB * .5f);
        this.StartCoroutine(this.coroutine[index]);
    }

    private IEnumerator Retreating(int index, float time)
    {
        Debug.Log("Retreating Character " + index + " ...");
        yield return new WaitForSeconds(time);
        this.character[index].SetActive(false);
        SelectNode selectNode = this.characterPos[index].GetComponent<SelectNode>();
        selectNode.characterOnTop = 0;
    }

    public void Transform(int index)
    {
        this.character[index].GetComponent<characterAttack>().change();
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
