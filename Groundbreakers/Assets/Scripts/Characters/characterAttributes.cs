using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterAttributes : MonoBehaviour
{
    //Stats
    public int POW = 1;
    public int ROF = 2;
    public int RNG = 6;
    public int MOB = 3;
    public int AMP = 1;

    //Abilities
    public bool criticalDraw = false;
    public bool reaper = false;
    public bool alchemy = false;
    public bool anticipate = false;
    public bool chemicals = false;
    public bool corrode = false;
    public bool fleetFeet = false;
    public bool nullify = false;
    public bool chaos = false;
    public bool swiftness = false;
    public bool juggling = false;
    public bool exit = false;

    //Attack Effects
    public bool burstAE = false;
    public bool ricochetAE = false;
    public bool laserAE = false;
    public bool splashAE = false;
    public bool pierceAE = false;
    public bool traceAE = false;
    public bool cleaveAE = false;
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
    characterAttack attack;

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
        POW = 1;
        RNG = 6;
        AMP = 1;
    }

    public void melee()
    {
        stance = "melee";
        POW = 3;
        RNG = 3;
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

        bool[] temp = new bool[17];
        for(int i = 5; i < inventory.Length; i++)
        {
            if (i > 0)
            {
                temp[i-5] = true;
            }
            else
            {
                temp[i-5] = false;
            }
        }

        //update effects
        burstAE = temp[0];
        ricochetAE = temp[1];
        laserAE = temp[2];
        splashAE = temp[3];
        pierceAE = temp[4];
        traceAE = temp[5];
        cleaveAE = temp[6];
        whirwindAE = temp[7];
        reachAE = temp[8];
        slowSE = temp[9];
        stunSE = temp[10];
        burnSE = temp[11];
        markSE = temp[12];
        purgeSE = temp[13];
        breakSE = temp[14];
        blightSE = temp[15];
        netSE = temp[16];
    }

    public void resetStats()
    {
        if(stance == "gun")
        {
            POW = 1;
            ROF = 2;
            RNG = 6;
            MOB = 3;
            AMP = 1;
        }
        else
        {
            POW = 3;
            ROF = 2;
            RNG = 3;
            MOB = 3;
            AMP = 2;
        }
    }
    


}
