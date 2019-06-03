using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Animator anim;
    public GameObject loot;
 
    public void setDirection(int i)
    {
        anim.SetInteger("Direction", i);
        createLoot();  // drop loot
    }

    private void createLoot()
    {   
        GameObject temp = (GameObject) Instantiate(loot, transform.position, transform.rotation);
        //temp.transform.parent = GameObject.Find("Loot").transform;
    }
}
