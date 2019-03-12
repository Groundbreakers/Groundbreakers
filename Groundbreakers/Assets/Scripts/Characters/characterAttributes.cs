using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAttributes : MonoBehaviour
{
    //Stats
    public int POW = 2;
    public int ROF = 2;
    public int RNG = 2;
    public int MOB = 2;
    public int AMP = 1;



    //Attack Effects
    public bool burstAE = false;
    public bool ricochetAE = false;
    public bool laserAE = false;
    public bool splashAE = false;
    public bool pierceAE = false;
    public bool traceAE = false;
    public bool whirwindAE = false;
    public bool reachAE = false;

    //Status Effects
    public bool slowSE = false;
    public bool stunSE = false;
    public bool burnSE = false;
    public bool markSE = false;
    public bool purgeSE = false;
    public bool breakSE = false;
    public bool blightSE = false;
    public bool netSE = false;
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
        MOB += inventory[3];
        AMP += inventory[4];

        if(POW > 10)
        {
            POW = 10;
        }

        if (ROF > 10)
        {
            ROF = 10;
        }

        if (RNG > 10)
        {
            RNG = 10;
        }

        if (MOB > 10)
        {
            MOB = 10;
        }

        if (AMP > 10)
        {
            AMP = 10;
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
        burstAE = temp[0];
        ricochetAE = temp[1];
        laserAE = temp[2];
        splashAE = temp[3];
        pierceAE = temp[4];
        traceAE = temp[5];
        whirwindAE = temp[6];
        reachAE = temp[7];
        slowSE = temp[8];
        stunSE = temp[9];
        burnSE = temp[10];
        markSE = temp[11];
        purgeSE = temp[12];
        breakSE = temp[13];
        blightSE = temp[14];
        netSE = temp[15];
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
