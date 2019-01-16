using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choices : MonoBehaviour
{
    public GameObject ui;
    public GameObject prefab;

    // Icons for choices
    public Sprite battleIcon;
    public Sprite lowLightIcon;
    public Sprite lowOxygenIcon;
    public Sprite aftershockIcon;
    public Sprite radiativeIcon;
    public Sprite swarmIcon;
    public Sprite eliteIcon;
    public Sprite repairStationIcon;
    public Sprite freeLootIcon;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate 3 choice buttons
        GameObject newChoicesButton1 = (GameObject)GameObject.Instantiate(this.prefab, this.ui.transform);
        GameObject newChoicesButton2 = (GameObject)GameObject.Instantiate(this.prefab, this.ui.transform);
        GameObject newChoicesButton3 = (GameObject)GameObject.Instantiate(this.prefab, this.ui.transform);

        // Set them all to inactive
        newChoicesButton1.SetActive(false);
        newChoicesButton2.SetActive(false);
        newChoicesButton3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        // Generate choices before opening the panel
        // else, Destory previous choices before closing the panel
        if (!ui.activeSelf)
        {
            GenerateChoices();
        }
        else
        {
            // Set all choice buttons to inactive
            this.ui.transform.GetChild(0).gameObject.SetActive(false);
            this.ui.transform.GetChild(1).gameObject.SetActive(false);
            this.ui.transform.GetChild(2).gameObject.SetActive(false);
        }
        ui.SetActive(!ui.activeSelf);
    }

    public void GenerateChoices()
    {
        System.Random rnd = new System.Random();

        // Get a number between 1-3
        int choicesNumber = rnd.Next(1, 4);
        for (int i = 0; i < choicesNumber; i++)
        {
            int battleModifier = rnd.Next(100);

            // Generate options randomly
            if (battleModifier < 50)
            {
                this.Battle(i);
            }
            else if (battleModifier >= 50 && battleModifier < 55)
            {
                this.LowLight(i);
            }
            else if (battleModifier >= 55 && battleModifier < 60)
            {
                this.LowOxygen(i);
            }
            else if (battleModifier >= 60 && battleModifier < 65)
            {
                this.Aftershock(i);
            }
            else if (battleModifier >= 65 && battleModifier < 70)
            {
                this.Radiative(i);
            }
            else if (battleModifier >= 70 && battleModifier < 75)
            {
                this.Swarm(i);
            }
            else if (battleModifier >= 75 && battleModifier < 80)
            {
                this.Elite(i);
            }
            else if (battleModifier >= 80 && battleModifier < 85)
            {
                this.RepairStation(i);
            }
            else if (battleModifier >= 85 && battleModifier < 100)
            {
                this.FreeLoot(i);
            }
        }
    }

    public void Battle(int i)
    {
        // Active the choice button
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        // Setup the component of the button
        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.battleIcon, "Battle", "Are you ready for the challenge?");
    }

    public void LowLight(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.lowLightIcon, "Low Light Battle", "All characters have lower RNG.");
    }

    public void LowOxygen(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.lowOxygenIcon, "Low Oxygen Battle", "All characters have lower ROF.");
    }

    public void Aftershock(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.aftershockIcon, "Aftershock Battle", "The battlefield is constantly collapsing.");
    }

    public void Radiative(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.radiativeIcon, "Radiative Battle", "Enemies regenerate HP.");
    }

    public void Swarm(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.swarmIcon, "Swarm Battle", "All enemies are flying.");
    }

    public void Elite(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.eliteIcon, "Elite Battle", "Enemies are significantly stronger.");
    }

    public void RepairStation(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.repairStationIcon, "Repair Station", "No enemies./nThe Groundbreaker acquires 3 Durability.");
    }

    public void FreeLoot(int i)
    {
        GameObject newButton = this.ui.transform.GetChild(i).gameObject;
        newButton.SetActive(true);

        ChoicesButton button = newButton.GetComponent<ChoicesButton>();
        button.Setup(this.freeLootIcon, "Free Loot", "No enemies.");
    }
}
