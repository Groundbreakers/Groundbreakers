//using UnityEngine;

//using Random = System.Random;

//public class RoutesGenerator : MonoBehaviour
//{
//    // Icons for choices
//    public Sprite aftershockIcon;

//    public Sprite battleIcon;

//    public Sprite eliteIcon;

//    public Sprite freeLootIcon;

//    public Sprite lowLightIcon;

//    public Sprite lowOxygenIcon;

//    public Sprite radiativeIcon;

//    public Sprite repairStationIcon;

//    public Sprite swarmIcon;

//    /// <summary>
//    /// The new Choices Button prefab.
//    /// </summary>
//    public GameObject prefab;

//    public GameObject ui;

//    public void GenerateChoices()
//    {
//        var rnd = new Random();

//        // Get a number between 1-3
//        var choicesNumber = rnd.Next(1, 4);
//        for (var i = 0; i < choicesNumber; i++)
//        {
//            // Active the choice button
//            var newButton = this.ui.transform.GetChild(i).gameObject;
//            newButton.SetActive(true);

//            // Setup the component of the button
//            var button = newButton.GetComponent<RoutesButton>();
//            var battleModifier = rnd.Next(100);

//            button.Setup(this.battleIcon, "Battle", "Are you ready for the challenge?");

//            // Generate options randomly
//            /*if (battleModifier < 50)
//            {
//                button.Setup(this.battleIcon, "Battle", "Are you ready for the challenge?");
//            }
//            else if (battleModifier >= 50 && battleModifier < 55)
//            {
//                button.Setup(this.lowLightIcon, "Low Light Battle", "All characters have lower RNG.");
//            }
//            else if (battleModifier >= 55 && battleModifier < 60)
//            {
//                button.Setup(this.lowOxygenIcon, "Low Oxygen Battle", "All characters have lower ROF.");
//            }
//            else if (battleModifier >= 60 && battleModifier < 65)
//            {
//                button.Setup(this.aftershockIcon, "Aftershock Battle", "The battlefield is constantly collapsing.");
//            }
//            else if (battleModifier >= 65 && battleModifier < 70)
//            {
//                button.Setup(this.radiativeIcon, "Radiative Battle", "Enemies regenerate HP.");
//            }
//            else if (battleModifier >= 70 && battleModifier < 75)
//            {
//                button.Setup(this.swarmIcon, "Swarm Battle", "All enemies are flying.");
//            }
//            else if (battleModifier >= 75 && battleModifier < 80)
//            {
//                button.Setup(this.eliteIcon, "Elite Battle", "Enemies are significantly stronger.");
//            }
//            else if (battleModifier >= 80 && battleModifier < 85)
//            {
//                button.Setup(this.repairStationIcon, "Repair Station", "No enemies./nThe Groundbreaker acquires 3 Durability.");
//            }
//            else if (battleModifier >= 85 && battleModifier < 100)
//            {
//                button.Setup(this.freeLootIcon, "Free Loot", "No enemies.");
//            }*/
//        }
//    }

//    public void Toggle()
//    {
//        // Generate choices before opening the panel
//        // else, hide previous choices before closing the panel
//        if (!this.ui.activeSelf)
//        {
//            this.GenerateChoices();
//        }
//        else
//        {
//            // Set all choice buttons to inactive
//            this.ui.transform.GetChild(0).gameObject.SetActive(false);
//            this.ui.transform.GetChild(1).gameObject.SetActive(false);
//            this.ui.transform.GetChild(2).gameObject.SetActive(false);
//        }

//        this.ui.SetActive(!this.ui.activeSelf);
//    }

//    // Start is called before the first frame update
//    private void Start()
//    {
//        // Instantiate 3 choice buttons
//        var newChoicesButton1 = Instantiate(this.prefab, this.ui.transform);
//        var newChoicesButton2 = Instantiate(this.prefab, this.ui.transform);
//        var newChoicesButton3 = Instantiate(this.prefab, this.ui.transform);

//        // Set them all to inactive
//        newChoicesButton1.SetActive(false);
//        newChoicesButton2.SetActive(false);
//        newChoicesButton3.SetActive(false);
//    }
//}