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

    public void createLoot()
    {
        GameObject temp = (GameObject) Instantiate(loot, transform.position, transform.rotation);
    }
}
