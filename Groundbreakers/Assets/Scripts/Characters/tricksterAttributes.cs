using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tricksterAttributes : MonoBehaviour
{
    //Stats
    public int POW = 1;
    public int ROF = 2;
    public int RNG = 6;
    public int MOB = 3;
    public int AMP = 0;

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

    //Scripts
    characterAttack attack;

    //Modules
    public GameObject[] modules = new GameObject[6];

    void Awake()
    {
        attack = GetComponent<characterAttack>();
    }
    //functions
    public void gun()
    {
        POW = 1;
        RNG = 6;
        AMP = 0;
    }

    public void melee()
    {
        POW = 3;
        RNG = 3;
        AMP = 2;
    }

    public void updateStats()
    {

    }
    


}
