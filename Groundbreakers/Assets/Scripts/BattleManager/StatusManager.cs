using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public bool isEnemy;
    public bool testStunned = false;
    private characterAttack attack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //Stun();
    }

    public void Stun()
    {
        if (isEnemy)
        {

        }
        else
        {
            attack = GetComponent<characterAttack>();
            if(testStunned)
            {
                attack.stun(2);
                testStunned = false;
            }
            
        }
    }

    public void Slow()
    {
        if (isEnemy)
        {
            
        }
        else
        {

        }
    }

    public void Shackle()
    {

    }
}
