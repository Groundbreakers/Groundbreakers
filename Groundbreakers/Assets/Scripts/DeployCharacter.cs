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

    private int characterCount = 0;

    private GameObject parent;

    // remove character with right mouse click
    void removeCharacter() {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    // deploy character with key 1~5
    void spawnCharacter() {
        if (this.parent.transform.childCount < this.maximumCharacter)
        {
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKeyDown("1"))
            {
                this.character = (GameObject)Instantiate(this.Character1, spawnPosition, this.transform.rotation);
                this.character.transform.parent = this.parent.transform;
            }
            else if (Input.GetKeyDown("2"))
            {
                this.character = (GameObject)Instantiate(this.Character2, spawnPosition, this.transform.rotation);
                this.character.transform.parent = this.parent.transform;
            }
            else if (Input.GetKeyDown("3"))
            {
                this.character = (GameObject)Instantiate(this.Character3, spawnPosition, this.transform.rotation);
                this.character.transform.parent = this.parent.transform;
            }
            else if (Input.GetKeyDown("4"))
            {
                this.character = (GameObject)Instantiate(this.Character4, spawnPosition, this.transform.rotation);
                this.character.transform.parent = this.parent.transform;
            }
            else if (Input.GetKeyDown("5"))
            {
                this.character = (GameObject)Instantiate(this.Character5, spawnPosition, this.transform.rotation);
                this.character.transform.parent = this.parent.transform;
            }
        }
    }

    void Start() {
        this.parent = GameObject.Find("character");
        Debug.Log(this.parent.name + " has " + this.parent.transform.childCount + " children");
    }

    void Update() {
        this.spawnCharacter();
        this.removeCharacter();
    }
}
