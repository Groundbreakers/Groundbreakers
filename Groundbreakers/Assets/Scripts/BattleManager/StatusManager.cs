using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    //public boolean statuses for testing
    public bool isStunned = false;
    public bool isBlinded = false;
    public bool isPlagued = false;
    public bool isInterfered = false;
    public bool isShackled = false;
    private characterAttack attack;
    private characterAttributes character;
    private characterAbilities ability;

    void Awake()
    {
        attack = GetComponent<characterAttack>();
        character = GetComponent<characterAttributes>();
        ability = GetComponent<characterAbilities>();
    }

    void Update()
    {
        if(isStunned)
        {
            Stun();
            isStunned = false;
        }

        if(isPlagued)
        {
            Plague();
            isPlagued = false;
        }

        if(isInterfered)
        {
            Interfere();
            isInterfered = false;
        }
    }

    public void Stun()
    {
        //stop character attack
        attack.stun(2);
    }

    public void Blind()
    {
        
    }

    public void Plague()
    {
        ability.disabled = true;
        StartCoroutine(plagueDuration(2));
    }


    private IEnumerator plagueDuration(int time)
    {
        yield return new WaitForSeconds(time);
        ability.disabled = false;
    }

    public void Interfere()
    {
        character.POW = character.POW / 2;
        StartCoroutine(interfereDuration(2));
    }

    private IEnumerator interfereDuration(int time)
    {
        yield return new WaitForSeconds(time);
        character.POW = character.POW * 2;
    }

    public void Shackle()
    {
        isShackled = true;
        StartCoroutine(shackleDuration(2));
    }

    private IEnumerator shackleDuration(int time)
    {
        yield return new WaitForSeconds(time);
        isShackled = false;
    }
}
