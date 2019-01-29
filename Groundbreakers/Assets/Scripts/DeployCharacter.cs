//// Currently supports 2 characters, can always add more

//using UnityEngine;

//public class DeployCharacter : MonoBehaviour
//{
//    public GameObject Character1;

//    public GameObject Character2;

//    private GameObject character1;

//    private GameObject character2;

//    private static int character1Count = 0;

//    private static int character2Count = 0;

//    private int maxNumForEachCharacterType = 1;
    
//    public Color hoverColor;

//    private SpriteRenderer rend;

//    private Color startColor;

    
//    // remove character with right mouse click on it
//    void removeCharacter()
//    {
//        if (Input.GetMouseButtonDown(1))
//        {
//            if (this.character1.activeSelf == true)
//            {
//                this.character1.SetActive(false);
//                character1Count--;
//            }
//            else if (this.character2.activeSelf == true)
//            {
//                this.character2.SetActive(false);
//                character2Count--;
//            }
//        }
//    }

//    // deploy character with key 1~2 on the node the cursor is on. 
//    void spawnCharacter()
//    {
//        if (Input.GetKeyDown("1") && isEmpty() == true && character1Count < maxNumForEachCharacterType)
//        {
//            character1.SetActive(true);
//            character1Count++;
//        }
//        else if (Input.GetKeyDown("2") && isEmpty() == true && character2Count < maxNumForEachCharacterType)
//        {
//            character2.SetActive(true);
//            character2Count++;
//        }
//    }

//    private bool isEmpty()
//    {
//        if (this.character1.activeSelf == false && this.character2.activeSelf == false) return true;
//        else return false;
//    }
    
//    void Start()
//    {
//        rend = GetComponent<SpriteRenderer>();
//        startColor = rend.color;
        
//        this.character1 = (GameObject)Instantiate(this.Character1, this.transform.position, this.transform.rotation);
//        this.character1.SetActive(false);
        
//        this.character2 = (GameObject)Instantiate(this.Character2, this.transform.position, this.transform.rotation);
//        this.character2.SetActive(false);
//    }
    
//    void OnMouseOver()
//    {
//        rend.color = hoverColor;
//        spawnCharacter();
//        removeCharacter();
//    }

//    void OnMouseExit()
//    {
//        rend.color = startColor;
//    }
//}
