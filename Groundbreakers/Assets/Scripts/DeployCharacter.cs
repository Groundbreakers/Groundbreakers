using UnityEngine;

public class DeployCharacter : MonoBehaviour
{
    public GameObject Character1;

    public GameObject Character2;

    public GameObject Character3;

    public GameObject Character4;

    public GameObject Character5;

    public int maximumCharacter;

    private GameObject character;

    private GameObject character2;

    private GameObject parent;

    public Color hoverColor;

    private SpriteRenderer rend;

    private Color startColor;

    //private string type;
    

    // remove character with right mouse click on it
    void removeCharacter()
    {
        if (Input.GetMouseButtonDown(1))
        {
            activeSetFalseAll();
        }
    }

    // deploy character with key 1~5 on the node the cursor is on
    void spawnCharacter()
    {
        
            if (Input.GetKeyDown("1") && isEmpty() == true)
            {
                this.character.SetActive(true);
               
            }
            else if (Input.GetKeyDown("2") && isEmpty() == true)
            {
                this.character2.SetActive(true);

            }
        /* else if (Input.GetKeyDown("2") && this.character == null)
         {
             this.character = (GameObject)Instantiate(this.Character2, this.transform.position, this.transform.rotation);
             this.character.transform.parent = this.parent.transform;
         }
         else if (Input.GetKeyDown("3") && this.character == null)
         {
             this.character = (GameObject)Instantiate(this.Character3, this.transform.position, this.transform.rotation);
             this.character.transform.parent = this.parent.transform;
         }
         else if (Input.GetKeyDown("4") && this.character == null)
         {
             this.character = (GameObject)Instantiate(this.Character4, this.transform.position, this.transform.rotation);
             this.character.transform.parent = this.parent.transform;
         }
         else if (Input.GetKeyDown("5") && this.character == null)
         {
             this.character = (GameObject)Instantiate(this.Character5, this.transform.position, this.transform.rotation);
             this.character.transform.parent = this.parent.transform;
         }*/

    }

    private bool isEmpty() {

        if (this.character.activeSelf == false && this.character2.activeSelf == false) return true;
        else return false;
    }

    private void activeSetFalseAll() {
        this.character.SetActive(false);
        this.character2.SetActive(false);
    }

    void Start()
    {
        this.parent = GameObject.Find("character");
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;
        
        this.character = (GameObject)Instantiate(this.Character1, this.transform.position, this.transform.rotation);
        this.character.SetActive(false);

        this.character2 = (GameObject)Instantiate(this.Character2, this.transform.position, this.transform.rotation);
        this.character2.SetActive(false);





    }

    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if(this.character == null) rend.color = hoverColor;
        spawnCharacter();
        removeCharacter();
    }

    void OnMouseExit()
    {
        rend.color = startColor;
    }
}
