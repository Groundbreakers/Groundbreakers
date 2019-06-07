//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LootGenerator : MonoBehaviour
//{
//    public GameObject ui;
//    public GameObject prefab;
//    public GameObject layout;
//    public GameObject powerModule;
//    public GameObject speedModule;
//    public GameObject rangeModule;
//    public GameObject handinessModule;
//    public GameObject strikeModule;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Instantiate 5 loot buttons
//        GameObject newLoot1 = (GameObject)GameObject.Instantiate(this.prefab, this.layout.transform);
//        GameObject newLoot2 = (GameObject)GameObject.Instantiate(this.prefab, this.layout.transform);
//        GameObject newLoot3 = (GameObject)GameObject.Instantiate(this.prefab, this.layout.transform);
//        GameObject newLoot4 = (GameObject)GameObject.Instantiate(this.prefab, this.layout.transform);
//        GameObject newLoot5 = (GameObject)GameObject.Instantiate(this.prefab, this.layout.transform);

//        // Set them all to inactive
//        newLoot1.SetActive(false);
//        newLoot2.SetActive(false);
//        newLoot3.SetActive(false);
//        newLoot4.SetActive(false);
//        newLoot5.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            Toggle();
//        }
//    }

//    public void Toggle()
//    {
//        // Generate loots before opening the panel
//        // else, hide previous loots before closing the panel
//        if (!ui.activeSelf)
//        {
//            GenerateLoots();
//        }
//        else
//        {
//            // Set all loot buttons and tooltips to inactive
//            this.layout.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
//            this.layout.transform.GetChild(0).gameObject.SetActive(false);
//            this.layout.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
//            this.layout.transform.GetChild(1).gameObject.SetActive(false);
//            this.layout.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(false);
//            this.layout.transform.GetChild(2).gameObject.SetActive(false);
//            this.layout.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);
//            this.layout.transform.GetChild(3).gameObject.SetActive(false);
//            this.layout.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.SetActive(false);
//            this.layout.transform.GetChild(4).gameObject.SetActive(false);
//        }
//        ui.SetActive(!ui.activeSelf);
//    }

//    public void GenerateLoots()
//    {
//        System.Random rnd = new System.Random();

//        // Get a number between 1-5
//        int lootNumber = rnd.Next(1, 6);
//        for (int i = 0; i < lootNumber; i++)
//        {
//            // Active the loot button
//            GameObject newButton = this.layout.transform.GetChild(i).gameObject;
//            newButton.SetActive(true);

//            // Setup the component of the button
//            LootButton button = newButton.GetComponent<LootButton>();
//            int lootIndex = rnd.Next(100);

//            // Generate options randomly
//            if (lootIndex < 50)
//            {
//                button.Setup(this.powerModule);
//            }
//            else if (lootIndex >= 50 && lootIndex < 55)
//            {
//                button.Setup(this.speedModule);
//            }
//            else if (lootIndex >= 55 && lootIndex < 60)
//            {
//                button.Setup(this.speedModule);
//            }
//            else if (lootIndex >= 60 && lootIndex < 65)
//            {
//                button.Setup(this.rangeModule);
//            }
//            else if (lootIndex >= 65 && lootIndex < 70)
//            {
//                button.Setup(this.rangeModule);
//            }
//            else if (lootIndex >= 70 && lootIndex < 75)
//            {
//                button.Setup(this.handinessModule);
//            }
//            else if (lootIndex >= 75 && lootIndex < 80)
//            {
//                button.Setup(this.handinessModule);
//            }
//            else if (lootIndex >= 80 && lootIndex < 85)
//            {
//                button.Setup(this.strikeModule);
//            }
//            else if (lootIndex >= 85 && lootIndex < 100)
//            {
//                button.Setup(this.strikeModule);
//            }
//        }
//    }
//}