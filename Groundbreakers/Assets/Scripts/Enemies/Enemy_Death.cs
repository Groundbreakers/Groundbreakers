using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Animator anim;
    public GameObject loot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setDirection(int i)
    {
        anim.SetInteger("Direction", i);
        dropLoot();
    }

    // drop loot when an enemy is killed
    public void dropLoot() {

        GameObject temp = Instantiate(loot, transform.position, transform.rotation);
         if(temp != null) Debug.Log("yes");
         else Debug.Log("no");
    }
}
