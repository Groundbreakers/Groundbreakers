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
    public bool reflectionAE = false;
    public bool trueStrikeAE = false;
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
    public Animator animator;
    public RuntimeAnimatorController BaseColor;
    public AnimatorOverrideController[] colors = new AnimatorOverrideController[4];
    public GameObject[] modules = new GameObject[6];
    private int index = 0;
   


    void Awake()
    {
        attack = GetComponent<characterAttack>();
        updateAttributes();
    }
    //functions
    public void gun()
    {
        stance = "gun";
        updateAttributes();
    }

    public void melee()
    {
        stance = "melee";
        updateAttributes();
    }

    public void disable()
    {
        disabled = true;
    }

    public void enabled()
    {
        disabled = false;
    }

    public void changeColor()
    {
        if(index < 4)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        switch(index)
        {
            case 0:
                animator.runtimeAnimatorController = BaseColor;
                break;
            case 1:
                animator.runtimeAnimatorController = colors[0];
                break;
            case 2:
                animator.runtimeAnimatorController = colors[2];
                break;
            case 3:
                animator.runtimeAnimatorController = colors[3];
                break;
            case 4:
                animator.runtimeAnimatorController = colors[4];
                break;
        }
        
    }

    public void updateAttributes()
    {
        //reset stats and update character attributes
        resetStats();

        switch (profession)
        {
            case "Trickster":
                TricksterLevelUp();
                break;
            case "Scavenger":
                ScavengerLevelUp();
                break;
            case "Gladiator":
                GladiatorLevelUp();
                break;
            case "Scholar":
                ScholarLevelUp();
                break;
            case "Agent":
                AgentLevelUp();
                break;
        }

        //unecessary code
        if (POW > 5)
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
    }

    public void resetStats()
    {
        switch(profession)
        {
            case "Trickster":
                if (stance == "gun")
                {
                    POW = 3;
                    ROF = 2;
                    RNG = 3;
                    AMP = 2;
                }
                else
                {
                    POW = 4;
                    ROF = 1;
                    RNG = 1;
                    AMP = 3;
                }
                break;
            case "Scavenger":
                if (stance == "gun")
                {
                    POW = 3;
                    ROF = 2;
                    RNG = 3;
                    AMP = 3;
                }
                else
                {
                    POW = 3;
                    ROF = 2;
                    RNG = 1;
                    AMP = 3;
                }
                break;
            case "Gladiator":
                    POW = 2;
                    ROF = 2;
                    RNG = 1;
                    AMP = 3;
                break;
            case "Scholar":
                    POW = 1;
                    ROF = 5;
                    RNG = 3;
                    AMP = 1;
                break;
            case "Agent":
                    POW = 1;
                    ROF = 5;
                    RNG = 3;
                    AMP = 1;
                break;
        }
    }

    public void LevelUp()
    {
        if(Level < 5)
        {
            //increase Level
            Level++;
            updateAttributes();
        }

        
    }

    private void TricksterLevelUp()
    {
        switch (Level)
        {
            case 2:
                ROF += 1;
                AMP += 1;
                break;
            case 3:
                ROF += 1;
                AMP += 1;
                whirlwindAE = true;
                break;
            case 4:
                if(stance == "gun")
                {
                    RNG += 1;
                }
                POW += 1;
                ROF += 1;
                AMP += 1;
                whirlwindAE = true;
                break;
            case 5:
                if (stance == "gun")
                {
                    RNG += 1;
                }
                POW += 1;
                ROF += 1;
                AMP += 1;
                whirlwindAE = true;
                slowSE = true;
                break;
        }          
    }

    private void ScavengerLevelUp()
    {
        switch (Level)
        {
            case 2:
                POW += 1;
                AMP += 1;
                break;
            case 3:
                POW += 1;
                AMP += 1;
                multishotAE = true;
                break;
            case 4:
                POW += 1;
                AMP += 1;
                multishotAE = true;
                if (stance == "gun")
                {
                    RNG += 1;
                }
                ROF += 1;
                break;
            case 5:
                POW += 1;
                AMP += 1;
                multishotAE = true;
                if (stance == "gun")
                {
                    RNG += 1;
                }
                ROF += 1;
                burnSE = true;
                break;
        }

    }

    private void GladiatorLevelUp()
    {
        switch (Level)
        {
            case 2:
                POW += 1;
                AMP += 1;
                break;
            case 3:
                POW += 1;
                AMP += 1;
                stunSE = true;
                break;
            case 4:
                ROF += 1;
                POW += 1;
                AMP += 2;
                stunSE = true;
                break;
            case 5:
                ROF += 1;
                POW += 1;
                AMP += 2;
                stunSE = true;
                slowSE = true;
                break;
        }

    }

    private void ScholarLevelUp()
    {
        switch (Level)
        {
            case 2:
                AMP += 1;
                RNG += 1;
                break;
            case 3:
                AMP += 1;
                RNG += 1;
                slowSE = true;
                break;
            case 4:
                AMP += 1;
                RNG += 2;
                POW += 1;
                slowSE = true;
                break;
            case 5:
                AMP += 1;
                RNG += 2;
                POW += 1;
                slowSE = true;
                reflectionAE = true;
                break;
        }

    }

    private void AgentLevelUp()
    {
        switch (Level)
        {
            case 2:
                ROF += 1;
                RNG += 1;
                break;
            case 3:
                ROF += 1;
                RNG += 1;
                trueStrikeAE = true;
                break;
            case 4:
                ROF += 2;
                RNG += 1;
                POW += 1;
                trueStrikeAE = true;
                break;
            case 5:
                ROF += 2;
                RNG += 1;
                POW += 1;
                trueStrikeAE = true;
                pierceAE = true;
                break;
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
