// Currently supports 2 characters, can always add more

using System;

using UnityEngine;

public class DeployCharacter : MonoBehaviour
{
    private GameObject character1;

    private GameObject character2;

    private GameObject character3;

    private GameObject character4;

    private GameObject character5;

    public Color hoverColor;

    private SpriteRenderer rend;

    private Color startColor;

    private int characterOnTop;

    void Start()
    {
        GameObject characterList = GameObject.Find("CharacterList");
        this.character1 = characterList.transform.GetChild(0).gameObject;
        this.character2 = characterList.transform.GetChild(1).gameObject;
        this.character3 = characterList.transform.GetChild(2).gameObject;
        this.character4 = characterList.transform.GetChild(3).gameObject;
        this.character5 = characterList.transform.GetChild(4).gameObject;

        this.rend = GetComponent<SpriteRenderer>();
        this.startColor = rend.color;

        this.characterOnTop = 0;
    }

    // remove character with right mouse click on it
    void removeCharacter()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (this.characterOnTop == 0)
            {
                return;
            }
            else if (this.characterOnTop == 1)
            {
                this.character1.SetActive(false);
                this.characterOnTop = 0;
            }
            else if (this.characterOnTop == 2)
            {
                this.character2.SetActive(false);
                this.characterOnTop = 0;
            }
            else if (this.characterOnTop == 3)
            {
                this.character3.SetActive(false);
                this.characterOnTop = 0;
            }
            else if (this.characterOnTop == 4)
            {
                this.character4.SetActive(false);
                this.characterOnTop = 0;
            }
            else if (this.characterOnTop == 5)
            {
                this.character5.SetActive(false);
                this.characterOnTop = 0;
            }
        }
    }

    // deploy character with key 1~2 on the node the cursor is on. 
    void spawnCharacter()
    {
        if (Input.GetKeyDown("1") && this.characterOnTop == 0)
        {
            this.character1.transform.position = this.transform.position;
            this.character1.transform.rotation = this.transform.rotation;
            this.character1.SetActive(true);
            this.characterOnTop = 1;
        }
        else if (Input.GetKeyDown("2") && this.characterOnTop == 0)
        {
            this.character2.transform.position = this.transform.position;
            this.character2.transform.rotation = this.transform.rotation;
            this.character2.SetActive(true);
            this.characterOnTop = 2;
        }
        else if (Input.GetKeyDown("3") && this.characterOnTop == 0)
        {
            this.character3.transform.position = this.transform.position;
            this.character3.transform.rotation = this.transform.rotation;
            this.character3.SetActive(true);
            this.characterOnTop = 3;
        }
        else if (Input.GetKeyDown("4") && this.characterOnTop == 0)
        {
            this.character4.transform.position = this.transform.position;
            this.character4.transform.rotation = this.transform.rotation;
            this.character4.SetActive(true);
            this.characterOnTop = 4;
        }
        else if (Input.GetKeyDown("5") && this.characterOnTop == 0)
        {
            this.character5.transform.position = this.transform.position;
            this.character5.transform.rotation = this.transform.rotation;
            this.character5.SetActive(true);
            this.characterOnTop = 5;
        }
    }
    
    void OnMouseOver()
    {
        this.rend.color = this.hoverColor;
        this.spawnCharacter();
        this.removeCharacter();
    }

    void OnMouseExit()
    {
        this.rend.color = this.startColor;
    }
}
