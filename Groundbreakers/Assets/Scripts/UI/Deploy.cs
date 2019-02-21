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

    public GameObject[] characterTransformButton;

    public GameObject ui;

    #endregion

    #region Internal Fields

    private GameObject[] character = new GameObject[5];

    private characterAttributes[] characterAttributes = new characterAttributes[5];

    private GameObject[] characterPos = new GameObject[5];

    private IEnumerator[] coroutine = new IEnumerator[5];

    private GameObject[] targetPos = new GameObject[5];

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
        if (this.running[index] == "Transforming")
        {
            Debug.Log("The character is transforming, do nothing instead...");
        }
        else if (this.character[index].activeSelf && this.running[index] != "Spawn" && this.running[index] != "Retreating")
        {
            // If the character is already activated, redeploy instead of deploy
            Debug.Log("The character has been deployed, relocating...");
            this.StartCoroutine(this.Redeploy(index, 2.0f / this.characterAttributes[index].MOB / .5f));
        }
        else
        {
            // Reference the Deploy meter
            var temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            var bar = temp.GetComponent<DeployBar>();

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
                this.targetPos[index].GetComponent<SelectNode>().SetCharacterIndex(-1);
            }
            else
            {
                // If the Coroutine is NOT running
                this.characterTransformButton[index].GetComponent<Button>().interactable = false;

                // Activate the cancel button
                this.characterCancelButton[index].SetActive(true);

                // Have the node store the character index
                this.targetPos[index].GetComponent<SelectNode>().SetCharacterIndex(index);

                // Put the character on the node and active it
                this.character[index].transform.position = this.targetPos[index].transform.position;
                this.character[index].transform.rotation = this.targetPos[index].transform.rotation;
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

    public GameObject GetTargetPos(int index)
    {
        return this.targetPos[index];
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

            if (this.characterPos[index] != this.targetPos[index])
            {
                this.targetPos[index].GetComponent<SelectNode>().SetCharacterIndex(-1);
            }

            this.running[index] = "Cancelled";

            this.characterTransformButton[index].GetComponent<Button>().interactable = true;
        }
        else
        {
            this.characterTransformButton[index].GetComponent<Button>().interactable = false;

            // Deactivate the cancel button
            this.characterCancelButton[index].SetActive(true);

            this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
            bar.Reset();
            this.characterAttributes[index].disable();
            this.coroutine[index] = this.Retreating(index, 2.0f / this.characterAttributes[index].MOB / .5f);
            this.StartCoroutine(this.coroutine[index]);
        }
    }

    public void SetTargetPos(int index, GameObject newNode)
    {
        this.targetPos[index] = newNode;
    }

    public void Toggle()
    {
        for (int i = 0; i < 5; i++)
        {
            this.targetPos[i] = null;
        }
        this.ui.SetActive(!this.ui.activeSelf);
    }

    public void Transform(int index)
    {
        // Disable the transform button and the deploy button
        this.characterTransformButton[index].GetComponent<Button>().interactable = false;
        this.characterDeployButton[index].GetComponent<Button>().interactable = false;

        // Reference the Deploy meter
        var temp = this.character[index].transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        var bar = temp.GetComponent<DeployBar>();
        bar.Reset();

        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
        this.characterAttributes[index].disable();

        this.coroutine[index] = this.Transforming(index, 2.0f / this.characterAttributes[index].MOB / .5f);
        this.StartCoroutine(this.coroutine[index]);
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

        // Remember the character's position
        this.characterPos[index] = this.targetPos[index];

        // Set the character to solid
        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);

        // Allow the character to aim and shoot
        this.characterAttributes[index].enabled();

        // Deactivate the cancel button
        this.characterCancelButton[index].SetActive(false);

        // Enable the transform button after deployment
        this.characterTransformButton[index].GetComponent<Button>().interactable = true;

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

    private IEnumerator Transforming(int index, float time)
    {
        this.running[index] = "Transforming";

        // Call the character transform function, we should move the transform function out of the characterAttack script probably
        this.character[index].GetComponent<characterAttack>().change();

        Debug.Log("Transforming Character " + index + " ...");
        Debug.Log("Time needed: " + time);
        yield return new WaitForSeconds(time);

        this.character[index].GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);

        // Allow the character to aim and shoot
        this.characterAttributes[index].enabled();

        // Enable the transform button and the deploy button
        this.characterTransformButton[index].GetComponent<Button>().interactable = true;
        this.characterDeployButton[index].GetComponent<Button>().interactable = true;

        this.running[index] = "Done";
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
        this.characterPos[index].GetComponent<SelectNode>().SetCharacterIndex(-1);

        // Reset the character's position
        this.characterPos[index] = null;

        this.running[index] = "Done";
    }

    private IEnumerator Redeploy(int index, float time)
    {
        // Have the node store the character index
        this.targetPos[index].GetComponent<SelectNode>().SetCharacterIndex(index);

        this.Retreat(index);
        yield return new WaitForSeconds(time);
        if (this.running[index] == "Done")
        {
            this.DeployCharacter(index);
        }
    }

    #endregion
}