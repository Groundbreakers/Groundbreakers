using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    public Animator anim;
    private lootDrop lootDrop;

    // Start is called before the first frame update
    void Start()
    {
        lootDrop = GameObject.Find("loot").GetComponent<lootDrop>();
    }

    public void setDirection(int i)
    {
        anim.SetInteger("Direction", i);
        lootDrop.createLoot();
    }

 

   
}
