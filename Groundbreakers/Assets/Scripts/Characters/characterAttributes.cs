using UnityEngine;

public class characterAttributes : MonoBehaviour
{
    public bool alchemy = false;

    public int AMP = 1;

    public bool anticipate = false;

    public bool blightSE = false;

    public bool breakSE = false;

    public bool burnSE = false;

    // Attack Effects
    public bool burstAE = false;

    public bool chaos = false;

    public bool chemicals = false;

    public bool cleaveAE = false;

    public bool corrode = false;

    // Abilities
    public bool criticalDraw = false;

    public bool disabled = false;

    public bool exit = false;

    public bool fleetFeet = false;

    public bool juggling = false;

    public bool laserAE = false;

    public bool markSE = false;

    public int MOB = 2;

    // Modules
    public GameObject[] modules = new GameObject[6];

    public bool netSE = false;

    public bool nullify = false;

    public bool pierceAE = false;

    // Stats
    public int POW = 2;

    public bool purgeSE = false;

    public bool reachAE = false;

    public bool reaper = false;

    public bool ricochetAE = false;

    public int RNG = 2;

    public int ROF = 2;

    // Status Effects
    public bool slowSE = false;

    public bool splashAE = false;

    public bool stunSE = false;

    public bool swiftness = false;

    public bool traceAE = false;

    public bool whirwindAE = false;

    // Scripts
    characterAttack attack;

    // stance
    private string stance = "gun";

    public void disable()
    {
        this.disabled = true;
    }

    public void enabled()
    {
        this.disabled = false;
    }

    // functions
    public void gun()
    {
        this.stance = "gun";
        this.POW = 2;
        this.RNG = 2;
        this.AMP = 1;
    }

    public void melee()
    {
        this.stance = "melee";
        this.POW = 3;
        this.RNG = 1;
        this.AMP = 2;
    }

    public void resetStats()
    {
        if (this.stance == "gun")
        {
            this.POW = 2;
            this.ROF = 2;
            this.RNG = 2;
            this.MOB = 2;
            this.AMP = 1;
        }
        else
        {
            this.POW = 3;
            this.ROF = 2;
            this.RNG = 1;
            this.MOB = 2;
            this.AMP = 2;
        }
    }

    public void Transform()
    {
        if (this.stance == "gun")
        {
            this.POW += 1;
            this.RNG -= 1;
            this.AMP += 1;
        }
        else
        {
            this.POW -= 1;
            this.RNG += 1;
            this.AMP -= 1;
        }
    }

    public void updateAttributes(int[] inventory)
    {
        // reset stats and update character attributes
        this.resetStats();
        this.POW += inventory[0];
        this.ROF += inventory[1];
        this.RNG += inventory[2];
        this.MOB += inventory[3];
        this.AMP += inventory[4];

        bool[] temp = new bool[17];
        for (int i = 5; i < inventory.Length; i++)
        {
            if (i > 0)
            {
                temp[i - 5] = true;
            }
            else
            {
                temp[i - 5] = false;
            }
        }

        // update effects
        this.burstAE = temp[0];
        this.ricochetAE = temp[1];
        this.laserAE = temp[2];
        this.splashAE = temp[3];
        this.pierceAE = temp[4];
        this.traceAE = temp[5];
        this.cleaveAE = temp[6];
        this.whirwindAE = temp[7];
        this.reachAE = temp[8];
        this.slowSE = temp[9];
        this.stunSE = temp[10];
        this.burnSE = temp[11];
        this.markSE = temp[12];
        this.purgeSE = temp[13];
        this.breakSE = temp[14];
        this.blightSE = temp[15];
        this.netSE = temp[16];
    }

    void Awake()
    {
        this.attack = this.GetComponent<characterAttack>();
    }
}