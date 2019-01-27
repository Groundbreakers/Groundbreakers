using UnityEngine;

public class DeployCharacter : MonoBehaviour
{
    public GameObject Character1;

    public GameObject Character2;

    public GameObject Character3;

    public GameObject Character4;

    public GameObject Character5;

    public int maximumCharacter;

    private GameObject character = null; // by default each node has no character on it

    private GameObject parent;

    public Color hoverColor;

    private SpriteRenderer rend;

    private Color startColor;


    // remove character with right mouse click on it
    void removeCharacter()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Player")
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    // deploy character with key 1~5 on the node the cursor is on
    void spawnCharacter()
    {
        if (this.parent.transform.childCount < this.maximumCharacter)
        {
            if (Input.GetKeyDown("1") && this.character == null )
            {
                this.character = (GameObject)Instantiate(this.Character1, this.transform.position, this.transform.rotation);
                this.character.transform.parent = this.parent.transform;
            }
            else if (Input.GetKeyDown("2") && this.character == null)
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
            }
        }
    }

    void Start()
    {
        this.parent = GameObject.Find("character");
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;
    }

    void Update()
    {
        removeCharacter();
    }

    void OnMouseOver()
    {
        if(this.character == null) rend.color = hoverColor;
        spawnCharacter();
    }

    void OnMouseExit()
    {
        rend.color = startColor;
    }
}
