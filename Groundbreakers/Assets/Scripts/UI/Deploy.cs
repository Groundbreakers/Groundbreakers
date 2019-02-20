using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Deploy : MonoBehaviour
{
    #region Inspector Properties

    public GameObject[] characterDeployButton;

    public GameObject[] characterReleaseButton;

    public GameObject[] characterCancelButton;

    public GameObject ui;

    #endregion

    #region Internal Fields

    private GameObject[] character = new GameObject[5];

    private characterAttributes[] characterAttributes = new characterAttributes[5];

    private GameObject[] characterPos = new GameObject[5];

    private IEnumerator[] coroutine = new IEnumerator[5];

    private GameObject node;

    [SerializeField]
    private String[] running = new String[5];

    #endregion

    #region Public API

    public void EnableDeploy(int index)
    {
        foreach (Transform child in GameObject.Find("TileMap").transform)
        {
            var selectNode = child.gameObject.GetComponent<SelectNode>();
            if (selectNode != null)
            {
                selectNode.EnableDeploy(index);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            this.characterReleaseButton[i].SetActive(false);
        }
        this.characterReleaseButton[index].SetActive(true);
    }

    public void DisableDeploy(int index)
    {
        foreach (Transform child in GameObject.Find("TileMap").transform)
        {
            var selectNode = child.gameObject.GetComponent<SelectNode>();
            if (selectNode != null)
            {
                selectNode.DisableDeploy();
            }
        }

        this.characterReleaseButton[index].SetActive(false);
    }

    public void CancelDeploy(int index)
    {
        for (int i = 0; i < 5; i++)
        {
            this.characterReleaseButton[i].SetActive(false);
        }

        if (this.running[index] == "Spawn")
        {
            this.DeployCharacter(index);
        }
        else if (this.running[index] == "Retreating")
        {
            this.Retreat(index);
        }
    }

    public void DeployCharacter(int index)
    {
        // If the character is already activated, redeploy instead of deploy
        if (this.character[index].activeSelf && this.running[index] != "Spawn" && this.running[index] != "Retreating")
        {
            Debug.Log("The character has been deployed, relocating...");
            this.StartCoroutine(this.Redeploy(index, 2.0f / this.characterAttributes[index].MOB / .5f));
        }
        else
        {
            // Disable the deploy
            this.DisableDeploy(index);

            // Reference the Deploy meter
            var temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            var bar = temp.GetComponent<DeployBar>();

            // Remember the character's position
            this.characterPos[index] = this.node;
            var selectNode = this.characterPos[index].GetComponent<SelectNode>();

            // If the Coroutine is still running
            if (this.running[index] == "Spawn")
            {
                // Deactivate the cancel button
                this.characterCancelButton[index].SetActive(false);

                // Click on the same button again to cancel the deployment
                Debug.Log("The character deployment has been cancelled");
                this.StopCoroutine(this.coroutine[index]);

                // Deactivate the character since they are not deployed
                this.character[index].SetActive(false);

                // Hide the Deploy meter
                bar.Hide();

                // Remember that the Coroutine has been stopped
                this.running[index] = "Cancelled";

                // Reset the characterOnTop to null (-1)
                selectNode.SetCharacterIndex(-1);
            }
            else
            {
                // If the Coroutine is NOT running
                // Deactivate the cancel button
                this.characterCancelButton[index].SetActive(true);

                // Have the node store the character index
                selectNode.SetCharacterIndex(index);

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
                this.coroutine[index] = this.Spawn(index, 2.0f / this.characterAttributes[index].MOB / .5f);
                this.StartCoroutine(this.coroutine[index]);
            }
        }
    }

    public GameObject GetNode()
    {
        return this.node;
    }

    /// <summary>
    /// Instantly retreat(deactivate) all characters. Only Called by Battle manager
    /// when battle finishes.
    /// </summary>
    public void InstantRetreatAllCharacter()
    {
        for (int i = 0; i < this.character.Length; i++)
        {
            this.DeactivateCharacter(i);
        }
    }

    public void Retreat(int index)
    {
        // Reference the Deploy meter
        var temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        var bar = temp.GetComponent<DeployBar>();

        if (this.running[index] == "Retreating")
        {
            // Deactivate the cancel button
            this.characterCancelButton[index].SetActive(false);

            // Click on the same button again to cancel the retreat
            Debug.Log("The character retreat has been cancelled");
            this.StopCoroutine(this.coroutine[index]);

            this.characterAttributes[index].enabled();
            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            bar.Hide();
            this.running[index] = "Cancelled";
        }
        else
        {
            // Deactivate the cancel button
            this.characterCancelButton[index].SetActive(true);

            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            bar.Reset();
            this.characterAttributes[index].disable();
            this.coroutine[index] = this.Retreating(index, 2.0f / this.characterAttributes[index].MOB / .5f);
            this.StartCoroutine(this.coroutine[index]);
        }
    }

    public void SetNode(GameObject newNode)
    {
        this.node = newNode;
    }

    public void Toggle()
    {
        this.node = null;
        this.ui.SetActive(!this.ui.activeSelf);
    }

    public void Transform(int index)
    {
        // Call the character transform function, we should move the transform function out of the characterAttack script probably
        this.character[index].GetComponent<characterAttack>().change();
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // Reference five characters and their attributes
        var characterList = GameObject.Find("CharacterList");
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
        for (int i = 0; i < 5; i++)
        {
            this.running[i] = "Done";
        }
    }

    #endregion

    #region Internal Functions

    private IEnumerator Spawn(int index, float time)
    {
        // Remember that the coroutine has started running
        this.running[index] = "Spawn";

        Debug.Log("Deploying Character " + index + " ...");
        Debug.Log("Time needed: " + time);

        // Wait for a certain amount of time
        yield return new WaitForSeconds(time);

        // Set the character to solid
        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);

        // Allow the character to aim and shoot
        this.characterAttributes[index].enabled();

        // Deactivate the cancel button
        this.characterCancelButton[index].SetActive(false);

        // Remember that the coroutine has stopped running
        this.running[index] = "Done";
    }

    private IEnumerator Retreating(int index, float time)
    {
        this.running[index] = "Retreating";

        Debug.Log("Retreating Character " + index + " ...");
        Debug.Log("Time needed: " + time);
        yield return new WaitForSeconds(time);

        // After waiting for a certain amount of time, deactivate the character
        this.DeactivateCharacter(index);
    }

    /// <summary>
    /// After waiting for a certain amount of time, deactivate the character
    /// </summary>
    /// <param name="index">
    /// The Character index.
    /// </param>
    private void DeactivateCharacter(int index)
    {
        this.character[index].SetActive(false);

        // Set the node's character on top to null (-1)
        var selectNode = this.characterPos[index].GetComponent<SelectNode>();
        selectNode.SetCharacterIndex(-1);

        // Reset the character's position
        this.characterPos[index] = null;

        this.running[index] = "Done";
    }

    private IEnumerator Redeploy(int index, float time)
    {
        this.Retreat(index);
        yield return new WaitForSeconds(time);
        if (this.running[index] == "Done")
        {
            this.DeployCharacter(index);
        }
    }

    #endregion
}