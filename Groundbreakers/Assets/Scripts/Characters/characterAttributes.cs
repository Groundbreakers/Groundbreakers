using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAttributes : MonoBehaviour
{

    //Name
    public string name;
    public string profession;
    [Range(1, 5)]
    public int Level = 1;
    //Stats
    public int POW = 2;
    public int ROF = 2;
    public int RNG = 2;
    public int AMP = 1;
    public int MOB = 2;



    //Attack Effects
    public bool laserAE = false;
    public bool pierceAE = false;
    public bool whirlwindAE = false;
    public bool multishotAE = false;

    //Status Effects
    public bool slowSE = false;
    public bool stunSE = false;
    public bool burnSE = false;
    public bool disabled = false;

    //Scripts
    private characterAttack attack;

    //stance
    private string stance = "gun";

    //Modules
    public GameObject[] modules = new GameObject[6];

    void Awake()
    {
        attack = GetComponent<characterAttack>();
    }
    //functions
    public void gun()
    {
        stance = "gun";
        POW = 2;
        RNG = 2;
        AMP = 1;
    }

    public void melee()
    {
        stance = "melee";
        POW = 3;
        RNG = 1;
        AMP = 2;
    }

    public void disable()
    {
        disabled = true;
    }

    public void enabled()
    {
        disabled = false;
    }


    public void updateAttributes(int[] inventory)
    {
        //reset stats and update character attributes
        resetStats();
        POW += inventory[0];
        ROF += inventory[1];
        RNG += inventory[2];
        AMP += inventory[3];
        MOB += inventory[4];
        

        if(POW > 5)
        {
            POW = 5;
        }

        if (ROF > 5)
        {
            ROF = 5;
        }

        if (RNG > 5)
        {
            RNG = 5;
        }

        if (MOB > 5)
        {
            MOB = 5;
        }

        if (AMP > 5)
        {
            AMP = 5;
        }



        bool[] temp = new bool[16];
        for (int i = 5; i < inventory.Length; i++)
        {
            if (inventory[i] > 0)
            {
                temp[i - 5] = true;
            }
            else
            {
                temp[i - 5] = false;
            }
        }

        //update effects
        laserAE = temp[2];
        pierceAE = temp[4];
        whirlwindAE = temp[6];
        slowSE = temp[8];
        stunSE = temp[9];
        burnSE = temp[10];
    }

    public void resetStats()
    {
        if (stance == "gun")
        {
            POW = 2;
            ROF = 2;
            RNG = 2;
            MOB = 2;
            AMP = 1;
        }
        else
        {
            POW = 3;
            ROF = 2;
            RNG = 1;
            MOB = 2;
            AMP = 2;
        }
    }

    public void Transform()
    {
        if (stance == "gun")
        {
            POW += 1;
            RNG -= 1;
            AMP += 1;
        }
        else
        {
            POW -= 1;
            RNG += 1;
            AMP -= 1;
        }
    }


    public void keepStance()
    {
        attack.setStance();
    }
}
