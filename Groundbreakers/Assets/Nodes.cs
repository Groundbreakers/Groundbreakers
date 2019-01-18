using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    private SpriteRenderer rend;
    public Color OnMouseColor;
    private Color StartColor;
    private GameObject character;
    private bool empty = true;

    void Start()
    {
        rend = gameObject.GetComponent<SpriteRenderer>();
        this.StartColor = this.rend.color;


    }

    void OnMouseEnter()
    {
        this.rend.color = this.OnMouseColor;
    }

    void OnMouseDown()
    {

        if (this.character != null && this.empty == false)
        {
            Debug.Log("can't build there!");

        }
        else if (this.character == null && this.empty == true && BuildManager.characterCount < BuildManager.maximumCharacter)
        {
            this.empty = false;
            GameObject characterToPlace = BuildManager.instance.GetCharacterToPlace();
            character = (GameObject)Instantiate(characterToPlace, transform.position, transform.rotation);
            BuildManager.characterCount += 1;
        }

        if (BuildManager.characterCount > BuildManager.maximumCharacter)
        {
            Debug.Log("Can't place more character!");
        }


    }



    void OnMouseExit()
    {
        this.rend.color = this.StartColor;

    }

}



