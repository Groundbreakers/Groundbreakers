using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.UI;

public class Deploy : MonoBehaviour
{
    public GameObject ui;

    private GameObject node;
    private GameObject[] character = new GameObject[5];
    private GameObject[] characterPos = new GameObject[5];
    private characterAttributes[] characterAttributes = new characterAttributes[5];
    private IEnumerator[] coroutine = new IEnumerator[5];
    private Boolean[] running = new Boolean[5];

    public GameObject[] characterDeployButton;
    public GameObject[] characterRetreatButton;

    void Start()
    {
        // Reference five characters and their attributes
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
        // Reference the Deploy meter
        GameObject temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        DeployBar bar = temp.GetComponent<DeployBar>();

        // Remember the character's position
        this.characterPos[index] = this.node;
        SelectNode selectNode = this.characterPos[index].GetComponent<SelectNode>();

        // If the Coroutine is still running
        if (this.running[index])
        {
            // Click on the same button again to cancel the deployment
            Debug.Log("The character deployment has been cancelled");
            this.StopCoroutine(this.coroutine[index]);

            // Deactivate the character since they are not deployed
            this.character[index].SetActive(false);

            // Hide the Deploy meter
            bar.Hide();

            // Remember that the Coroutine has been stopped
            this.running[index] = false;

            // Reset the characterOnTop to null (-1)
            selectNode.characterOnTop = -1;

            // Change the text of the button back to Deploy
            this.characterDeployButton[index].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Deploy";
        }
        else
        {
            // If the Coroutine is NOT running
            // Change the UI text to cancel in case the player wants to cancel the deployment
            this.characterDeployButton[index].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";

            // Have the node store the character index
            selectNode.characterOnTop = index;

            // Disallow other characters to be deployed on the same tile
            this.checkNode(this.node);

            // Put the character on the node and active it
            this.character[index].transform.position = this.node.transform.position;
            this.character[index].transform.rotation = this.node.transform.rotation;
            this.character[index].SetActive(true);

            // Disallow the character to aim or shoot
            this.characterAttributes[index].disable();
            
            // Reset the meter so that it starts going down
            bar.Reset();

            // Set the character to half transparent to notify the player that the character is unable to do anything
            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

            // Setup and start the coroutine
            this.coroutine[index] = this.Spawn(index, 2.0f / (float)this.characterAttributes[index].MOB / .5f);
            this.StartCoroutine(this.coroutine[index]);
        }
    }

    public IEnumerator Spawn(int index, float time)
    {
        // Remember that the coroutine has started running
        this.running[index] = true;

        Debug.Log("Deploying Character " + index + " ...");
        Debug.Log("Time needed: " + time);

        // Wait for a certain amount of time
        yield return new WaitForSeconds(time);

        // Set the character to solid
        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);

        // Allow the character to aim and shoot
        this.characterAttributes[index].enabled();

        // Change the UI text back to Deploy
        this.characterDeployButton[index].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Deploy";

        // Enable the Retreat Button and make it cover the Deploy button
        this.characterRetreatButton[index].SetActive(true);

        // Remember that the coroutine has stopped running
        this.running[index] = false;
    }

    public void Retreat(int index)
    {
        // Reference the Deploy meter
        GameObject temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        DeployBar bar = temp.GetComponent<DeployBar>();

        if (this.running[index])
        {
            // Click on the same button again to cancel the retreat
            Debug.Log("The character retreat has been cancelled");
            this.StopCoroutine(this.coroutine[index]);

            this.characterAttributes[index].enabled();
            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            bar.Hide();
            this.running[index] = false;
            this.characterRetreatButton[index].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Retreat";
        }
        else
        {
            // Update the UI
            this.characterRetreatButton[index].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";

            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            bar.Reset();
            this.characterAttributes[index].disable();
            this.coroutine[index] = this.Retreating(index, 2.0f / (float)this.characterAttributes[index].MOB / .5f);
            this.StartCoroutine(this.coroutine[index]);
        }
    }

    private IEnumerator Retreating(int index, float time)
    {
        this.running[index] = true;

        Debug.Log("Retreating Character " + index + " ...");
        Debug.Log("Time needed: " + time);
        yield return new WaitForSeconds(time);

        // After waiting for a certain amount of time, deactivate the character
        this.character[index].SetActive(false);

        // Set the node's character on top to null (-1)
        SelectNode selectNode = this.characterPos[index].GetComponent<SelectNode>();
        selectNode.characterOnTop = -1;

        // Reset the character's position
        this.characterPos[index] = null;

        // This allow the player to immediately deploy another character to the same tile
        // It is very important to use this.node instead of this.characterPos[index]
        this.checkNode(this.node);

        // Disable the retreat button and reset its text
        this.characterRetreatButton[index].SetActive(false);
        this.characterRetreatButton[index].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Retreat";

        this.running[index] = false;
    }

    public void Transform(int index)
    {
        // Call the character transform function, we should move the transform function out of the characterAttack script probably
        this.character[index].GetComponent<characterAttack>().change();
    }

    public GameObject GetNode()
    {
        return this.node;
    }

    public void SetNode(GameObject newNode)
    {
        this.node = newNode;
        this.checkNode(newNode);
    }

    private void checkNode(GameObject nodeChecked)
    {
        // If the selected node has a character on top
        if (nodeChecked.GetComponent<SelectNode>().characterOnTop != -1)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != nodeChecked.GetComponent<SelectNode>().characterOnTop)
                {
                    // Disable the deploy button for all other characters
                    this.characterDeployButton[i].GetComponent<Button>().interactable = false;
                }
            }
        }
        else
        {
            // If not, enable deploy buttons for all characters
            // That being said, deployed characters will still have their deploy buttons covered by their retreat buttons
            for (int i = 0; i < 5; i++)
            {
                this.characterDeployButton[i].GetComponent<Button>().interactable = true;
            }
        }
    }
}
