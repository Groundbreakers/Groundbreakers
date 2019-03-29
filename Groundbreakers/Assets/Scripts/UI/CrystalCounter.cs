using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalCounter : MonoBehaviour
{
    public Text crystalCounter;
    private int crystals;

    // Start is called before the first frame update
    void Start()
    {
        this.crystals = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        this.crystalCounter.text = this.crystals.ToString();
    }

    public int GetCrystals()
    {
        return this.crystals;
    }

    public void SetCrystals(int amount)
    {
        this.crystals += amount;
    }
}
