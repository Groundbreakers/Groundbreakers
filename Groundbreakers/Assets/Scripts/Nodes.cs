// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildManager.cs" company="UCSC">
//   MIT
// </copyright>
// <summary>
//   Javy Wu
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

public class Nodes : MonoBehaviour
{
    public Color OnMouseColor;

    private GameObject character;

    private bool empty = true;

    private SpriteRenderer rend;

    private Color StartColor;

    void OnMouseDown() {
        if (this.character != null && this.empty == false)
        {
            Debug.Log("can't build there!");
        }
        else if (this.character == null && this.empty == true
                                        && BuildManager.characterCount < BuildManager.maximumCharacter)
        {
            this.empty = false;
            GameObject characterToPlace = BuildManager.instance.GetCharacterToPlace();
            this.character = (GameObject)Instantiate(
                characterToPlace,
                this.transform.position,
                this.transform.rotation);
            BuildManager.characterCount += 1;
        }

        if (BuildManager.characterCount > BuildManager.maximumCharacter)
        {
            Debug.Log("Can't place more character!");
        }
    }

    void OnMouseEnter() {
        this.rend.color = this.OnMouseColor;
    }

    void OnMouseExit() {
        this.rend.color = this.StartColor;
    }

    void Start() {
        this.rend = this.gameObject.GetComponent<SpriteRenderer>();
        this.StartColor = this.rend.color;
    }
}
