using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    //The Main Prefab Used To Create The Popups
    public GameObject popUpObject;

    //A List That Holds All Currently Instantiated PopUp Objects
    List<GameObject> popUpList;

    void Start()
    {
        popUpList = new List<GameObject>();
    }


    public void ProduceText(int damage, Transform transform)
    {
        //Create Instance Of Popup
        GameObject clone;
        clone = Instantiate(popUpObject, new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z), Quaternion.identity);

        //Set Text And Color Of Popup
        if (damage > 0)
        {
            clone.transform.GetComponent<TextMeshProUGUI>().text = "-" + damage;
            clone.transform.GetComponent<TextMeshProUGUI>().color = Color.red;
            clone.transform.GetComponent<TextMeshProUGUI>().fontSize = .3f;
        }
        else if (damage == 0)
        {
            clone.transform.GetComponent<TextMeshProUGUI>().text = "miss";
            clone.transform.GetComponent<TextMeshProUGUI>().color = Color.white;
            clone.transform.GetComponent<TextMeshProUGUI>().fontSize = .3f;
        }
        else
        {
            clone.transform.GetComponent<TextMeshProUGUI>().text = "ouch";
            clone.transform.GetComponent<TextMeshProUGUI>().color = Color.yellow;
            clone.transform.GetComponent<TextMeshProUGUI>().fontSize = .3f;
        }

        //Randomly Choose If Text Appears Center, Left, Or Right Of Character
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            clone.transform.GetComponent<TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Center;
        }
        else if (rand == 1)
        {
            clone.transform.GetComponent<TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Left;
        }
        else if (rand == 2)
        {
            clone.transform.GetComponent<TextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.Right;
        }

        //Add Clone To List And Start The Wait Coroutine
        popUpList.Add(clone);
        StartCoroutine(Wait(clone));
    }

    //Wait Half a Second Before Removing The Clone From The List, Then Destory It
    IEnumerator Wait(GameObject clone)
    {
        yield return new WaitForSeconds(.5f);
        popUpList.Remove(clone);
        Destroy(clone);
    }

    void FixedUpdate()
    {
        foreach (GameObject pop in popUpList)
        {
            pop.transform.GetComponent<TextMeshProUGUI>().fontSize = pop.transform.GetComponent<TextMeshProUGUI>().fontSize - Time.deltaTime * .3f;
        }
    }
}
